---
description: >-
  This document outlines the various Filters types supported by the Edge Delta
  agent, and how the filters are configured for inputs.
---

# Filters

## Overview

Filters can be used to filter and transform the received logs before further processing. They are useful to discard unnecessary logs, protect sensitive data \(e.g. mask ssn\), transform content. Adding filters at the right place help reducing the agent resource usage due to less data being fed into processors.

Filters are defined at the top level in config yaml.

```yaml
filters:
  - name: error
    type: regex
    pattern: "error"
```

Once defined, filters can be referred at different places in the config yaml.

* Input filters apply right after the data ingestion from the input and before running workflows associated with the input.
* Workflow filters apply before running processors within the workflow.
* Processor filters apply before the processor runs regardless of which workflow the processor is running within.

### Regex Filters

Regex filters pass all log lines that match the specified regular expression for further processing and discard all unmatched logs.

| Key | Description | Required |
| :--- | :--- | :--- |
| name | User defined name of this specific filter, used for referring this filter in inputs/processors/workflows | Yes |
| type | Default filter type is 'regex' | No |
| pattern | Regular expression pattern to define which strings to match on | Yes |
| negate | Can be set to 'true' to reverse the effect of filter | No |

Below example will grab the log lines that are error related and discard other lines.

```yaml
  - name: error
    type: regex
    pattern: "error|ERROR|ERR|Err"
```

Negative filters are also possibla via _negate_ option. Below example will discard DEBUG logs and only pass thru other logs:

```yaml
  - name: not_debug
    pattern: "DEBUG"
    negate: true
```

### Mask Filters

Mask filters transform the log lines to hide the matched part of the content for the given regex pattern. If is useful to mask sensitive data such as phone numbers, social securit numbers, credit card numbers etc.

| Key | Description | Required |
| :--- | :--- | :--- |
| name | User defined name of this specific filter, used for referring this filter in inputs/processors/workflows | Yes |
| type | Type should be set to "mask" to define a mask filter | Yes |
| pattern | Regular expression pattern to define which strings to match on. Either _pattern_ or _predefined\_pattern_ must be set. | No |
| predefined\_pattern | There are some commonly used patterns predefined which can be used instead of custom pattern. Available predefined patterns are _credit\_card_ and _us\_phone\_dash_ | No |
| mask | String to be used as replacement for the matched part of the log. Default mask is "\*\*\*\*\*\*". Specifying empty mask "" will simply remove matched the pattern from the log line | No |

Below example filter replaces "password: SOME\_PASSWORD" with "password: \*\*\*\*\*\*":

```yaml
  - name: mask_password
    type: mask
    pattern: "password:\s*\w+"
    mask: "password: ******"
```

Instead of defining a regex pattern, it is also possible to use one of the predefined patterns.

Below is an example filter that masks US phone numbers:

```yaml
  - name: mask_phone
    type: mask
    predefined_pattern: us_phone_dash
```

### JSON Field Extractor

JSON Field Extractor extracts a field's value and replaces the whole JSON content with the field's value. Original JSON is discarded once this filter is applied. It's recommended to attach this filter to the processor if the original json needs to be fed into other workflows/processors.

| Key | Description | Required |
| :--- | :--- | :--- |
| name | User defined name of this specific filter, used for referring this filter in inputs/processors/workflows | Yes |
| type | Type should be set to "extract-json-field" | Yes |
| field_path | Field path is a dot separated path of the field (i.e. "log.message"). Its value will be extracted and the original json content will be discarded | Yes |

The example below extracts the message field. If the field was nested then we would set field_path to its path.

```yaml
  - name: extract_message
    type: extract-json-field
    field_path: "message"
```


Example log:
**`{"timestamp":1623793757, "level": "info", "message": "hello world"}`**

After extractor filter is applied:
**`hello world`**

### Buffered Trace Filter

Buffered Trace Filter is a special purpose filter for trace logs. By "trace log" we mean a set of logs that can be tied together with an id such as trace id or request id. Buffered trace filter groups the logs by specified id, waits for a specified duration to make sure all relavant events of that trace/request is collected and then makes a decision on either discard the trace logs or pass them based on configuration.

Options are

* pass through failed operation events
* pass through high latency operation events
* pass through certain percentage of successful events

| Key | Description | Required |
| :--- | :--- | :--- |
| name | User defined name of this specific filter, used for referring this filter in inputs/processors/workflows | Yes |
| trace\_id\_pattern | Regular expression pattern which is used to extract the trace id values ffrom logs. Must be a regex with single capture group | Yes |
| failure\_pattern | Regular expression pattern whose match indicates that the trace event \(group of logs sharing same id\) is a failure. Failures are passed thru from this filter. | Yes |
| trace\_deadline | A [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents the max duration of a trace. Once the specified trace deadline reached the buffered trace filter takes all events belonging to the same trace, applies filters/sampling \(based on configuration\), and then if passed the events are passed thru | Yes |
| success\_sample\_rate | The sample rate \[0,1\] of successfull traces. Default is zero which means successfull traces are discarded. If it's set to 0.2 then 20% of successfull traces will pass thru this filter. _Note:_ Any trace event without a failure\_pattern match indicates successful trace. | No |
| latency\_pattern | Regular expression pattern which is used to extract the latency value \(if applicable\) from the trace logs. Must be a regex with single numeric capture group. Only one of the logs belonging to the same trace \(sharing same id\) should have such latency information or the last one will be picked to represent the latency of the trace. Once the latency value is extracted and converted to a number it can be used in conjunction with _latency\_threshold_ to pass thru high latency traces. This is useful to collect the high latency traces in addition to the failed ones which are already pass thru as described in _failure\_pattern_. | No |
| latency\_threshold | A numeric value representing threshold for high latency limit. Latency of a trace is extracted using latency\_pattern | No |

```yaml
filters:
- name: trace_filter
  type: buffered-trace
  trace_id_pattern: \"trace_id\":\"(?P<id>\w+)"
  failure_pattern: \"status\":\"Failed\"
  trace_deadline: 1m
  success_sample_rate: 0.0
  latency_pattern: \"duration\":\"(?P<latency>\d+)\"
  latency_threshold: 8000
```

## Usage

Filter names are used to apply filters to inputs, workflows and processors. When multiple filters are provided they are applied in given order, from top to bottom.

* Below is an example file input with _error_ and _mask\_card_ filters:

  ```yaml
    inputs:
      files:
        - labels: "billing"
          path: "/var/log/billing/*.log"
          filters:
            - error
            - mask_card
  ```

  See [Inputs](https://docs.edgedelta.com/configuration/inputs) documentation for details about inputs that can be filtered.

* Below is an example workflow with _error_ filter:

  ```yaml
  workflows:
    application_workflow:
      input_labels:
        - system_stats
        - agent_stats
        - application_logs
      filters:
        - error
      processors:
        - error-check
        - fail-check
        - success-check
      destinations:
        - sumo-logic-devops-integration
        - slack-devops-integration
  ```

  See [Workflows](https://docs.edgedelta.com/configuration/workflows) documentation for details about workflows that can be filtered.

* Below is an example Dimension Counter Processor with _not\_debug_ filter.

  ```yaml
  regexes:
    - name: "log"
      pattern: "level=(?P<level>\\w+) "
      dimensions: ["level"]
      trigger_thresholds:
        anomaly_probability_percentage: 90
      filters:
        - not_debug
  ```

See [Processors](https://docs.edgedelta.com/configuration/processors) documentation for details about processors that can be filtered.

