---
description: >-
  The following document covers the process for deploying the Edge Delta agent
  as a DaemonSet on your Kubernetes cluster via helm charts. We are assuming you
  have conceptual understanding of helm charts
---

# Kubernetes via Helm

Edge Delta agent is a daemon that analyzes logs and container metrics from a Kubernetes cluster and stream analytics to configured streaming destinations. This page streamlined instructions to get you up and running in the Kubernetes environment.

Edge Delta uses Kubernetes recommended node level logging architecture, in other words DaemonSet architecture. The DaemonSet runs Edge Delta agent pod on each node. Each Agent pod analyzes logs from all other pods running on the same node.

## Installation

Add Edge Delta helm repository

```text
helm repo add edgedelta https://edgedelta.github.io/charts
```

Run helm installation command and create "edgedelta" namespace to use agent with default parameters:

```text
helm install edgedelta edgedelta/edgedelta --set apiKey=<API-KEY> -n edgedelta --create-namespace
```

If you need to configure [Environment Variables](environment-variables.md) and other advanced options download the default [values.yml](https://edgedelta.github.io/charts/edgedelta/values.yaml) file.

You can use either apiKey or secretApiKey in values.yml file to set your API-KEY.

If you use apiKey it will be kept in clear text as part of your pod property. Change values.yml file as below:

```yaml
apiKey: "API-KEY"
```

If you want to use secretApiKey as a Kubernetes secret, change values.yml as below:

```yaml
# apiKey: ""

secretApiKey:
  name: "ed-api-key"
  key: "ed-api-key"
```

You need to create API-KEY as a Kubernetes secret using command below:

```text
kubectl create namespace edgedelta
kubectl create secret generic ed-api-key --namespace=edgedelta --from-literal=ed-api-key="API-KEY"
```

You can also add environment variables or refer secrets as environment variables using commented samples in the values.yml file.

Use below command to install helm chart using values.yml in the same folder:

```text
helm install edgedelta edgedelta/edgedelta -n edgedelta --create-namespace -f values.yaml
```

Output

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

Show helm installed packages in "edgedelta" namespace

```text
helm ls -n edgedelta
```

## Useful Tips

### Uninstall helm chart

```text
helm delete edgedelta -n edgedelta
```