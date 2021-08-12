# Azure Function Monitoring Setup

Azure functions generate various telemetry data e.g. \(Traces, Error, Exceptions, RemoteDependencies and Requests\). Such telemetry data is ingested directly to application insight from Azure functions. Azure provides builtin primitive sampling techniques mentioned [here](https://docs.microsoft.com/en-us/azure/azure-monitor/app/sampling). Edge Delta provides smarter sampling options such as collecting telemetry data only from failed functions and from small percentage of successful ones. By applying such processing, Application Insight telemetry ingestion costs can be reduced drastically. Below are the steps describing deployment of Edge Delta components to an AKS cluster.

## Prepare edgedelta processor in AKS cluster

Create a new config on [admin.edgedelta.com](https://admin.edgedelta.com/) with the content from [here](https://github.com/eddocs/doc/tree/51eff2ab7f42d72460f771c88a373ea5fee305e4/appendices/aks_appinsight_trace_processor_agent_config.yaml)

Replace INSTRUMENTATION\_KEY with your the instrumentation key

Create a new AKS cluster or use an existing one

Add a new node pool on AKS with below spec. If you skip this step update the nodeSelector in [ed-appinsights-trace-processor.yaml](https://github.com/eddocs/doc/tree/51eff2ab7f42d72460f771c88a373ea5fee305e4/appendices/ed-appinsights-trace-processor.yaml).

```text
  name: processors
    OS: linux
    size: 1
    SKU: Standard_D4s_v3
```

When cluster is ready, connect to it and create Edgedelta API Key secret

```text
kubectl create namespace edgedelta
kubectl create secret generic ed-api-key \
    --namespace=edgedelta \
    --from-literal=ed-api-key="c40bafd5-xxxxxxx"
```

Create ingress resources as described [here](https://docs.microsoft.com/en-us/azure/aks/ingress-tls). Note: This step requires helm.

```text
kubectl create namespace ingress-basic
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm install nginx-ingress ingress-nginx/ingress-nginx \
    --namespace ingress-basic \
    --set controller.replicaCount=1 \
    --set controller.nodeSelector.agentpool=processors \
    --set defaultBackend.nodeSelector.agentpool=processors
```

Get IP address

```text
kubectl --namespace ingress-basic get services -o wide -w nginx-ingress-ingress-nginx-controller
```

Create a DNS zone on Azure portal as described [here](https://docs.microsoft.com/en-us/azure/dns/dns-getstarted-portal).

Note: You will need to have a public dns entry for your zone to be publicly accessible. For example if your dns zone is contoso.xyz you need to own contoso.xyz. A workaround is to create a separate AKS cluster with http application routing enabled and use its dns zone. It will have public DNS records created by Azure.

Create an A record in your DNS zone which points to IP address of ingress controller.

```text
ingest.edgedelta.<your dns zone> ->  <IP Address from above step>
```

Install cert-manager

```text
kubectl label namespace ingress-basic cert-manager.io/disable-validation=true
helm repo add jetstack https://charts.jetstack.io
helm repo update
helm install \
    cert-manager \
    --namespace ingress-basic \
    --version v0.16.1 \
    --set installCRDs=true \
    --set nodeSelector."beta\.kubernetes\.io/os"=linux \
    jetstack/cert-manager
```

Update the host values in edgedelta-ingress resource defined in [ed-appinsights-trace-processor.yaml](https://github.com/eddocs/doc/tree/51eff2ab7f42d72460f771c88a373ea5fee305e4/appendices/ed-appinsights-trace-processor.yaml) with the dns entry you configured above.

Create edgedelta http recorder and agent

```text
kubectl apply -f ed-appinsights-trace-processor.yaml`
```

Verify edgedelta pods are running

```text
kubectl get pods -n edgedelta
```

Verify certificate creation

```text
kubectl get certificate --namespace ingress-basic
```

Verify public endpoint

```text
https://ingest.edgedelta.198de54f02b345ab92a8.centralus.aksapp.io/
```

## Azure function dual telemetry write mode

Navigate to Azure application folder and add dependencies.

```text
dotnet add package Microsoft.Azure.Functions.Extensions
dotnet add package Microsoft.Extensions.Logging.ApplicationInsights
```

Create a StartUp.cs file under targeted azure function application. [Here](https://github.com/eddocs/doc/tree/51eff2ab7f42d72460f771c88a373ea5fee305e4/appendices/azure_function_startup.cs) is the content of the StartUp.cs file. Make sure to update the namepace at the top. As seen below, this custom sinker implementation called ForkingTelemetryChannel and replicates telemetry data to be ingested into secondary ingestion endpoint. It is worth to note that dual ingestion process is parallelized to reduce the overall latency.

```text
...
        public void Send(ITelemetry item)
        {
            var itemDup = item.DeepClone();
            itemDup.Context.InstrumentationKey = this.secondaryInstrumentationKey;
            Parallel.Invoke(
                () => { orginalChannel.Send(item); },
                () => { secondaryChannel.Send(itemDup); }
            );
        }
...
```

Set the secondary application insight connection string. Provide secondary endpoint address created in AKS cluster ingress endpoint. Provide secondary instrumentation key that will be used to forward the matching telemetry data to application insight from edgedelta processor.

```text
{
...
  "Values": {
...
    // Either original instrumentation key or original connection string is provided but not both
    "APPINSIGHTS_INSTRUMENTATIONKEY": "OriginalKey123";
    "APPLICATIONINSIGHTS_CONNECTION_STRING": "InstrumentationKey=OriginalKey123;IngestionEndpoint=https://dc.services.visualstudio.com",
    // Required secondary connection string for forking application insight traffic
    "APPLICATIONINSIGHTS_SECONDARY_CONNECTION_STRING": "InstrumentationKey=SecondaryKey123;IngestionEndpoint=https://ingest.edgedelta.198de54f02b345ab92a8.centralus.aksapp.io",
...
  },
...
}
```

## Azure function setup without dual writes

Set application insight connection string to point it to public endpoint created above. Use your target appinsight instrumentation key.

```text
"APPLICATIONINSIGHTS_CONNECTION_STRING": "InstrumentationKey=***;IngestionEndpoint=https://ingest.edgedelta.198de54f02b345ab92a8.centralus.aksapp.io",
```

## Next

Run azure functions and simulate failure scenarios. Visit application insights to check failed traces are forwarded by edgedelta agent.