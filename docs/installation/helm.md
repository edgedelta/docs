---
description: >-
  The following document covers the process for deploying the Edge Delta agent
  as a DaemonSet on your Kubernetes cluster via helm charts. We are assuming you
  have conceptual understanding of helm charts
---

## Overview

You can use this document to learn how to install the Edge Delta Agent as a DaemonSet on your Kubernetes cluster with Helm.

The agent is a daemon that analyzes logs and container metrics from a Kubernetes cluster, and then streams analytics to configured streaming destinations.

Edge Delta uses a Kubernetes-recommended, node-level log collecting architecture, also known as a DaemonSet architecture. The DaemonSet runs the agent pod on each node. Each agent pod analyzes logs from all other pods running on the same node.

> **Note**
>
> This document is designed for existing users. If you have not created an account with Edge Delta, then see [Basic Onboarding](../basic-onboarding.md).

> **Note**
>
> If you do **not** want to install the agent on your Kubernetes with Helm, then see [Kubernetes](kubernetes.md).

***


## Create a Configuration and Install the Agent via Helm

1. In the Edge Delta App, on the left-side navigation, click **Agent Settings**.
2. Click **Create Configuration**.
3. Select **Helm**.
4. Click **Save**.  
5. In the table, locate the newly created configuration, and then click the corresponding green rocket to deploy additional instructions.
6. Click **Helm**.
7. In the window that appears, follow the on-screen instructions. (This window also displays your API key.)
8. To view helm-installed packages in the **edgedelta** namespace, run the following command:

```
helm ls -n edgedelta
```

***


## Configure Helm

1.To provide **API-KEY** as clear text, set **apiKey** in values.yml

```yaml
apiKey: "API-KEY"
```

2.To provide **API-KEY** as a k8s secret, set **secretApiKey** in values.yml. Note that this secret must be created before running helm.

```yaml
secretApiKey:
  name: "ed-api-key"
  key: "ed-api-key"
```


> **Note**
>
> You can also add environment variables or refer secrets as environment variables using commented samples in the values.yml file. For additional environment variables, you can download and edit [https://edgedelta.github.io/k8s/edgedelta-agent.yml](https://edgedelta.github.io/k8s/edgedelta-agent.yml).
> To learn more, review the [Environment Variables](https://docs.edgedelta.com/installation/environment-variables/) document, specially the **Examples - Kubernetes (yml configuration) section**.

3.Review the following output:

```
NAME: edgedelta
LAST DEPLOYED: Fri Jul 17 17:49:42 2020
NAMESPACE: edgedelta
STATUS: deployed
REVISION: 1
TEST SUITE: None
NOTES:
1. Visit https://app.edgedelta.com
2. Find the configuration with <API-KEY> to check if agents are active
```


4.In the same folder, install the helm chart using values.yml:

```
helm install edgedelta edgedelta/edgedelta -n edgedelta --create-namespace -f values.yaml
```

***

## Review values.yml Parameters

| Name | Description | Example Value |
| :--- | :--- | :--- |
| apiKey | API Key used to pull agent's configuration details (generated via the Edge Delta App), should not be specified when secretApiKey is set | "8d32..." |
| secretApiKey.name | Reference to Edge Delta Agent API Key secret name in same namespace, should not be specified when apiKey is set | "ed-api-key" |
| secretApiKey.key | The secret key inside secretApiKey.name k8s secret. | "value" |
| storePort | Reference to Edge Delta Agent API Key secret key in same namespace, should not be specified when apiKey is set | "ed-api-key" |
| httpProxy | Proxy details for routing Edge Delta agent's outbound traffic through an HTTP internal proxy. For details see golang's httpproxy documentation [here](https://pkg.go.dev/golang.org/x/net/http/httpproxy). | "http://127.0.0.1:3128" |
| httpsProxy | Proxy details for routing Edge Delta agent's outbound traffic through an HTTPS internal proxy. For details see golang's httpproxy documentation [here](https://pkg.go.dev/golang.org/x/net/http/httpproxy). | "https://127.0.0.1:3128" |
| noProxy | Disable proxy for requests hitting a specific destination. For details see golang's httpproxy documentation [here](https://pkg.go.dev/golang.org/x/net/http/httpproxy). | "https://your-endpoint.com" |
| edWorkflows | Colon (:) separated workflow names that will enable all matching workflows and disable the rest together with edWorkflowPrefixes. All workflows are enabled by default when edWorkflows and edWorkflowPrefixes are unset. | "billing-workflow:error-workflow" |
| edWorkflowPrefixes | Colon (:) separated workflow prefixes that will enable all matching workflows according their prefixes and disable the rest together with edWorkflows. All workflows are enabled by default when edWorkflows and edWorkflowPrefixes are unset. | "billing:error" |
| persistingCursorProps.enabled | Enable/disable persistent cursor feature | false |
| persistingCursorProps.hostMountPath | Host mount path to keep persisting cursor state | /var/lib/edgedelta |
| persistingCursorProps.containerMountPath | Container mount path to keep persisting cursor state | /var/lib/edgedelta |
| resources.limits.cpu | Maximum cpu usage limit for agent container | 1000m |
| resources.limits.memory | Maximum memory usage limit for agent container | 512Mi |
| resources.requests.cpu| Minimum requested cpu for agent container | 200m |
| resources.requests.memory | Minimum requested memory for agent container |256Mi |
| image | Agent docker image | gcr.io/edgedelta/agent:latest |
| aggregatorProps.enabled | When set to true aggregator agent is added to the deployment. | false |
| aggregatorProps.port | Port to be used by rest of the agents to communicate with aggregator. | 9191 |

***

## Uninstall Helm Release

To uninstall the helm chart:

```
helm delete edgedelta -n edgedelta
```

***
