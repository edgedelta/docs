---
description: >-
  The following document covers the process for deploying the Edge Delta agent
  as a DaemonSet on your Kubernetes cluster via helm charts. We are assuming you
  have conceptual understanding of helm charts
---

## Overview

You can use this document to learn how to install the Edge Delta Agent as a DaemonSet on your Kubernetes cluster with Helm.

The agent is a daemon that analyzes logs and container metrics from a Kubernetes cluster, and then streams analytics to configured streaming destinations.

Edge Delta uses a Kubernetes-recommended, node-level logging architecture, also known as a DaemonSet architecture. The DaemonSet runs the agent pod on each node. Each agent pod analyzes logs from all other pods running on the same node.

> **Note**
>
> This document is designed for existing users. If you have not created an account with Edge Delta, then see [Basic Onboarding](/docs/basic-onboarding.md).

> **Note**
>
> If you do **not** want to install the agent on your Kubernetes with Helm, then see [Kubernetes](kubernetes.md).

***

## Add and Configure Helm

1. Add the Edge Delta Helm repository:

```text
helm repo add edgedelta https://edgedelta.github.io/charts
```

2. Run the helm installation command, and then create the **edgedelta** namespace to use the Edge Delta Agent with default parameters:

```text
helm install edgedelta edgedelta/edgedelta --set apiKey=<API-KEY> -n edgedelta --create-namespace
```

3. To set your **API-KEY**, you can use either **apiKey** or **secretApiKey** in the values.yml file.

  - To use **apiKey** as a Kubernetes secret, change the values.yml file: 

```yaml
apiKey: "API-KEY"
```

> **Note**
> 
> **apiKey** will be kept in clear text as part of your pod property.

  - To use **secretApiKey** as a Kubernetes secret, change the values.yml file: 

```yaml
# apiKey: ""

secretApiKey:
  name: "ed-api-key"
  key: "ed-api-key"
```

4. Create **API-KEY** as a Kubernetes secret:

```text
kubectl create namespace edgedelta
kubectl create secret generic ed-api-key --namespace=edgedelta --from-literal=ed-api-key="API-KEY"
```

> **Note**
>
> You can also add environment variables or refer secrets as environment variables using commented samples in the values.yml file. For additional environment variables, you can download and edit [https://edgedelta.github.io/k8s/edgedelta-agent.yml](https://edgedelta.github.io/k8s/edgedelta-agent.yml). To learn more, review the [Environment Variables](https://docs.edgedelta.com/installation/environment-variables/) document, specially the **Examples - Kubernetes (yml configuration) section**. 


5. Install helm chart using values.yml in the same folder:

```text
helm install edgedelta edgedelta/edgedelta -n edgedelta --create-namespace -f values.yaml
```

6. Review the following output: 

```text
NAME: edgedelta
LAST DEPLOYED: Fri Jul 17 17:49:42 2020
NAMESPACE: edgedelta
STATUS: deployed
REVISION: 1
TEST SUITE: None
NOTES:
1. Visit https://admin.edgedelta.com
2. Find the configuration with <API-KEY> to check if agents are active
```

7. View helm-installed packages in the "edgedelta" namespace:

```text
helm ls -n edgedelta
```

***

## Review Value.yml Parameters

| Name | Description | Example Value |
| :--- | :--- | :--- |
| apiKey | API Key used to pull agent's configuration details (generated via ED Admin Portal), should not be specified when secretApiKey is set | "8d32..." |
| secretApiKey.name | Reference to Edge Delta Agent API Key secret name in same namespace, should not be specified when apiKey is set | "ed-api-key" |
| storePort | Reference to Edge Delta Agent API Key secret key in same namespace, should not be specified when apiKey is set | "ed-api-key" |
| httpProxy | Proxy details for routing Edge Delta agent's outbound traffic through an HTTP internal proxy | "http://127.0.0.1:3128" |
| httpsProxy | Proxy details for routing Edge Delta agent's outbound traffic through an HTTPS internal proxy | "https://127.0.0.1:3128" |
| noProxy | Disable proxy for requests hitting a specific destination | "https://your-endpoint.com" |
| edWorkflows | Colon (:) separated workflow names that will enable all matching workflows and disable the rest together with edWorkflowPrefixes | "billing-workflow:error-workflow" |
| edWorkflowPrefixes | Colon (:) separated workflow prefixes that will enable all matching workflows according their prefixes and disable the rest together with edWorkflows | "billing:error" |
| persistingCursorProps.enabled | Enable/disable persistent cursor feature | false |
| persistingCursorProps.hostMountPath | Host mount path to keep persisting cursor state | /var/lib/edgedelta |
| persistingCursorProps.containerMountPath | Container mount path to keep persisting cursor state | /var/lib/edgedelta |
| resources.limits.cpu | Maximum cpu usage limit for agent pod | 1000m |
| resources.limits.memory | Maximum memory usage limit for agent pod | 512Mi |
| resources.requests.cpu| Minimum requested cpu for agent pod | 200m |
| resources.requests.memory | Minimum requested memory for agent pod |256Mi |
| image | Agent docker image | edgedelta/agent |

***

## Uninstall helm chart

To uninstall the helm chart:

```text
helm delete edgedelta -n edgedelta
```

***
