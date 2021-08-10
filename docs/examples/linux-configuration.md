---
description: >-
  The following document contains an example configuration for deployment on
  Linux-based Operating Systems
---

# Linux Configuration

The example configuration is a default, working configuration requiring no changes to be deployed. 

Examples of initial changes to customize to your environment would be adding specific [Inputs](../configuration/inputs.md), [Processors](../configuration/processors.md), or [Outputs](../configuration/outputs.md) to various [Streaming](../configuration/outputs.md#streaming-destinations) and [Trigger](../configuration/outputs.md#trigger-destinations) destinations. 

Please comment/uncomment parameters as needed, as well as populate the appropriate values to create your desired configuration.

```go
#This is a sample edgedelta agent config. See https://docs.edgedelta.com/configuration for more details.
version: v2

#Global settings to apply to the agent
agent_settings:
  tag: linux_onboarding
  log:    
    level: info
  anomaly_capture_size: 1000
  anomaly_confidence_period: 30m

#Inputs define which datasets to monitor (files, containers, syslog ports, windows events, etc.)
inputs:
  system_stats:
    labels: "system_stats"
  files:
    - labels: "system_logs, auth"
      path: "/var/log/auth.log"
    - labels: "system_logs, syslog"
      path: "/var/log/syslog"
    - labels: "system_logs, secure"
      path: "/var/log/secure"
    - labels: "system_logs, messages"
      path: "/var/log/messages"
  #ports:
  # - labels: "syslog_ports"
  #   protocol: tcp
  #   port: 1514

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
    - name: "auth_failed"
      pattern: "\\b(?:[Aa]uthentication failure|FAILED SU|input_userauth_request: invalid user|Invalid user|Failed publickey|Failed password)\\b"
      trigger_thresholds:
        anomaly_probability_percentage: 95

    - name: "auth_success"
      pattern: "\\b(?:su:|sudo:|sshd:|sshd\\[|pam_unix).*(?:\\b[Aa]ccepted|session opened|to\\b.*\\bon)\\b"
      trigger_thresholds:
        anomaly_probability_percentage: 95

    - name: "auth_root"
      pattern: "\\b(?:sudo|root|su)\\b"
      trigger_thresholds:
        anomaly_probability_percentage: 95

    - name: "auth_su_attempt"
      pattern: "\\b(?:su:|su\\[).*(?:[Aa]uthentication failure|FAILED SU|input_userauth_request: invalid user|Invalid user|Failed publickey|Failed password)\\b"
      trigger_thresholds:
        anomaly_probability_percentage: 95

    - name: "system_start"
      pattern: "\\bInitializing cgroup subsys cpuset\\b"
      trigger_thresholds:
        anomaly_probability_percentage: 95

    - name: "system_stop"
      pattern: "\\b(?:exiting|exited|terminating|terminated|shutting)\\b"
      trigger_thresholds:
        anomaly_probability_percentage: 95

    - name: "error-check"
      pattern: "error|ERROR|problem|ERR|Err"
      trigger_thresholds:
        anomaly_probability_percentage: 95

#Workflows define the mapping between input sources, which processors to apply, and which destinations to send the streams/triggers to
workflows:
  system_stats_workflow:
    input_labels:
      - system_stats

  example_workflow:
    input_labels:
      - system_logs
    processors:
      - clustering
      - auth_failed
      - auth_success
      - auth_root
      - auth_su_attempt
      - system_start
      - system_stop
      - error-check
    destinations:
      #- streaming_destination_a    #Replace with configured streaming destination
      #- streaming_destination_b    #Replace with configured streaming destination
      #- trigger_destination_a      #Replace with configured trigger destination
      #- trigger_destination_b      #Replace with configured trigger destination
```

