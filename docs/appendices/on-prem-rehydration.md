## Overview

You can use this document to learn how to deploy rehydration components to a K8s cluster.

***

## Review Related Concepts

**Archiving**
<p>
<p>By default, the Edge Delta agent archives logs on customer S3 buckets owned by Edge Delta. This action can be disabled for user-owned S3 buckets. 
<p>A custom S3 bucket (or GCS, Blob, Minio, etc.) can be created in the [integrations page](https://app.edgedelta.com/integrations) in the Edge Delta Admin portal. 
<p>After you create and add the bucket to a workflow in the agent configuration, the agent will start to send gzipped logs to the custom bucket.

**Rehydration**
<p>    
<p>Rehydration is the process of pushing already-archived data to a target streaming platform, such as Splunk, Elasticsearch, etc.
<p>You can use the **Rehydration** section of the Edge Delta Admin portal to initiate a rehydration for a specific time range, source filter, and keyword filters.
<p>By default, the Edge Delta backend handles rehydrations. 
<p> Rehydration handlers will: 
    
  * Scan the S3 bucket, then
  * Filter the logs as requested, and then 
  * Push the logs to the target streaming platform.

    
> Note
>
> If you are satisfied with the above-mentioned workflow, then you can use the rehydration feature without addtional configuration.  
    
This document explains how to run rehydration components on your own infrastructure. Specifically, if you have sensitve data that cannot leave your infrastructure, then you will need to create an on-prem rehydration setup. 
    



## On-prem Rehydrations

Rehydration components can be deployed to any K8s cluster. It uses OpenFaaS technology to handle rehydration requests and can scale out to multiple instances as needed.

Setup details are explained below. Once you complete them you will still be able to use [Rehydrations page](https://app.edgedelta.com/rehydrations) to trigger rehydrations and Edge Delta backend will not be involved in raw data handling. Edge Delta backend will serve as metadata storage for rehydrations such as input/filters/destination and status.

### Setup

Prerequisites:
- [kubectl](https://kubernetes.io/docs/tasks/tools/)
- [helm](https://helm.sh/docs/helm/helm_install/)
- [faas](https://docs.openfaas.com/cli/install/#installation)


1. Create the edgedelta-rehydration namespace:

```
kubectl create namespace edgedelta-rehydration
```

2. Install openfaas via helm chart:

```
helm repo add openfaas https://openfaas.github.io/faas-netes;
helm repo update;
helm upgrade openfaas --wait --install openfaas/openfaas \
    --namespace edgedelta-rehydration \
    -f https://raw.githubusercontent.com/edgedelta/docs/master/docs/appendices/on-prem-rehydration-helm-values.yml;
```

3. Access the Edge Delta Admin portal, specifically the [Global Settings](https://app.edgedelta.com/global-settings) page, and then create an Edge Delta token. Specifically, create a token with the following permissions: 

    * Write permission on Rehydration resources
    * Read permission on Integration resources

> **Note**
> 
> To learn how to access and create a token, see [Tokens](tokens.md).



4. Create a k8s secret to store the token:

```
kubectl create secret generic ed-rehydration-token \
  --namespace=edgedelta-rehydration \
  --from-literal=ed-rehydration-token="<token value goes here>"
```

5. Access the Edge Delta Admin portal, specifically the [Rehydrations](https://app.edgedelta.com/rehydrations) page, click Settings, and then enable **On Prem Rehydration** for your organization's rehydrations.

> **Note**
> 
> This setting may be hidden for your organization. If you do not see this option, then please contact Edge Delta. 


6. Deploy the rehydration function handler. (The external load balancer for the OpenFaaS gateway was not exposed when helm chart was installed. As a result, Edge Delta will temporarily enable port forwarding to connect to OpenFaas.)


```
kubectl port-forward -n edgedelta-rehydration svc/gateway 8080:8080
```

7. On another terminal, deploy the function handler:

```
faas deploy -f https://raw.githubusercontent.com/edgedelta/docs/master/docs/appendices/on-prem-rehydration-function.yml;
```

8. Stop the port forwarding.

9. Prepare for rehydration poller deployment yml. Specifically

- Download [this file](https://raw.githubusercontent.com/edgedelta/docs/master/docs/appendices/on-prem-rehydration-poller.yml) to a local file /tmp/rehydration-poller.yml
```
curl https://raw.githubusercontent.com/edgedelta/docs/master/docs/appendices/on-prem-rehydration-poller.yml -o /tmp/rehydration-poller.yml
```

- Put your org id as the value of `ED_ORG_ID` in /tmp/rehydration-poller.yml. You can find your org id in the url of the api requests made by app.edgedelta.com. We will expose it on the UI soon.

7. Deploy rehydration poller:

```
kubectl apply -f /tmp/rehydration-poller.yml;
```

8. Review the logs to verify a successful connection to the OpenFaaS gateway:

```
kubectl logs deployment/rehydration-poller -n edgedelta-rehydration
```

9. Return to the [Rehydrations](https://app.edgedelta.com/rehydrations) page in the portal, and create rehydration requests. 

    * The requests will be processed by the rehydration components that was just installed on your k8s cluster.

***
