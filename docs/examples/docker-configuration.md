---
description: >-
  The following document contains an example configuration for deployment on
  Docker-based Operating Systems
---

# Docker Configuration

The example configuration is a default, working configuration requiring no changes to be deployed.

Examples of initial changes to customize to your environment would be adding specific [Inputs](../configuration/inputs.md), [Processors](../configuration/processors.md), or [Outputs](../configuration/outputs.md) to various [Streaming](../configuration/outputs.md#streaming-destinations) and [Trigger](../configuration/outputs.md#trigger-destinations) destinations.

Please comment/uncomment parameters as needed, as well as populate the appropriate values to create your desired configuration.

```yaml
#Configuration File Version (currently v1 and v2 supported)
version: v2

#Global settings to apply to the agent
agent_settings:
  tag: docker_onboarding
  log:
    level: info
  anomaly_capture_size: 1000
  anomaly_confidence_period: 30m

#Inputs define which datasets to monitor (files, containers, syslog ports, windows events, etc.)
inputs:
  container_stats:
    labels: "container_stats"
  containers:
    - labels: "docker_logs,all_containers"
      include:
        - "image=.*"
#  files:
#    - labels: "system_logs, auth"
#      path: "/var/log/auth.log"
#  ports:
#    - labels: "syslog_ports"
#      protocol: tcp
#      port: 1514

#Outputs define destinations to send both streaming data, and trigger data (alerts/automation/ticketing)
outputs:
  #Streams define destinations to send "streaming data" such as statistics, anomaly captures, etc. (Splunk, Sumo Logic, New Relic, Datadog, InfluxDB, etc.)
  streams:
    ##Sumo Logic Example
    #- name: sumo-logic-integration
    #  type: sumologic
    #  endpoint: "<ADD SUMO LOGIC HTTPS ENDPOINT>"

    #Splunk Example
    #- name: splunk-integration
    #  type: splunk
    #  endpoint: "<ADD SPLUNK HEC ENDPOINT>"
    #  token: "<ADD SPLUNK TOKEN>"

    ##Datadog Example
    #- name: datadog-integration
    #  type: datadog
    #  api_key: "<ADD DATADOG API KEY>"

    ##New Relic Example
    #- name: new-relic-integration
    #   type: newrelic
    #   endpoint: "<ADD NEW RELIC API KEY>"

    ##Influxdb Example
    #- name: influxdb-integration
    #  type: influxdb
    #  endpoint: "<ADD INFLUXDB ENDPOINT>"
    #  port: <ADD PORT>
    #  features: all
    #  tls:
    #    disable_verify: true
    #  token: "<ADD JWT TOKEN>"
    #  db: "<ADD INFLUX DATABASE>"

  ##Triggers define destinations for alerts/automation (Slack, PagerDuty, ServiceNow, etc)
  triggers:
    ##Slack Example
    #- name: slack-integration
    #  type: slack
    #  endpoint: "<ADD SLACK WEBHOOK/APP ENDPOINT>"


#Processors define analytics and statistics to apply to specific datasets
processors:
  cluster:
    name: clustering
    num_of_clusters: 50          # keep track of only top 50 and bottom 50 clusters
    samples_per_cluster: 2       # keep last 2 messages of each cluster
    reporting_frequency: 30s     # report cluster samples every 30 seconds

#Regexes define specific keywords and patterns for matching, aggregation, statistics, etc.
  regexes:
    - name: "error_level"
      pattern: "ERROR|error|Error|Err|ERR"
      trigger_thresholds:
        anomaly_probability_percentage: 95

    - name: "exception_check"
      pattern: "Exception|exception|EXCEPTION"
      trigger_thresholds:
        anomaly_probability_percentage: 95

    - name: "fail_level"
      pattern: "FAIL|Fail|fail"
      trigger_thresholds:
        anomaly_probability_percentage: 95

    - name: "info_level"
      pattern: "INFO|info|Info"

    - name: "warn_level"
      pattern: "WARN|warn|Warn"

    - name: "debug_level"
      pattern: "DEBUG|debug|Debug"

    - name: "success_check"
      pattern: "Success|SUCCESS|success|Succeeded|succeeded|SUCCEEDED"

#Workflows define the mapping between input sources, which processors to apply, and which destinations to send the streams/triggers to
workflows:
  stats_workflow:
    input_labels:
      - container_stats

  example_workflow:
    input_labels:
      - docker_logs
    processors:
      - clustering
      - error_level
      - info_level
      - warn_level
      - debug_level
      - fail_level
      - exception_check
      - success_check
    destinations:
      #- streaming_destination_a    #Replace with configured streaming destination
      #- streaming_destination_b    #Replace with configured streaming destination
      #- trigger_destination_a      #Replace with configured trigger destination
      #- trigger_destination_b      #Replace with configured trigger destination
```