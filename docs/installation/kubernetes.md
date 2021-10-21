---
description: >-
  The following document covers the process for deploying the Edge Delta agent
  as a DaemonSet on your Kubernetes cluster. We are assuming you have conceptual
  understanding of Kubernetes.
---

# Kubernetes

## Overview

You can use this document to learn how to install the Edge Delta Agent as a DaemonSet on your Kubernetes cluster.

The agent is a daemon that analyzes logs and container metrics from a Kubernetes cluster, and then streams analytics to configured streaming destinations.

Edge Delta uses a Kubernetes-recommended, node-level logging architecture, also known as a DaemonSet architecture. The DaemonSet runs the agent pod on each node. Each agent pod analyzes logs from all other pods running on the same node.


> **Note**
>
> This document is designed for existing users. If you have not created an account with Edge Delta, then see [Basic Onboarding](/docs/basic-onboarding.md).

***


## Step 1: Install the Agent 

1. Create a Kubernetes namespace:

```text
kubectl create namespace edgedelta
```

2. Create a kube secret that contains your API token.

```text
kubectl create secret generic ed-api-key \
    --namespace=edgedelta \
    --from-literal=ed-api-key="(log in to view API tokens)"
```

3. Based on your needs, choose an agent manifest:

| Manifest | Description |
| :---     | :---        |
| [Default](https://edgedelta.github.io/k8s/edgedelta-agent.yml) | Default Agent DaemonSet. |
| [Persisting cursor](https://edgedelta.github.io/k8s/edgedelta-agent-persisting-cursor.yml) | Agent DaemonSet with mounted host volumes to track file cursor positions persistently. |
| [Metric exporter](https://edgedelta.github.io/k8s/edgedelta-prom-agent.yml) | Agent DaemonSet exposing port 6062 /metrics endpoint in Prometheus format. See [Prometheus Scraping](../appendices/prometheus-scraping.md) |
| [On premise](https://edgedelta.github.io/k8s/edgedelta-agent-onprem.yml) | Agent DaemonSet for locally managed or offline deployments. |


4. Based on the selected manifest, create the DaemonSet with the manifest link:

```text
kubectl apply -f https://edgedelta.github.io/k8s/edgedelta-agent.yml
```

> **Note**
>
> To provide additional environment variables, download and edit [https://edgedelta.github.io/k8s/edgedelta-agent.yml](https://edgedelta.github.io/k8s/edgedelta-agent.yml), as described in [Environment Variables](https://docs.edgedelta.com/installation/environment-variables/) in Kubernetes with yaml section.


5. Check the status of the Edge Delta container:

```text
kubectl get pods --namespace=edgedelta
```

6. When the name of the pod is running the agent, run the following command to see the agent logs:

```text
kubectl logs <pod_name> -n edgedelta
```

***

## Uninstall Edge Delta DaemonSet

To uninstall the Edge Delta DaemonSet:

```text
kubectl delete daemonset edgedelta --namespace edgedelta
```

***

## Run the Agent on Specific Nodes

To run the agent on specific nodes in your cluster, add a node selector or nodeAffinity section to your pod config file. If your desired nodes are labeled logging=edgedelta, then adding the following nodeSelector will restrict the agent pods to Nodes that have the logging=edgedelta label.

```text
spec:
  nodeSelector:
    logging: edgedelta
```

> **Note**
>
> To learn more about node selectors and affinity, please review this [article from Kubernetes](https://kubernetes.io/docs/concepts/scheduling-eviction/assign-pod-node/).


***

### Special Considerations for SELinux and Openshift Users

If you are running a SELinux-enforced Kubernetes cluster, then you need to add the following securityContext configuration into edgedelta-agent.yml manifest DaemonSet spec. This update will run agent pods in privileged mode to allow the collection of logs of other pods.

```text
     runAsUser: 0
     privileged: true
```

In an OpenShift cluster, you need to also run the following commands to allow agent pods to run in privileged mode:

```text
oc adm policy add-scc-to-user privileged system:serviceaccount:edgedelta:edgedelta
oc patch namespace edgedelta -p \
'{"metadata": {"annotations": {"openshift.io/node-selector": ""}}}'
```

***

### Output to cluster services in other namespaces

Edge Delta pods run in dedicated edgedelta namespace. 

If you want to configure an output destination within your Kubernetes cluster, then you must set a resolvable service endpoint in your agent configuration.

For example, if you have an "elasticsearch-master" Elasticsearch service in the "elasticsearch" namespace with port 9200 in your "cluster-domain.example" cluster, then you need to specify the elastic output address in the agent configuration:


```text
  address:
       - http://elasticsearch-master.elasticsearch.svc.cluster-domain.example:9200
```

Read more about [service DNS resolution](https://kubernetes.io/docs/concepts/services-networking/dns-pod-service/#a-aaaa-records)

***
