---
description: >-
  This document describes enabling prometheus scraping from the Edge Delta
  Agent.
---

# Prometheus Scraping

The Edge Delta Agent has the ability to expose its metrics to be scraped by Prometheus for organizations who want to also keep their existing monitoring pipeline.

## Metrics Exposed to Prometheus

Prometheus mainly supports two types of metrics:

* **Counters**: Ever increasing counts or sums, where the consumer is mostly interested in the rate of increase in unit time. Example: errors in the last 10 seconds.
* **Gauges**: Sampled numeric value that might increase or decrease. Example: number of processes, temperature, Edge Delta anomaly score.

Edge Delta agent exposes **\_counts** metrics collected as counters.

Edge Delta agent exposes below metrics as gauges:

* **\_avg**
* **\_min**
* **\_max**
* **\_anomaly1**

For further information on metric types and configuring processors to create them see: [processors](../configuration/processors.md)

Exposed metrics have labels to be used in Prometheus queries that identify the metric sources.

### Example anomaly1 gauge in Prometheus format

Actual line is split and indented for readability:

```text
provision_failure_anomaly1 {
  metricType="stat",
  tag="api-backend",
  workflow="api-workflow",
  sourceType="K8s",
  logicalSource="K8s,default,ReplicaSet,api-deployment,admin",
  pod_name="api-deployment-****",
  pod_id="****",
  region="us-****",
  controllerLogicalName="api-deployment",
  docker_id="d7****",
  namespace_name="default",
  controllerKind="ReplicaSet",
  labels_pod_template_hash="d4****",
  container_name="api",
  controllerName="api-deployment-d4****",
  environment="staging",
  container_image="****",
  host="****",
  labels_app="api"
} 5.000000 1622638103171
```

* provision\_failure\_anomaly1 is the metric name
* metricType="stat" denote that its is a processor collected metric
* 5 is the counter value
* 1622638103171 is the timestamp \(only available in gauges\)
* Rest of the labels identify the metrics source which is a K8s pod
* Counters also have same structure with gauges but metrics name ends with \_count

### Example cluster counter in Prometheus format

If you enable the cluster processor in one or more of your workflows, clustered log pattern counts will be exposed as counters. Actual line is split and indented for readability, source labels are hidden:

```text
debug_span_operation_getcachedorgsetting_latency_ms_reqid_userid_x_forwarded_for_x_host_id {
  tag="admin-api-backend",
  workflow="api-workflow",
  pattern="* DEBUG span* Operation getCachedOrgSetting latency *ms reqID * userID * X Forwarded For * X Host ID *",
  metricType="cluster",
  ...
} 1100.000000
```

* metricType="cluster" denote that this is a cluster count
* pattern=`"* DEBUG span* Operation getCachedOrgSetting latency *ms reqID * userID * X Forwarded For * X Host ID *"` is the log pattern signature
* debug\_span\_operation\_getcach... is the metric name which is the  Prometheus format compliant version of the pattern value.
* 1100 is the number of occurrences of the log pattern seen so far on that source.

## Exposing Agent Metrics

Agent requires STORE\_PORT environment variable to be set to expose the metrics on [http://AGENT\_IP:STORE\_PORT/metrics](http://AGENT_IP:STORE_PORT/metrics) HTTP endpoint in Prometheus format. See [Environment Variables](../installation/environment-variables.md) for how to set environment variables in different platforms.

Once exposed and metrics endpoint is reachable by Prometheus, it can be scraped like any other Prometheus metrics source.

## Kubernetes Deployment

Prometheus usage is very common on Kubernetes since most of the internal components also support it. You can use Prometheus specific agent deployment manifest at "Create daemonset" step of [Kubernetes Installation](../installation/kubernetes.md) as seen below, then complete the rest of the steps there:

```text
kubectl apply -f https://edgedelta.github.io/k8s/edgedelta-prom-agent.yml
```

If you use [Prometheus Service Operator](https://github.com/prometheus-operator/prometheus-operator) Helm chart to deploy Prometheus you can also deploy the Edge Delta Agent service monitor configuration to allow Prometheus to discover the agents automatically following below steps:

* Download [https://edgedelta.github.io/k8s/edgedelta-prom-servicemonitor.yml](https://edgedelta.github.io/k8s/edgedelta-prom-servicemonitor.yml)
* Check Helm release name and namespace of Prometheus Operator deployment as below, find CHART starting with prometheus-operator:

  ```bash
  helm ls --all-namespaces
  Expected Output:

  NAME      NAMESPACE     REVISION    UPDATED                                 STATUS      CHART                        APP VERSION
  promop    monitoring    1           2020-08-28 19:49:30.516141 +0300 +03    deployed    prometheus-operator-9.3.1    0.38.1
  ```

  Namespace is "monitoring" and the release name is "promop" in the above example.

* Update “promop” in value in edgedelta-prom-servicemonitor.yml with your helm release name for prometheus if you give another release name.

  ```yaml
  release: promop
  ```

* Find the service monitor name prefix. To get the release name prefix for monitors run\(change monitoring to your namespace if it is different\):

  ```bash
  kubectl get servicemonitor -n monitoring
  ```

  Expected Output:

  ```bash
  NAME                                                 AGE
  promop-prometheus-operator-alertmanager              1d
  ...
  promop-prometheus-operator-prometheus                1d
  ```

* Update the service monitor name and namespace to match the release name prefix in edgedelta-prom-servicemonitor.yml file:

  ```yaml
  name: promop-prometheus-operator-edgedelta
  namespace: monitoring
  ```

  As seen above all service monitors starts with promop-prometheus-operator so our service monitor name should be "promop-prometheus-operator-edgedelta".

* Run below command to deploy service monitor

  ```text
  kubectl apply -f edgedelta-prom-servicemonitor.yml
  ```

  Expected Output:

  ```text
  servicemonitor.monitoring.coreos.com/promop-prometheus-operator-edgedelta created
  service/edgedelta-metrics created
  ```

  After a short while metrics should be available on Prometheus.