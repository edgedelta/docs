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

* [Agent Stats](https://docs.edgedelta.com/configuration/inputs#agent-stats)
* [System Stats](https://docs.edgedelta.com/configuration/inputs#system-stats)
* [Container Stats \(Docker\)](https://docs.edgedelta.com/configuration/inputs#container-stats-docker)
* [Files](https://docs.edgedelta.com/configuration/inputs#files)
* [Ports](https://docs.edgedelta.com/configuration/inputs#ports)
* [Windows Events](https://docs.edgedelta.com/configuration/inputs#windows-events)
* [Containers \(Docker\)](https://docs.edgedelta.com/configuration/inputs#containers-docker)
* [Kubernetes](https://docs.edgedelta.com/configuration/inputs#kubernetes)
* [AWS ECS](https://docs.edgedelta.com/configuration/inputs#aws-ecs)
* [Kafka](https://docs.edgedelta.com/configuration/inputs#kafka)
* [Execs \(Scripted Input\)](https://docs.edgedelta.com/configuration/inputs#execs-scripted-input)
* [Kubernetes Events](https://docs.edgedelta.com/configuration/inputs#kubernetes-events)

You can specify the filters to monitor sources of containers, Kubernetes and AWS ECS.

* [Input filters](https://docs.edgedelta.com/configuration/inputs#filters-for-containers-kubernetes-and-aws-ecs)

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

If enabled, agents will collect events generated by Kubernetes (namespace independent). Note that Kubernetes Events should be used with `ED_LEADER_ELECTION_ENABLED` environment variable in order to observe events, for this purpose environment variable should be set as `ED_LEADER_ELECTION_ENABLED=1`.

```yaml
  k8s_events:
    labels: "k8s-events"
```