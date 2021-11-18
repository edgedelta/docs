# Set Up On-Prem Rehydration

## Overview

You can use this document to learn how to deploy and run rehydration components on your own infrastructure (on-prem).

This process is useful if you have sensitive data that cannot leave your infrastructure.

You can deploy an on-prem rehydration with or without the OpenFaaS dependency.

> **Note**
>
> OpenFaaS depends on apiextensions.k8s.io/v1beta1, which is compatible with Kubernetes v1.16 and higher.
>
> Edge Delta recommends that you deploy with the OpenFaaS dependency; however, if your cluster is older than v1.16, then you must deploy without the OpenFaaS dependency.

***

## Understand Rehydration Concepts

**Archiving**

By default, the Edge Delta agent archives logs on customer S3 buckets owned by Edge Delta. 

  * This action can be disabled for user-owned S3 buckets. 

A custom S3 bucket (or GCS, Blob, Minio, etc.) can be created in the Integrations page of the Edge Delta Admin portal. 
After you create and add the bucket to a workflow in the agent configuration, the agent will start to send gzipped logs to the custom bucket.

**Rehydration**

Rehydration is the process of pushing already-archived data to a target streaming platform, such as Splunk, Elasticsearch, etc.

You can use the Rehydration section of the Edge Delta Admin portal to initiate a rehydration for a specific time range, source filter, and keyword filters.

By default, the Edge Delta backend handles rehydrations. Rehydration handlers will: 
    
  * Scan the S3 bucket, then
  * Filter the logs as requested, and then 
  * Push the logs to the target streaming platform.

    
Rehydration components can be deployed to any K8s cluster. OpenFaaS technology is used to handle rehydration requests and can scale out to multiple instances as needed.

After you follow the steps below, you will still be able to use the **Rehydrations** page to trigger rehydrations and the Edge Delta backend will **not** be involved in the handling of raw data. The Edge Delta backend will simply serve as metadata storage for rehydrations, such as input/filters/destination and status.
    
***    

## Deploy with OpenFaaS

Edge Delta recommends that you deploy with the OpenFaaS dependency.

***

### Step 1: Review Pre-Deployment Considerations
    
Review the following prerequisites:  

- [kubectl](https://kubernetes.io/docs/tasks/tools/)
- [helm](https://helm.sh/docs/helm/helm_install/)
- [faas](https://docs.openfaas.com/cli/install/#installation)

***

### Step 2: Deploy an On-Prem Rehydration 
    
1.Create the edgedelta-rehydration namespace:

```
kubectl create namespace edgedelta-rehydration
```

2.Install openfaas via helm chart:

```
helm repo add openfaas https://openfaas.github.io/faas-netes;
helm repo update;
helm upgrade openfaas --wait --install openfaas/openfaas \
    --namespace edgedelta-rehydration \
    -f https://raw.githubusercontent.com/edgedelta/docs/master/docs/appendices/on-prem-rehydration-helm-values.yml;
```

3.Access the Edge Delta Admin portal, specifically the [Global Settings](https://app.edgedelta.com/global-settings) page, and then create an Edge Delta token with the following permissions: 

  - Write permission on Rehydration resources
  - Read permission on Integration resources

> **Note**
> 
> To learn how to access and create a token, see [Tokens](tokens.md).

4.Create a k8s secret to store the token:

```
kubectl create secret generic ed-rehydration-token \
  --namespace=edgedelta-rehydration \
  --from-literal=ed-rehydration-token="<token value goes here>"
```

5.Access the Edge Delta Admin portal, specifically the [Rehydrations](https://app.edgedelta.com/rehydrations) page, click **Settings**, and then enable **On Prem Rehydration** for your organization's rehydrations.

> **Note**
> 
> This setting may be hidden for your organization. If you do not see this option, then please contact Edge Delta. 

6.Deploy the rehydration function handler. (The external load balancer for the OpenFaaS gateway was not exposed when helm chart was installed. As a result, you need to temporarily enable port forwarding to connect to OpenFaaS.)

```
kubectl port-forward -n edgedelta-rehydration svc/gateway 8080:8080
```

7.On another terminal, deploy the function handler:

```
faas deploy -f https://raw.githubusercontent.com/edgedelta/docs/master/docs/appendices/on-prem-rehydration-function.yml;
```

8.Stop port forwarding.

9.Deploy the rehydration poller YML. Specifically:

- Download poller YML to a local file /tmp/rehydration-poller.yml
    
```
curl https://raw.githubusercontent.com/edgedelta/docs/master/docs/appendices/on-prem-rehydration-poller.yml -o /tmp/rehydration-poller.yml
```

- Put your organization ID as the value of `ED_ORG_ID` in /tmp/rehydration-poller.yml. You can find your organization ID in the URL of the API requests made by app.edgedelta.com. 

10.Deploy rehydration poller:

```
kubectl apply -f /tmp/rehydration-poller.yml;
```

11.Review the logs to verify a successful connection to the OpenFaaS gateway:

```
kubectl logs deployment/rehydration-poller -n edgedelta-rehydration
```


12.Return to the [Rehydrations](https://app.edgedelta.com/rehydrations) page in the portal, and then create rehydration requests. 

The requests will be processed by the rehydration components that was just installed on your k8s cluster.

***

## Deploy Without OpenFaaS

OpenFaaS depends on apiextensions.k8s.io/v1beta1, which is compatible with Kubernetes v1.16 and higher. As a result, if your cluster is older than v1.16, then you can use the following instructions to deploy without installing the OpenFaaS components.

***

### Step 1: Review Pre-Deployment Considerations
    
Review the following prerequisites:  

- [kubectl](https://kubernetes.io/docs/tasks/tools/)

***

### Step 2: Deploy an On-Prem Rehydration 

1.Create the edgedelta-rehydration namespace:

```
kubectl create namespace edgedelta-rehydration
```

2.Access the Edge Delta Admin portal, specifically the [Global Settings](https://app.edgedelta.com/global-settings) page, and then create an Edge Delta token with the following permissions: 

    * Write permission on Rehydration resources
    * Read permission on Integration resources

> **Note**
> 
> To learn how to access and create a token, see [Tokens](tokens.md).

3.Create a k8s secret to store the token:

```
kubectl create secret generic ed-rehydration-token \
  --namespace=edgedelta-rehydration \
  --from-literal=ed-rehydration-token="<token value goes here>"
```

4.Access the Edge Delta Admin portal, specifically the [Rehydrations](https://app.edgedelta.com/rehydrations) page, click **Settings**, and then enable **On Prem Rehydration** for your organization's rehydrations.

> **Note**
> 
> This setting may be hidden for your organization. If you do not see this option, then please contact Edge Delta. 

5.Deploy the rehydration handler:

```
kubectl apply -f https://raw.githubusercontent.com/edgedelta/docs/master/docs/appendices/on-prem-rehydration-handler-faasless.yml;
```

6.Deploy the rehydration poller YML. Specifically:

- Download poller YML to a local file /tmp/rehydration-poller.yml
    
```
curl https://raw.githubusercontent.com/edgedelta/docs/master/docs/appendices/on-prem-rehydration-poller-faasless.yml -o /tmp/rehydration-poller-faasless.yml
```

- Put your organization ID as the value of `ED_ORG_ID` in /tmp/rehydration-poller-faasless.yml. You can find your organization ID in the URL of the API requests made by app.edgedelta.com. 

7.Deploy rehydration poller:

```
kubectl apply -f /tmp/rehydration-poller-faasless.yml;
```

8.Review the logs to verify a poller has no errors:

```
kubectl logs deployment/rehydration-poller -n edgedelta-rehydration
```


10.Return to the [Rehydrations](https://app.edgedelta.com/rehydrations) page in the portal, and then create rehydration requests. 

The requests will be processed by the rehydration components that was just installed on your k8s cluster.

***

