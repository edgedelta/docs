## Overview


**Archiving:**
Edge Delta agents archive logs on per-customer S3 buckets owned by Edge Delta by default. This can be disabled if a user-owned S3 bucket is desired.
A custom s3 bucket (or GCS, Blob, Minio etc.) can be created on [integrations page](https://app.edgedelta.com/integrations).
Once created you can add it to a workflow in the agent config. The agents will start sending gzipped logs to the custom bucket.

**Rehydration:**
Rehydration is the process of pushing the already archived data to the target streaming platform such as Splunk, Elasticsearch etc.
[Rehydrations page](https://app.edgedelta.com/rehydrations) can be used to initiate a rehydration for a given time range, source filter and keyword filters.
Rehydrations are handled by Edge Delta backend by default. Rehydration handlers will scan the s3 bucket, filter the logs as requested and push them to the target platform.

If this flow sounds good then you can use rehydrations as is without any further configuration/deployment.
Rest of this document explains how to run rehydration components on your own infrastructure. You would only need on-prem rehydration setup if you have sensitive data that shouldn't leave your infrastructure.


## On-prem Rehydrations

Rehydration components can be deployed to any K8s cluster. It uses OpenFaaS technology to handle rehydration requests and can scale out to multiple instances as needed.
Setup details are explained below. Once you complete them you will still be able to use [Rehydrations page](https://app.edgedelta.com/rehydrations) to trigger rehydrations and Edge Delta backend will not be involved in raw data handling. Edge Delta backend will serve as metadata storage for rehydrations such as input/filters/destination and status.

### Setup

Prerequisites:
- [kubectl](https://kubernetes.io/docs/tasks/tools/)
- [helm](https://helm.sh/docs/helm/helm_install/)
- [faas](https://docs.openfaas.com/cli/install/#installation)


1. Create edgedelta-rehydration namespace

```
kubectl create namespace edgedelta-rehydration
```

2. Install openfaas via helm chart

```
helm repo add openfaas https://openfaas.github.io/faas-netes;
helm repo update;
helm upgrade openfaas --wait --install openfaas/openfaas \
    --namespace edgedelta-rehydration \
    -f https://raw.githubusercontent.com/edgedelta/docs/master/docs/appendices/on-prem-rehydration-helm-values.yml;
```


3. Create an Edge Delta token

Visit [Settings page](https://app.edgedelta.com/global-settings) and create a token with following permissions:
- Write permission on Rehydration resources
- Read permission on Integration resources


Create a k8s secret to store the token:
```
kubectl create secret generic ed-rehydration-token \
  --namespace=edgedelta-rehydration \
  --from-literal=ed-rehydration-token="<token value goes here>"
```

4. Mark your organization's rehydrations as "on-prem"

Visit app.edgedelta.com/rehydrations.
Click on Settings.
Enable On Prem Rehydration.

Note: This setting can be hidden for your org. Please contact us if that's the case.

5. Deploy rehydration function handler

We didn't expose an external load balancer for the OpenFaaS gateway when we installed helm chart. 
So in order to deploy our rehydration function we will enable port forwarding temporarily to connect to OpenFaas.

```
kubectl port-forward -n edgedelta-rehydration svc/gateway 8080:8080
```

On another terminal, deploy the function handler:
```
faas deploy -f https://raw.githubusercontent.com/edgedelta/docs/master/docs/appendices/on-prem-rehydration-function.yml;
```

Now stop the port forwarding.

6. Prepare rehydration poller deployment yml

Put your org id as the value of `ED_ORG_ID` in deploy/onprem-rehydration/deployment.yml
You can find your org id in the url of the api requests made by app.edgedelta.com. We will expose it on the UI soon.

7. Deploy rehydration poller pod

```
kubectl apply -f https://raw.githubusercontent.com/edgedelta/docs/master/docs/appendices/on-prem-rehydration-poller.yml;
```


Check its logs to see if it can successfully connect to the OpenFaaS gateway:
```
kubectl logs deployment/rehydration-poller -n edgedelta-rehydration
```

8. Now you can go to [Rehydrations page](https://app.edgedelta.com/rehydrations) to create rehydration requests. It will be processed by the rehydration components we just installed to your k8s cluster.
