# Set Up Azure Function Monitoring 

## Overview

Azure functions generate various telemetry data, such Traces, Error, Exceptions, RemoteDependencies and Requests. 

This telemetry data is ingested directly into the application insight from Azure functions. 

Azure provides built-in, primitive sampling techniques, as mentioned [in this document from Microsoft](https://docs.microsoft.com/en-us/azure/azure-monitor/app/sampling). 

Edge Delta provides smarter sampling options, such as collecting telemetry data specifically from failed functions and from a small percentage of successful ones. By applying such capability, Application Insight telemetry ingestion costs can be reduced drastically. 

This document explains how to deploy Edge Delta components to an AKS cluster.

***

## Step 1: Set Up the Edge Delta Processor in an AKS Cluster

1. Create a new config in the [Edge Delta Admin portal](https://admin.edgedelta.com/) with the content from [this file](https://raw.githubusercontent.com/edgedelta/docs/master/docs/appendices/aks_appinsight_trace_processor_agent_config.yaml).

2. Replace **INSTRUMENTATION\_KEY** with your the instrumentation key.

3. Create a new AKS cluster or use an existing cluster.

4. Add a new node pool on AKS with the sepcs below. 

   * If you skip this step, then update the nodeSelector in [ed-appinsights-trace-processor.yaml](https://raw.githubusercontent.com/edgedelta/docs/master/docs/appendices/ed-appinsights-trace-processor.yaml).

```text
  name: processors
    OS: linux
    size: 1
    SKU: Standard_D4s_v3
```

5. When cluster is ready, connect to to the cluster, and then create the Edge Delta Secret API Key:

```text
kubectl create namespace edgedelta
kubectl create secret generic ed-api-key \
    --namespace=edgedelta \
    --from-literal=ed-api-key="c40bafd5-xxxxxxx"
```

6. Create the ingress resources described [in this document from Microsoft](https://docs.microsoft.com/en-us/azure/aks/ingress-tls). 

  * This step requires helm.

```text
kubectl create namespace ingress-basic
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm install nginx-ingress ingress-nginx/ingress-nginx \
    --namespace ingress-basic \
    --set controller.replicaCount=1 \
    --set controller.nodeSelector.agentpool=processors \
    --set defaultBackend.nodeSelector.agentpool=processors
```

7. Obtain the IP address:

```text
kubectl --namespace ingress-basic get services -o wide -w nginx-ingress-ingress-nginx-controller
```

8. Create a DNS zone on the Azure portal, as described [in this document from Microsoft](https://docs.microsoft.com/en-us/azure/dns/dns-getstarted-portal).

> **Note**
> 
>  You will need to have a public DNS entry for your zone to be publicly accessible. For example, if your DNS zone is **contoso.xyz**, then you need to own **contoso.xyz**. As a workaround, you can create a separate AKS cluster with http application routing enabled and use that DNS zone. It will have public DNS records created by Azure.

9. Create an A record in your DNS zone which points to the IP address of ingress controller:

```text
ingest.edgedelta.<your dns zone> ->  <IP Address from above step>
```

10. Install cert-manager:

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

11. With the DNS entry you configured earlier, update the host values in the edgedelta-ingress resource, as defined in [ed-appinsights-trace-processor.yaml](https://raw.githubusercontent.com/edgedelta/docs/master/docs/appendices/ed-appinsights-trace-processor.yaml).

12. Create the Edge Delta http recorder and agent: 

```text
kubectl apply -f ed-appinsights-trace-processor.yaml`
```

13. Verify that edgedelta pods are running:

```text
kubectl get pods -n edgedelta
```

14. Verify certificate creation:

```text
kubectl get certificate --namespace ingress-basic
```

15. Verify public endpoint:

```text
https://ingest.edgedelta.198de54f02b345ab92a8.centralus.aksapp.io/
```

***

## Step 2: Set Up Azure Function With Dual Telemetry Write Mode

1. Navigate to the Azure application folder, and then add dependencies:

```text
dotnet add package Microsoft.Azure.Functions.Extensions
dotnet add package Microsoft.Extensions.Logging.ApplicationInsights
```

2. Create a StartUp.cs file under the targeted Azure function application. Use the content in this [StartUp.cs file](https://raw.githubusercontent.com/edgedelta/docs/master/docs/appendices/azure_function_startup.cs). 

  * Update the namepace at the top. In the example below, the custom sinker implementation called ForkingTelemetryChannel replicates telemetry data for ingestion into a secondary ingestion endpoint. Note that the dual ingestion process is parallelized to reduce the overall latency.

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

3. Set the secondary application insight connection string. 

  * Provide the secondary endpoint address created in the AKS cluster ingress endpoint. 
  * Provide the secondary instrumentation key that will be used to forward the matching telemetry data to the application insight from the Edge Delta processor.

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

***

## Step 3: Set Up Azure Function Without Dual Writes

1. Set the application insight connection string to point to the public endpoint previously. Use your target appinsight instrumentation key.

```text
"APPLICATIONINSIGHTS_CONNECTION_STRING": "InstrumentationKey=***;IngestionEndpoint=https://ingest.edgedelta.198de54f02b345ab92a8.centralus.aksapp.io",
```

***

## Step 4: Test the Setup

1. Run the Azure functions and simulate failure scenarios. 
2. Visit application insights to check if failed traces are forwarded by the Edge Delta agent.

***
