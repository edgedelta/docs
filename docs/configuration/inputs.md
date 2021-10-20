---
description: >-
  This document outlines the various Input types supported by the Edge Delta
  agent, and how the inputs are configured.
---

# Inputs

## Overview

Inputs are the mechanism that tells the Edge Delta agent which data types for it to listen to, their location or configuration, as well as associated tags.

The labels are used to map inputs to specific monitoring rules, or streaming and alerting destinations.

There are a number of different input types supported by the Edge Delta service. Select from the following input types below to review the appropriate documentation:

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

You can specify the filters to monitor sources of containers, Kubernetes and AWS ECS.

* [Input filters](./inputs.md#filters-for-containers-kubernetes-and-aws-ecs)

There are some common options for all input types:

| Key | Description | Required |
| :--- | :--- | :--- |
| enable_incoming_line_anomalies | If enabled then anomaly scores are also generated | No |
| boost_stacktrace_detection | If enabled, gets stack trace that accumulates over date patterns. | No |


_Note:_ The example input configurations in this document demonstrate how to define an input. You must put the input in a [workflow](./workflows.md) for it to be active

## Agent Stats

If enabled, Agent Stats will report agent level metrics, such as lines analyzed, bytes analyzed, etc.

```yaml
  agent_stats:
    enabled: true
    labels: "agent_stats"
```

## System Stats

If enabled, System Stats will report host level metrics, such as CPU, Memory, Disk, etc. for the host the agent is deployed on.

```yaml
  system_stats:
    enabled: true
    labels: "system_stats"
```

## Container Stats \(Docker\)

If enabled, Container Stats will report container level metrics, such as CPU, Memory, Disk, etc. for each container running on the host.

```yaml
 container_stats:
    enabled: true
    labels: "docker_stats"
```

## Files

If enabled, Files allows you to specify a set of files to have monitored by the Edge Delta service.

Provide the full path to the file\(s\) you want to monitor. Wildcards are supported.

If you want the agent to process lines not for New Line\("\n"\) but for a specific line separation rule then you need to define a "line\_pattern" regex rule.

If you collect the docker container standard output logs on a file with "JSON File logging driver" \([https://docs.docker.com/config/containers/logging/json-file/](https://docs.docker.com/config/containers/logging/json-file/)\) then you need define and enable docker\_mode.

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

## Ports

If enabled, Ports allows you to specify a set of ports and protocols to have the agent listen on for incoming traffic. Ports are typically used to listen for traffic from network devices \(firewalls, switches, routers, ...\), time-series metrics \(statsd, graphite, carbon, ...\), as well as centralized logging architectures \(rsyslog, syslog-ng, ...\).

If you want the agent to process lines not for New Line\("\n"\) but for a specific line separation rule then you need to define a "line\_pattern" regex rule.

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

## Windows Events

If enabled, Windows Events allows you to specify a set of Windows Events channels to be monitored by the Edge Delta service.

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

## Filters for Containers, Kubernetes and AWS ECS

You can specify which inputs to add by providing include/exclude regex filters. These filters work only with Containers, Kubernetes and AWS ECS input types.

* All rules in the same line with a comma\(","\) separated means AND

  ```text
   include:
     - "rule-1,rule-2"
  ```

* All rules under the same part \(include/exclude\) means OR

  ```text
   include:
     - "rule-1"
     - "rule-2"
  ```

## Containers \(Docker\)

If enabled, Containers allows you to specify a set of Docker Containers to be monitored by the Edge Delta service.

If you want the agent to process lines not for New Line\("\n"\) but for a specific line separation rule then you need to define a "line\_pattern" regex rule.

**Note**: In the 'include' section, after "image=" this is a contains match, so as long as the value provided is contained anywhere in the image name, the value will match.

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

## Kubernetes

If enabled, the Kubernetes Input allows you to specify a set of Kubernetes pods and namespaces to be monitored by the Edge Delta service.

**Note**: In the 'include' and 'exclude' section, after "pod=", "namespace=" or "kind=" this is a contains match, so as long as the value provided is contained anywhere in the pod or namespace name, the value will match.

**Note**: Excluded pods/namespaces/kinds take precedence over Included pods/namespaces/kinds.

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

## AWS ECS

If enabled, the ECS Input allows you to specify a set of ECS assets \(tasks, containers, etc.\) to be monitored by the Edge Delta service.

**Note**: In the 'include' and 'exclude' section, after "container-name=", or "task-family=" this is a contains match, so as long as the value provided is contained anywhere in the container or task name, the value will match.

**Note**: Excluded container/task take precedence over Included container/task.

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

## AWS S3

If enabled, log files in an S3 bucket can be monitored by the Edge Delta service. This input type depends on SQS notifications to be enabled on the target bucket. 
Details around how to configure S3, SQS and IAM user for this input type can be found [here](../appendices/s3-sqs.md).

```yaml
  s3_sqs:
    - labels: "my-log-files-in-s3"
      sqs_url: "https://sqs.us-west-2.amazonaws.com/<account id>/my-sqs"
      access_key_id: ""
      access_secret: ""
      region: "us-west-2" # region where the bucket and sqs queue located
```

## AWS CloudWatch Log Events 

You can use the AWS CloudWatch input to specify a set of AWS CloudWatch Log Events that Edge Delta will monitor.  

With the AWS CloudWatch input, you can monitor multiple regions and log streams. 

Review the following parameters that you can use to define your input:  

- Region (Optional)
    - You can define a region pattern via regex expression. For example, for all regions in United States, use: ```region: "^us.*$"```
    - To monitor log events for all regions, you do not need to provide or give an all-matches pattern ".*".
- Log Group
    - You must enter the Log Group name that is associated with the CloudWatch Logs agent.
- Log Stream (Optional)
    - You can define log streams pattern via regex expression. For example, for streams that start with log, use ```log_stream: ^log.*$""```
    - To monitor all log events for all regions, you do not need provide or give an all-matches pattern ".*".

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

## Kafka

If enabled, agents will collect events from a kafka topic.


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

## Execs \(Scripted Input\)

If enabled, the Execs Input allows you to specify a command, set of commands, or scripts to have executed on a given frequency by the service. The output of the script is then consumed by the service, similar to any other input type.

**Note**: If the command is a single line, the command can be provided directly in the command parameter, without the need for the script section. If a script is preferred, provide the scripting type / value for the command parameter, and the script inline \(see third example below\).

A saved script can also be referenced directly via the command parameter \(see second example below\).

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

## Kubernetes Events

If enabled, Kubernetes events will be collected (namespace independent) and sent to Edge Delta backend. These events can be visualized on the [Insights](https://admin.edgedelta.com/insights) page.

_Note:_ Kubernetes event collection requires agent leader election mechanism to be enabled because only one agent collects the events from cluster. This mechanism is already enabled in the default agent deployment commands via `ED_LEADER_ELECTION_ENABLED=1`.

```yaml
  k8s_events:
    labels: "k8s-events"
```

## Kubernetes Stats

If enabled, pod metrics such as cpu/memory usage are collected and sent to configured streaming destinations as well as Edge Delta backend. 


_Note:_ This input type depends on the Kubernetes metrics API to be enabled on the cluster which can be configured by installing [metrics-server](https://github.com/kubernetes-sigs/metrics-server).

_Note:_ Kubernetes pod metric collection requires agent leader election mechanism to be enabled because only one agent collects the metrics from cluster. This mechanism is already enabled in the default agent deployment commands via `ED_LEADER_ELECTION_ENABLED=1`.

```yaml
  kubernetes_stats:
    labels: "k8s-stats"
```

## Agent Components Health Stats

Agent component health data contains information about agent's internal state and can be useful for debugging. If enabled, Agent Components Health Stats will report to Edge Delta backend by default to help our support team for troubleshooting agent related issues. It can also report to a 3rd party streaming destination if needed by adding agent_components_health to a workflow.

```yaml
  agent_components_health:
    labels: "agent-components-health"
```

## Elastic Beats

If enabled, Elastic Beats allows you to specify an endpoint to have the agent listen on for incoming traffic from filebeats. This functionality is used in conjunction with the Logstash output configuration of filebeats.

Sample filebeats.yml Logstash Config:

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
