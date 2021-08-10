---
description: >-
  The following document contains an example configuration for deployment on
  Windows-based Operating Systems
---

# Windows Configuration

The example configuration is a default, working configuration requiring no changes to be deployed. 

Examples of initial changes to customize to your environment would be adding specific [Inputs](../configuration/inputs.md), [Processors](../configuration/processors.md), or [Outputs](../configuration/outputs.md) to various [Streaming](../configuration/outputs.md#streaming-destinations) and [Trigger](../configuration/outputs.md#trigger-destinations) destinations. 

Please comment/uncomment parameters as needed, as well as populate the appropriate values to create your desired configuration.

```go
#Configuration File Version (currently v1 and v2 supported)
version: v2

#Global settings to apply to the agent
agent_settings:         
  tag: windows_onboarding
  log:    
    level: info
  anomaly_capture_size: 1000
  anomaly_confidence_period: 30m


#Inputs define which datasets to monitor (files, containers, syslog ports, windows events, etc.)
inputs:
  system_stats:
    labels: "system_stats"
  winevents:
    - labels: "windows_events, application"
      channel: "Application"
    - labels: "windows_events, security"
      channel: "Security"
    - labels: "windows_events, system"
      channel: "System"
#  files:
#    - labels: "iis_logs"
#      path: "C:\\inetpub\\Logs\\LogFiles\\W3SVC1\\*.log"
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
    # Error level Windows logs
    - name: "win_err"
      pattern: "{\"level\":\"error\"}"
      trigger_thresholds:
        anomaly_probability_percentage: 95
    # Critical Windows logs
    - name: "win_crit"
      pattern: "{\"level\":\"critical\"}"
      trigger_thresholds:
        anomaly_probability_percentage: 95
    # Windows Update Failure
    - name: "win_update_err"
      pattern: "{\"eventId\":20}"
      trigger_thresholds:
        anomaly_probability_percentage: 95
    # Windows Logon Failure
    # Enable via GPO or locally with windows CMD.exe: Auditpol /set /category:"Logon/Logoff" /Success:enable /failure:enable
    - name: "win_logon_fail"
      pattern: "{\"eventId\":4625}"
      trigger_thresholds:
        anomaly_probability_percentage: 95
    # Windows PowerShell
    - name: "win_ps_warn"
      pattern: "{\"level\":\"warning\"}"
      trigger_thresholds:
        anomaly_probability_percentage: 95

#Workflows define the mapping between input sources, which processors to apply, and which destinations to send the streams/triggers to
workflows:

  system_stats_workflow:
    input_labels:
      - system_stats

  example_workflow:
    input_labels:
      - windows_events
    processors:
      - clustering
      - win_err
      - win_crit
      - win_update_err
      - win_logon_fail
      - win_ps_warn
    destinations:
      #- streaming_destination_a    #Replace with configured streaming destination
      #- streaming_destination_b    #Replace with configured streaming destination
      #- trigger_destination_a      #Replace with configured trigger destination
      #- trigger_destination_b      #Replace with configured trigger destination
```

