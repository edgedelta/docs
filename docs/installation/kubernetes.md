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

## Step 1: Create, Download, and Install the Agent 

1. In the Edge Delta Admin Portal, on the left-side navigation, click **Agent Settings**.
2. Click **Create Configuration**. 
3. Select **Kubernetes**.
4. Click **Save**.  
5. In the table, locate the newly created agent configuration, and then click the corresponding green rocket to deploy additional instructions. 
6. Click **Kubernetes**. 
7. In the window that appears, follow the on-screen instructions. 
  - This window also displays your API key. 
  - For advanced users, there are additional installation steps that you can consider. 

***

## Step 2: Advanced Installation Instructions

For advanced users, review the following options to customize the installation process: 


<!-- 

## Step 1: Install the Agent 

1. Create a Kubernetes namespace:

```
kubectl create namespace edgedelta
```

2. Create a kube secret that contains your API token.

```
kubectl create secret generic ed-api-key \
    --namespace=edgedelta \
    --from-literal=ed-api-key="(log in to view API tokens)"
```

-->

1. Review the available agent manifest:

| Manifest          | Description                                                                                                                                                                          | URL to use in command                                                 |
|-------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------|
| Default           | This manifest is the default agent DaemonSet.                                                                                                                                        | https://edgedelta.github.io/k8s/edgedelta-agent.yml                   |
| Persisting cursor | This manifest is the agent DaemonSet with mounted host volumes to track file cursor positions persistently.                                                                          | https://edgedelta.github.io/k8s/edgedelta-agent-persisting-cursor.yml |
| Metric exporter   | This manifest is the agent DaemonSet that exposes port 6062 (metrics endpoint) in Prometheus format. To learn more, see [Prometheus Scraping](../appendices/prometheus-scraping.md). | https://edgedelta.github.io/k8s/edgedelta-prom-agent.yml              |
| On premise        | This manifest is the agent DaemonSet for locally managed or offline deployments.                                                                                                     | https://edgedelta.github.io/k8s/edgedelta-agent-onprem.yml            |


2. Based on the desired manifest, create the DaemonSet with the corresponding manifest URL:

```
kubectl apply -f https://edgedelta.github.io/k8s/edgedelta-agent.yml
```

> **Note**
>
> For additional environment variables, you can download and edit [https://edgedelta.github.io/k8s/edgedelta-agent.yml](https://edgedelta.github.io/k8s/edgedelta-agent.yml). To learn more, review the [Environment Variables](https://docs.edgedelta.com/installation/environment-variables/) document, specially the **Examples - Kubernetes (yml configuration) section**. 

<!-- 


5. Review the status of the Edge Delta container:

```
kubectl get pods --namespace=edgedelta
```

6. When the name of the pod is running the agent, run the following command to see the agent logs:

```
kubectl logs <pod_name> -n edgedelta
```

-->

> **Note**
>
> For SELinux and Openshift users, see [Special Considerations for SELinux and Openshift Users](#special-considerations-for-selinux-and-openshift-users).

***

## Run the Agent on Specific Nodes

To run the agent on specific nodes in your cluster, add a node selector or nodeAffinity section to your pod config file. For example, if the desired nodes are labeled as **logging=edgedelta**, then adding the following nodeSelector will restrict the agent pods to nodes that have the **logging=edgedelta** label.

```
spec:
  nodeSelector:
    logging: edgedelta
```

> **Note**
>
> To learn more about node selectors and affinity, please review this [article from Kubernetes](https://kubernetes.io/docs/concepts/scheduling-eviction/assign-pod-node/).


***

## Special Considerations for SELinux and Openshift Users

If you are running a SELinux-enforced Kubernetes cluster, then you need to add the following securityContext configuration into edgedelta-agent.yml manifest DaemonSet spec. This update will run agent pods in privileged mode to allow the collection of logs of other pods.

```
     runAsUser: 0
     privileged: true
```

In an OpenShift cluster, you need to also run the following commands to allow agent pods to run in privileged mode:

```
oc adm policy add-scc-to-user privileged system:serviceaccount:edgedelta:edgedelta
oc patch namespace edgedelta -p \
'{"metadata": {"annotations": {"openshift.io/node-selector": ""}}}'
```

***

## Output to Cluster Services in Other Namespaces

Edge Delta pods run in a dedicated edgedelta namespace. 

If you want to configure an output destination within your Kubernetes cluster, then you must set a resolvable service endpoint in your agent configuration.

For example, if you have an **elasticsearch-master** Elasticsearch service in the **elasticsearch** namespace with port 9200 in your **cluster-domain.example** cluster, then you need to specify the elastic output address in the agent configuration:


```text
  address:
       - http://elasticsearch-master.elasticsearch.svc.cluster-domain.example:9200
```

To learn more, please review this [article from Kubernetes](https://kubernetes.io/docs/concepts/services-networking/dns-pod-service/#a-aaaa-records).

***

## Uninstall Edge Delta DaemonSet

To uninstall the Edge Delta DaemonSet:

```
kubectl delete daemonset edgedelta --namespace edgedelta
```

***
