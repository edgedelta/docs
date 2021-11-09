---
description: >-
  This document outlines the various Input types supported by the Edge Delta
  agent, and how the inputs are configured.
---

# Inputs

## Overview

You can use this document to learn about the configuration parameters available in a configuration file, specifically for **Inputs**.

An inputs tells the Edge Delta agent which data types to listen for, their location or configuration, as well as associated tags.

Each input allows you to create a label, which is used to map the input to a specific monitoring rule or streaming and alerting destination.

At a high level, there are 2 ways to manage **Inputs**:

  * If you need to create a new configuration, then you can use the visual editor to populate a YAML file, as well as make changes directly in the YAML file.
  * If you already have an existing configuration, then you can update the configuration in the YAML file. 

> **Note**
> 
> The document explains how to define an input; however, an input is not active until the input is added to a workflow. To learn how to create a workflow, see [Workflows](./workflows.md).

***

## Access Input Types

**To access the visual editor for a new configuration:** 

1. In the Edge Delta Admin portal, on the left-side navigation, click **Agent Settings**.
2. Click **Create Configuration**.
3. Click **Visual**.
4. On the right-side, select **Inputs**. 
5. Review the list of options. 

**To access the YAML file for an existing configuration:** 

1. In the Edge Delta Admin portal, on the left-side navigation, click **Agent Settings**.
2. Locate the desired configuration, and then under **Actions**, click the corresponding edit icon.
3. Review the YAML file.

***

## Review Common Options

Review common options for all input types:

| Key | Description | Required |
| :--- | :--- | :--- |
| enable_incoming_line_anomalies | When enabled, anomaly scores are also generated. | No |
| boost_stacktrace_detection | When enabled, obtains stack trace that accumulates over date patterns. | No |

***

## Review Input Types

* [Agent Stats](./inputs.md#agent-stats)
* [System Stats](./inputs.md#system-stats)
* [Container Stats \(Docker\)](./inputs.md#container-stats-docker)
* [Files](./inputs.md#files)
* [Ports](./inputs.md#ports)
* [Windows Events](./inputs.md#windows-events)
* [Containers \(Docker\)](./inputs.md#containers-docker)
* [Kubernetes](./inputs.md#kubernetes)
* [AWS ECS](./inputs.md#aws-ecs)
* [AWS S3](./inputs.md#aws-s3)
* [AWS Cloudwatch Log Events](./inputs.md#aws-cloudwatch-log-events)
* [Kafka](./inputs.md#kafka)
* [Execs \(Scripted Input\)](./inputs.md#execs-scripted-input)
* [Kubernetes Events](./inputs.md#kubernetes-events)
* [Kubernetes Stats](./inputs.md#kubernetes-stats)
* [Agent Components Health Stats](./inputs.md#agent-components-health-stats)
* [Elastic Beats](./inputs.md#elastic-beats)


***

## Agent Stats

This input type reports agent-level metrics, such as lines analyzed and bytes analyzed.

Review the following example:

```yaml
  agent_stats:
    enabled: true
    labels: "agent_stats"
```

***

## System Stats

This input type reports host-level metrics, such as CPU, memory, disk, for the host where the agent is deployed on.

Review the following example:

```yaml
  system_stats:
    enabled: true
    labels: "system_stats"
```
***

## Container Stats \(Docker\)

This input type reports container-level metrics, such as CPU, memory, disk, for each container that runs on the host.

Review the following example:

```yaml
 container_stats:
    enabled: true
    labels: "docker_stats"
```

***

## Files

This input type allows you to specify a set of files for Edge Delta to monitor. 

In **File Path**, enter the full path to the file (or files) that you want monitored. 
  * Wildcards are supported. 
  * If you want the agent process lines for a specific line separation rule (not for New Line\("\n"\)), then you need to define a "line\_pattern" regex rule.
  * If you collect the Docker container standard output logs on a file with the JSON File logging driver \([https://docs.docker.com/config/containers/logging/json-file/](https://docs.docker.com/config/containers/logging/json-file/)\), then you need define and enable docker\_mode.

Review the following example:

```yaml
  files:
    - path: "/var/log/service_a.log"
      labels: "app,service_a"
      line_pattern: "^\d{4}-\d{2}-\d{2}\s+\d{2}:\d{2}:\d{2}"
    - path: "/var/log/service_b.log"
      labels: "app,service_b"
    - path: "/var/log/apache2.access.log"
      labels: "web,apache"
    - path: "/var/log/docker/my_container/stdout/json.log"
      labels: "docker,my_container"
      docker_mode: true
```

***

## Ports

This input type allows you to specify a set of ports and protocols that the agent will listen for. 

Ports are typically used to listen to incoming traffic from:

  * Network devices (firewalls, switches, routers)
  * Time-series metrics (statsd, graphite, carbon)
  * Centralized logging architectures (rsylog, syslog-ng)

To have the agent process lines for a specific line separation rule, you need to define a "line\_pattern" regex rule. When you define a "line\_pattern" regex rule, the agent will not process lines for New Line("\n"). 

  * To learn more, see [MultiLine Detection](./appendices/line_detection.md).

Review the following example:

```yaml
  ports:
    - protocol: tcp
      port: 514
      labels: "syslog,firewall"
      line_pattern: "^\d{4}-\d{2}-\d{2}\s+\d{2}:\d{2}:\d{2}"
    - protocol: udp
      port: 1514
      labels: "syslog,router"
    - protocol: tcp
      port: 8080
      labels: "syslog,tls,service_a"
      tls:
        crt_file: /certs/server-cert.pem
        key_file: /certs/server-key.pem
        ca_file: /certs/ca.pem
    - protocol: udp
      port: 8125
      labels: "syslog, metrics, statsd"
    - protocol: tcp
      port: 2003
      labels: "syslog, metrics, graphite"
```

***

## Windows Events

This input type allows you to specify a set of Windows Events channels for Edge Delta to monitor. 

Review the following example:

```yaml
  winevents:
    - channel: "Application"
      labels: "win_events,application"
    - channel: "Security"
      labels: "win_events,security"
    - channel: "System"
      labels: "win_events,system"
    - channel: "Microsoft-Windows-Sysmon/Operational"
      labels: "win_events,sysmon"
```

***


## Containers \(Docker\)

This input type allows you to specify a set of Docker containers for Edge Delta to monitor. 

If you want the agent process lines for a specific line separation rule (not for New Line\("\n"\)), then you need to define a "line\_pattern" regex rule.

> **Note**
> 
> In the visual editor, in the **Container Include** field, note that if the value you provide after **image=** is contained anywhere in the image name, then the value will match. 

Review the following example: 

```yaml
  containers:
    - include:
        - "image=.*"
      labels: "docker, all_containers"
      line_pattern: "^\d{4}-\d{2}-\d{2}\s+\d{2}:\d{2}:\d{2}"
    - include:
        - "image=nginx:latest"
      labels: "docker, nginx"
```

***

### Filters for Containers

To specify which input to add, you must provide include/exclude regex filters. 

* All rules in the same line with a comma\(","\) separated means AND:

  ```text
   include:
     - "rule-1,rule-2"
  ```

* All rules under the same part \(include/exclude\) means OR:

  ```text
   include:
     - "rule-1"
     - "rule-2"
  ```

***

## Kubernetes

This input type allows you to specify a set of Kubernetes pods and namespaces for Edge Delta to monitor. 

> **Note**
> 
> In the visual editor, in the **Kubernetes Include** and **Kubernetes Exclude** fields, note that if the value you provide after **pod=**, **namespace=**, or **kind=** is contained anywhere in the pod or namespace name, then the value will match. 

<br>

> **Note**
> 
> The **Kubernetes Exclude** field takes precednece over the **Kubernetes Include** field. 

Review the following example:

```yaml
  kubernetes:
    - labels: "kubernetes_logs"
      include:
        - "namespace=.*"
      exclude:
        - "namespace=kube-system"
        - "namespace=kube-public"
        - "namespace=kube-node-lease"
        - "pod=edgedelta"
        - "kind=ReplicaSet"
      auto_detect_line_pattern: true
```

***

### Filters for Kubernetes 

To specify which input to add, you must provide include/exclude regex filters. 

* All rules in the same line with a comma\(","\) separated means AND:

  ```text
   include:
     - "rule-1,rule-2"
  ```

* All rules under the same part \(include/exclude\) means OR:

  ```text
   include:
     - "rule-1"
     - "rule-2"
  ```

***

## AWS ECS (ECS Containers)

This input type allows you to specify a set of ECS assets \(tasks, containers, etc.\) for Edge Delta to monitor. 

> **Note**
> 
> In the visual editor, in the **ECS Include** and **ECS Exclude** fields, note that if the value you provide after **container-name=** or **task-family=** is contained anywhere in the pod or namespace name, then the value will match. 

<br>

> **Note**
> 
> The **ECS Exclude** field takes precednece over the **ECS Include** field. 

Review the following example:

```yaml
  ecs:
    - labels: "docker_logs,all_containers"
      include:
        - container-name=.*
      exclude:
        - container-name=xray
        - container-name=edgedelta
      auto_detect_line_pattern: true
```

***

### Filters for Containers, Kubernetes and AWS ECS

To specify which input to add, you must provide include/exclude regex filters. 

* All rules in the same line with a comma\(","\) separated means AND:

  ```text
   include:
     - "rule-1,rule-2"
  ```

* All rules under the same part \(include/exclude\) means OR:

  ```text
   include:
     - "rule-1"
     - "rule-2"
  ```

***

## AWS S3 (S3 via SQS input)

This input type allows you to specify log files in an S3 bucket for Edge Delta to monitor. 

This input type depends on SQS notifications to be enabled on the target bucket. 
  * To learn how to configure S3, SQS, and IAM user for this input type, see [S3 SQS](../appendices/s3-sqs.md).

Review the following example:

```yaml
  s3_sqs:
    - labels: "my-log-files-in-s3"
      sqs_url: "https://sqs.us-west-2.amazonaws.com/<account id>/my-sqs"
      access_key_id: ""
      access_secret: ""
      region: "us-west-2" # region where the bucket and sqs queue located
```

***

## AWS CloudWatch Log Events (Cloudwatch event logs)

This input type allows you to specify a set of AWS CloudWatch Log Events for Edge Delta to monitor. 

With this input, you can monitor multiple regions and log streams. 

To define your input, review the following parameters:

| Parameter | Description | Required or Optional |
| :--- | :--- | :--- |
| AWS Region | You can define a region pattern via regex expression. For example, for all regions in United States, enter: ```region: "^us.*$"``` To monitor log events for all regions, you do not need to provide or give an all-matches pattern: ".*"     | Optional |
| Log Group | You must enter the Log Group name that is associated with the CloudWatch Logs agent. | Required |
| Log Stream | You can define log streams pattern via regex expression. For example, for streams that start with log, enter: ```log_stream: ^log.*$""``` To monitor all log events for all regions, you do not need provide or give an all-matches pattern: ".*" | Optional |

> **Note**
> 
> By default, an AWS account is not enabled with all regions. As a result, you can monitor AWS CloudWatch Log Events for all regions without defining a region in the config file; the Edge Delta Agent will obtain and monitor logs from all enabled regions in your account. To accomplish this, you must add "ec2:DescribeRegions" to your account. 
> 
> To learn more, please review the AWS document about [DescribeRegions](https://docs.aws.amazon.com/AWSEC2/latest/APIReference/API_DescribeRegions.html).

Review the following sample script to better understand how to define your input:

```yaml
 cloudwatches:
    - labels: "us-west-2_ed-log-group_admin-api"
      # region supports regex, below is for all regions in us.
      # if not defined then it means look all regions.
      region: "^us.*$"
      # you should provide log group name literally, it does not support regex.
      log_group: /ed-log-group
      # log_stream supports regex expression and if it is not provided means get all log streams.
      log_stream: "^log.*$"
      # lookback is used for how long ago to monitor log events. Default is 1 hour.
      lookback: 1h
      # interval is used for polling frequency to look new coming log events. Default is 1 minute.
      interval: 1m
      # prepend_timestamp is used to add event timestamp as a prefix of event message with a tab("\t") delimiter.
      prepend_timestamp: true
      # The maximum number of log events returned.
      # Default the maximum is as many log events as can fit in a response size of 1 MB, up to 10,000 log events.
      result_limit: 5000
```
***

## Kafka

With this input type, agents will collect events from a kafka topic.

Review the following example:

```yaml
  kafkas:
    - labels: "my-kafka-events"
      # Kafka endpoints are comma separated urls list for brokers
      endpoint: "something"
      # Kafka topic to listen to.
      topic: "topic"
      # Consumer group to isolate the consumption of topic for the agents. All agents sharing same config will be joining same consumer group.
      group_id: "my-group"
```

***

## Execs \(Scripted Input\)

This input type allows you to specify a command, set of commands, or scripts to have executed on a given frequency by Edge Delta. Then, the output of the script is  consumed by the service, similar to any other input type.

> **Note**
> 
> If the command is a single line, then the command can be provided directly in the command parameter without the need for the script section.
> If a script is preferred, then inline, you can provide the scripting type / value for the command parameter.
>   * Review the third example below. 

> **Note**
> 
> A saved script can also be referenced directly via the command parameter.
>   * Review the second example below.  

```yaml
  execs:
    - labels: "top_exec"
      name: "processes"
      labels: "top"
      command: "top"
      interval: 3m
    - labels: "foo_script_exec"
      command: /bin/bash /opt/scripts/foo.sh
      interval: "30s"
    - name: "welcomes"
      labels: "script"
      interval: 10s
      command: "/bin/sh -c"
      script: |
        for i in {1..50}
          do
            echo "Welcome $i times"
          done
```

***

## Kubernetes Events

This input type collects Kubernetes events (namespace independent) and sends these events to the Edge Delta backend. These events can be visualized on the [Insights](https://admin.edgedelta.com/insights) page.

> **Note**
> 
> > The Kubernetes pod metric collection requires an agent leader election mechanism to be enabled because only 1 agent collects the metrics from cluster.
> This mechanism is already enabled in the default agent deployment command via `ED_LEADER_ELECTION_ENABLED=1`.

Review the following example:

```yaml
  k8s_events:
    labels: "k8s-events"
```

***

## Kubernetes Stats

This input type collects and sends pod metrics, such as CPU and memory usage, to a configured streaming destination, as well as to the Edge Delta backend. 

> **Note**
> 
> This input type depends on the Kubernetes metrics API to be enabled on the cluster. To configure, you must install metrics-server. To learn more, review this [document from Kubernetes](https://github.com/kubernetes-sigs/metrics-server).

> **Note**
> 
> The Kubernetes pod metric collection requires an agent leader election mechanism to be enabled because only 1 agent collects the metrics from cluster.
> This mechanism is already enabled in the default agent deployment command via `ED_LEADER_ELECTION_ENABLED=1`.

Review the following example:

```yaml
  kubernetes_stats:
    labels: "k8s-stats"
```

***

## Agent Components Health Stats

This input type contains information about an agent's internal state, which can be useful for debugging. 

By default, this input type reports to the Edge Delta backend to help Edge Delta Support with troubleshooting agent-related issues. 

Additionally, if necessary, this input type can also report to a 3rd-party streaming destination by adding agent_components_health to a workflow.

Review the following example:

```yaml
  agent_components_health:
    labels: "agent-components-health"
```

***

## Elastic Beats

This input type allows you to specify an endpoint to have the agent listen on for incoming traffic from filebeats. This functionality is used in conjunction with the Logstash output configuration of filebeats.

Review the following sample filebeats.yml Logstash config:

```yaml
output.logstash:
  hosts: ["127.0.0.1:5044"]
```
Edge Delta Agent Input Config:

```yaml
  ports:
    - protocol: tcp
      port: 5044
      labels: "filebeats"
```

***
