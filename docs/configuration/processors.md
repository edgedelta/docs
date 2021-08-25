---
description: >-
  This document outlines the various Processor types supported by the Edge Delta
  agent, and how the processors are configured. Processor declarations go under
  `processors` section in config yaml and in
---

# Processors

## Overview

Processors are the mechanism used to allow users to specify various analytical, statistical, and machine-learning based algorithms to apply to their incoming data.

## Cluster

The cluster processor finds patterns in the logs and clusters them based on similarity. Cluster patterns and samples of each cluster are reported to target streaming destination.

_Note_: Analyzing patterns in high log volume environments can be compute intensive. To be super resource friendly, by default cluster processor applies sampling to the incoming logs. It will only process 200 logs per-source \(e.g. a container or file\) unless one of the rate limiting related fields are set. See _cpu\_friendly_ and _throttle\_limit\_per\_sec_ below for details.

| Key | Description | Required |
| :--- | :--- | :--- |
| name | User defined name of this specific processor, used for mapping this processor to a workflow. | Yes |
| reporting\_frequency | The frequency to report clustering results \(patterns + samples\) to streaming destinations | Yes |
| num\_of\_clusters | Clustering reports top N and bottom N clusters. N = num\_of\_clusters | Yes |
| samples\_per\_cluster | The number of sample events to report | Yes |
| retention | A [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents a cluster's retention The clusters that don't have any new logs for last retention period will be dropped and will no longer be reported until seen again. | Yes |
| cpu\_friendly | When set to 'true' the CPU aware rate limiting is enabled. This makes the agent honor the given CPU limit \(specified at top level agent\_settings section\) by dropping some percentage of events in order to keep agent's CPU usage below the given limit. Unless you have more than 1k logs per second don't worry about this setting. | Yes |
| throttle\_limit\_per\_sec | Puts a hard limit on how many logs should be clustered per second from a single source. If cpu\_friendly is enabled then this will be ignored. | Yes |
| filters | List of filter names to be applied before running this processor. See [Filters](./filters.md) documentation for details about filters. | No |

**Example config:**

```yaml
  cluster:
    name: clustering
    num_of_clusters: 100
    samples_per_cluster: 20
    reporting_frequency: 1m
    retention: 30m
```

## Simple Keyword Match Processor

The simple keyword match processor checks for basic regex match in the logs, counts the matching logs and generates anomaly scores. The counts are rolled up by the agent with given _interval_ and emitted as metrics. Thresholds can be set to get notified when anomalies occur \(e.g. metric value spikes or drops unexpectedly\).

| Key | Description | Required |
| :--- | :--- | :--- |
| name | User defined name of this specific processor, used for mapping this processor to a workflow. | Yes |
| pattern | Regular expression pattern to define which strings to match on. _Note:_ It must follow Golang regex protocol, e.g. "error\|ERROR" | Yes |
| interval | A [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents reporting/rollup interval for the generated statistics. Default value is 1m. | No |
| retention | A [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents how far back the agent should look when generating anomaly scores. Default value is 3h. | No |
| **trigger\_thresholds** | The trigger\_thresholds section has sub-fields that define thresholds based on calculated metrics. When a threshold hits the agent notifies the trigger destinations that are specified in the same workflow. | No |
|   anomaly\_probability\_percentage | The percent confidence level \(0 - 100\) that needs to be reached in order to generate a trigger. No default value. | No |
|   upper\_limit\_per\_interval | Static threshold for generating a trigger. If the number of events that match the given pattern for the most recent reporting interval is greater than the limit, a trigger will be generated. No default value. | No |
|   lower\_limit\_per\_interval | Static threshold for generating a trigger. If the number of events that match the given pattern for the most recent reporting interval is less than the limit, a trigger will be generated. No default value. | No |
|   consecutive | Consecutive indicates how many times in a row a threshold must be exceeded before actually generating a trigger. Useful for static thresholds because anomaly scores are usually low in the next interval after seeing a sudden spike due to widened baselines. Default is 0. | No |
| filters | List of filter names to be applied before running this processor. See [Filters](./filters.md) documentation for details about filters. | No |

**Example config:**

```yaml
regexes:
  - name: "error"
    pattern: "error|err|ERROR|ERR"
    trigger_thresholds:
      anomaly_probability_percentage: 90
```

Metrics generated from example config:

* _error.count_: Total count of matches within an interval
* _error.anomaly1_: Anomaly score of current interval based on total count history. represents the how anomalous the current error count is compared to its history. Score is in the range of \[0,100\].

## Numeric Capture Processor

Numeric capture processor supports exact same configuration as **Simple Keyword Match** processor except that the regular expression has a numeric capture group. Typically **Numeric Capture** processors are used for numerical statistics, such as response duration, payload size etc.

| Key | Description | Required |
| :--- | :--- | :--- |
| name | User defined name of this specific processor, used for mapping this processor to a workflow. | Yes |
| pattern | Regular expression pattern which has a named group. The matching part of the log will be extracted and converted to a floating point number. _Note:_ named capture groups must follow Golang regex protocol, e.g. "\(?P&lt;latency&gt;\\\d+\)ms" | Yes |
| interval | A [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents reporting/rollup interval for the generated statistics. Default value is 1m. | No |
| retention | A [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents how far back the agent should look when generating anomaly scores. Default value is 3h. | No |
| **trigger\_thresholds** | The trigger\_thresholds section has sub-fields that define thresholds based on calculated metrics. When a threshold hits the agent notifies the trigger destinations that are specified in the same workflow. | No |
|   anomaly\_probability\_percentage | The percent confidence level \(0 - 100\) that needs to be reached in order to generate a trigger. No default value. | No |
|   upper\_limit\_per\_interval | Static threshold for generating a trigger. If the number of events that match the given pattern for the most recent reporting interval is greater than the limit, a trigger will be generated. No default value. | No |
|   lower\_limit\_per\_interval | Static threshold for generating a trigger. If the number of events that match the given pattern for the most recent reporting interval is less than the limit, a trigger will be generated. No default value. | No |
|   consecutive | Consecutive indicates how many times in a row a threshold must be exceeded before actually generating a trigger. Useful for static thresholds because anomaly scores are usually low in the next interval after seeing a sudden spike due to widened baselines. Default is 0. | No |
| filters | List of filter names to be applied before running this processor. See [Filters](./filters.md) documentation for details about filters. | No |

**Example config:**

```yaml
regexes:
  - name: "response_time"
    pattern: "completed in (?P<latency>\\d+)ms"
    trigger_thresholds:
      anomaly_probability_percentage: 90
```

When such regular expression pattern is provided the following statistics are generated and emitted as metrics

* _response\_time\_latency.count_: total count of matches in current interval
* _response\_time\_latency.avg_: average of captured numeric values. e.g. average response time in above example.
* _response\_time\_latency.min_: minimum of captured numeric values.
* _response\_time\_latency.max_: maximum of captured numeric values.
* _response\_time\_latency.anomaly1_: anomaly score based on history of average values.

## Dimension Counter Processor

The dimension counter processor extracts a specified part of the log, considers it as the dimension \(the key\) and counts matching logs for each distinct dimension value. It generates count and anomaly metrics for each unique dimension value.

Dimension counter supports exact same configuration as **Simple Keyword Match** processor with addition of below fields:

| Key | Description | Required |
| :--- | :--- | :--- |
| name | User defined name of this specific processor, used for mapping this processor to a workflow. | Yes |
| pattern | Regular expression pattern containing a named capture group representing the dimension. _Note:_ named capture groups must follow Golang regex protocol, e.g.  "level=\(?P&lt;level&gt;\\\w+\)" | Yes |
| dimensions | List of named capture group fields to use as dynamic dimensions \(group by\). Each dimension specified here must have a corresponding named capture group in the pattern field for this processor. | Yes |
| interval | A [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents reporting/rollup interval for the generated statistics. Default value is 1m. | No |
| retention | A [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents how far back the agent should look when generating anomaly scores. Default value is 3h. | No |
| **trigger\_thresholds** | The trigger\_thresholds section has sub-fields that define thresholds based on calculated metrics. When a threshold hits the agent notifies the trigger destinations that are specified in the same workflow. | No |
|   anomaly\_probability\_percentage | The percent confidence level \(0 - 100\) that needs to be reached in order to generate a trigger. No default value. | No |
|   upper\_limit\_per\_interval | Static threshold for generating a trigger. If the number of events that match the given pattern for the most recent reporting interval is greater than the limit, a trigger will be generated. No default value. | No |
|   lower\_limit\_per\_interval | Static threshold for generating a trigger. If the number of events that match the given pattern for the most recent reporting interval is less than the limit, a trigger will be generated. No default value. | No |
|   consecutive | Consecutive indicates how many times in a row a threshold must be exceeded before actually generating a trigger. Useful for static thresholds because anomaly scores are usually low in the next interval after seeing a sudden spike due to widened baselines. Default is 0. | No |
| filters | List of filter names to be applied before running this processor. See [Filters](./filters.md) documentation for details about filters. | No |

**Example config:** Count per log level

```yaml
regexes:
  - name: "log"
    pattern: "level=(?P<level>\\w+) "
    dimensions: ["level"]
    trigger_thresholds:
      anomaly_probability_percentage: 90
```

Let's assume our logs have 3 levels: DEBUG, INFO, ERROR. In this case the following metrics will be generated by the Dimension Counter processor:

* _log\_level\_debug.count_
* _log\_level\_debug.anomaly1_
* _log\_level\_info.count_
* _log\_level\_info.anomaly1_
* _log\_level\_error.count_
* _log\_level\_error.anomaly1_

Format: _{processor name}\_{dimension name}\_{dimension value}.{stat type}_

## Dimension Numeric Capture Processor

The dimension numeric capture processor keeps track of a specific numerical field \(i.e. latency\) per unique dimension value \(i.e. api\_path\), and automatically generate statistics \(counts, averages\) and detect anomalies based on the aggregate values grouped by dimensions.

It supports same configurations as **Dimension Counter Processor** with the difference being regex pattern contains a numeric capture group.

| Key | Description | Required |
| :--- | :--- | :--- |
| name | User defined name of this specific processor, used for mapping this processor to a workflow. | Yes |
| pattern | Regular expression pattern containing one named capture group representing dimension and one or more numeric named captured groups. _Note:_ named capture groups must follow Golang regex protocol, e.g. "\(?P&lt;method&gt;\\\w+\) took \(?P&lt;latency&gt;\\\d+\) ms"\ | Yes |
| dimensions | List of named capture group fields to use as dynamic dimensions \(group by\). Each dimension specified here must have a corresponding named capture group in the pattern field for this processor. | Yes |
| interval | A [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents reporting/rollup interval for the generated statistics. Default value is 1m. | No |
| retention | A [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents how far back the agent should look when generating anomaly scores. Default value is 3h. | No |
| **trigger\_thresholds** | The trigger\_thresholds section has sub-fields that define thresholds based on calculated metrics. When a threshold hits the agent notifies the trigger destinations that are specified in the same workflow. | No |
|   anomaly\_probability\_percentage | The percent confidence level \(0 - 100\) that needs to be reached in order to generate a trigger. No default value. | No |
|   upper\_limit\_per\_interval | Static threshold for generating a trigger. If the number of events that match the given pattern for the most recent reporting interval is greater than the limit, a trigger will be generated. No default value. | No |
|   lower\_limit\_per\_interval | Static threshold for generating a trigger. If the number of events that match the given pattern for the most recent reporting interval is less than the limit, a trigger will be generated. No default value. | No |
|   consecutive | Consecutive indicates how many times in a row a threshold must be exceeded before actually generating a trigger. Useful for static thresholds because anomaly scores are usually low in the next interval after seeing a sudden spike due to widened baselines. Default is 0. | No |
| filters | List of filter names to be applied before running this processor. See [Filters](./filters.md) documentation for details about filters. | No |

**Example config:**

```yaml
regexes:
  - name: "http"
    pattern: "(?P<method>\\w+) took (?P<latency>\\d+) ms"
    dimensions: ["method"]
    trigger_thresholds:
      anomaly_probability_percentage: 90
```

Let's say we have following logs feeding into the **Dimension Numeric Capture Processor** that is configured above:

* "GetAlbums took 12ms"
* "GetRecords took 16ms"

When agent sees these logs, it will start generating the metrics below:

* _http\_method\_getalbums\_latency.count_
* _http\_method\_getalbums\_latency.avg_
* _http\_method\_getalbums\_latency.min_
* _http\_method\_getalbums\_latency.max_
* _http\_method\_getalbums\_latency.anomaly1_
* _http\_method\_getrecords\_latency.count_
* _http\_method\_getrecords\_latency.avg_
* _http\_method\_getrecords\_latency.min_
* _http\_method\_getrecords\_latency.max_
* _http\_method\_getrecords\_latency.anomaly1_

Format: _{processor name}\_{dimension name}\_{dimension value}\_{numeric capture group name}.{stat type}_

For each distinct dimension \(_getalbums_ and _getrecords_\) numeric statistics are calculated and reported with a metric name containing the dimension in it. **Dimension Numeric Capture Processor** processor basically does what **Numeric Capture Processor** does for each distinct dimension value.

## Trace Processor

The trace processor is useful for tracking events that has a unique ID and a distinguishable start and end logs. IDs typically dynamic fields such as transaction IDs, trace ID etc. Each event's duration is tracked and average/min/max in current interval are emitted as metrics. Anomalies are detected based on the average event duration based on the history of average durations.

Trace processor configuration looks similar to **Simple Keyword Match Processor** except that instead of _pattern_ field the trace processor expects _start\_pattern_ and _finish\_pattern_. In addition to the other trigger thresholds, the trace processor supports _max\_duration_ which is useful to get notified of long running events.

| Key | Description | Required |
| :--- | :--- | :--- |
| name | User defined name of this specific processor, used for mapping this processor to a workflow. | Yes |
| start\_pattern | Regular expression match pattern to define which strings to match a success event | Yes |
| finish\_pattern | Regular expression match pattern to define which strings to match a failure event | Yes |
| interval | A [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents reporting/rollup interval for the generated statistics. Default value is 1m. | No |
| retention | A [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents how far back the agent should look when generating anomaly scores. Default value is 3h. | No |
| **trigger\_thresholds** | The trigger\_thresholds section has sub-fields that define thresholds based on calculated metrics. When a threshold hits the agent notifies the trigger destinations that are specified in the same workflow. | No |
|   max\_duration | Maximum duration of an event allowed. If an event doesn't complete within this duration then a trigger is generated | No |
|   anomaly\_probability\_percentage | The percent confidence level \(0 - 100\) that needs to be reached in order to generate a trigger. No default value. | No |
|   upper\_limit\_per\_interval | Static threshold for generating a trigger. If the number of events that match the given pattern for the most recent reporting interval is greater than the limit, a trigger will be generated. No default value. | No |
|   lower\_limit\_per\_interval | Static threshold for generating a trigger. If the number of events that match the given pattern for the most recent reporting interval is less than the limit, a trigger will be generated. No default value. | No |
|   consecutive | Consecutive indicates how many times in a row a threshold must be exceeded before actually generating a trigger. Useful for static thresholds because anomaly scores are usually low in the next interval after seeing a sudden spike due to widened baselines. Default is 0. | No |
| filters | List of filter names to be applied before running this processor. See [Filters](./filters.md) documentation for details about filters. | No |

**Example config:**

```yaml
traces:
  - name: render-trace
    start_pattern: "rendering job: (?P<ID>[0-9a-fA-F]{8}) started"
    finish_pattern: "rendering job: (?P<ID>[0-9a-fA-F]{8}) finished"
    trigger_thresholds:
      max_duration: 50000 # 50 seconds
```

## Top-K processor

Top-K processor keeps track of top K records \(e.g. k=10\) where the records are identified with one or more named regex group values combined together. It reports top k items as a string value.

| Key | Description | Required |
| :--- | :--- | :--- |
| name | User defined name of this specific processor, used for mapping this processor to a workflow. | Yes |
| pattern | Logs matching this pattern will be selected and named groups combined together will be the key of the record for which we keep counter. | Yes |
| k | Integer value that specifies how many top records to keep track of at every interval. Records are ordered by their count descendingly and top k items are picked for reporting. | Yes |
| interval | A [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents reporting interval. At every interval the top records will be reported and they will be removed locally. | Yes |
| lower\_limit | If a lower limit is provided only records whose count is greater than the limit will be able to make it to top k. | No |
| separator | separator is used to combine the named group values together to form a record key. Default is comma ',' | No |
| filters | List of filter names to be applied before running this processor. See [Filters](./filters.md) documentation for details about filters. | No |

**Example config:**

```yaml
  top_ks:
    - name: top-api-requests
      pattern: (?P<ip>\d+\.\d+\.\d+\.\d+) - \w+ \[.*\] "(?P<method>\w+) (?P<path>.+) HTTP\/\d.0" (?P<code>.+) \d+
      k: 10
      interval: 30s
```

**Example log:**

_"12.195.88.88 - joe \[08/Aug/2020:05:57:49 +0000\] "GET /optimize/engage HTTP/1.0" 200 19092"_

The pattern above would generate a record key like this: _"12.195.88.88,GET,/optimize/engage,200"_

Let's say this record has been seen 5 times in last period and it was one of the top k items. Then below log will be generated by Top-K processor and sent to workflow's destinations:

_"12.195.88.88,GET,/optimize/engage,200=5"_

## Ratio

The ratio processor takes one success and one failure regex pattern and calculates success ratio. Ratio is calculated with following formula: `ratio = success / (success+failure)`. Ratio processor detects ratio anomalies and supports static thresholds as well.

Ratio processor configuration looks similar to **Simple Keyword Match** processor except that instead of pattern field it expects two regular expressions: _success\_pattern_ and _failure\_pattern_.

| Key | Description | Required |
| :--- | :--- | :--- |
| name | User defined name of this specific processor, used for mapping this processor to a workflow. | Yes |
| success\_pattern | Regular expression match pattern to define which strings to match a success event | Yes |
| failure\_pattern | Regular expression match pattern to define which strings to match a failure event | Yes |
| interval | A [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents reporting/rollup interval for the generated statistics. Default value is 1m. | No |
| retention | A [golang duration](https://golang.org/pkg/time/#ParseDuration) string that represents how far back the agent should look when generating anomaly scores. Default value is 3h. | No |
| **trigger\_thresholds** | The trigger\_thresholds section has sub-fields that define thresholds based on calculated metrics. When a threshold hits the agent notifies the trigger destinations that are specified in the same workflow. | No |
|   anomaly\_probability\_percentage | The percent confidence level \(0 - 100\) that needs to be reached in order to generate a trigger. No default value. | No |
|   upper\_limit\_per\_interval | Static threshold for generating a trigger. If the number of events that match the given pattern for the most recent reporting interval is greater than the limit, a trigger will be generated. No default value. | No |
|   lower\_limit\_per\_interval | Static threshold for generating a trigger. If the number of events that match the given pattern for the most recent reporting interval is less than the limit, a trigger will be generated. No default value. | No |
|   consecutive | Consecutive indicates how many times in a row a threshold must be exceeded before actually generating a trigger. Useful for static thresholds because anomaly scores are usually low in the next interval after seeing a sudden spike due to widened baselines. Default is 0. | No |
| filters | List of filter names to be applied before running this processor. See [Filters](./filters.md) documentation for details about filters. | No |

**Example config:**

```yaml
ratios:
  - name: error-ratio
    success_pattern: "request succeeded"
    failure_pattern: "request failed"
    trigger_thresholds:
      anomaly_probability_percentage: 90
```

## Complete example

```yaml
version: v2
agent_settings:
  tag: prod

inputs:
  kubernetes:
    - labels: "my-app"
      include:
        - "pod=^my-app.*$,kind=ReplicaSet"
outputs:
  streams:
    - name: my-datadog-trial
      type: datadog
      log_host: <ADD DATADOG LOG_HOST>
      metric_host: <ADD DATADOG METRIC_HOST>
      api_key: {{Env "TEST_DD_APIKEY"}} # define datadog api key as environment variable (recommended way for secrets)

processors:
  # Clustering processor
  cluster:
    name: clustering
    num_of_clusters: 100
    samples_per_cluster: 20
    reporting_frequency: 1m
    retention: 30m

  regexes:

    # Simple keyword match processor
    - name: "error"
      pattern: "error|err|ERROR|ERR"
      trigger_thresholds:
        anomaly_probability_percentage: 90

    # Numeric capture processor
    - name: "response_time"
      pattern: "completed in (\\d+)ms"
      trigger_thresholds:
        anomaly_probability_percentage: 90

    # Dimension counter processor
    - name: "log"
      pattern: "level=(?P<level>\\w+) "
      dimensions: ["level"]
      trigger_thresholds:
        anomaly_probability_percentage: 90

    # Dimension Numeric Capture Processor
    - name: "http"
      pattern: "(?P<method>\\w+) took (?P<latency>\\d+) ms"
      dimensions: ["method"]
      trigger_thresholds:
        anomaly_probability_percentage: 90

  traces:

    # Trace processor
    - name: render-trace
      start_pattern: "rendering job: (?P<ID>[0-9a-fA-F]{8}) started"
      finish_pattern: "rendering job: (?P<ID>[0-9a-fA-F]{8}) finished"
      trigger_thresholds:
        max_duration: 50000 # 50 seconds

  top_ks:
    - name: top-api-requests
      pattern: (?P<ip>\d+\.\d+\.\d+\.\d+) - \w+ \[.*\] "(?P<method>\w+) (?P<path>.+) HTTP\/\d.0" (?P<code>.+) \d+
      k: 10
      interval: 30s

  ratios:
    # Ratio processor
    - name: error-ratio
      success_pattern: "request succeeded"
      failure_pattern: "request failed"
      trigger_thresholds:
        anomaly_probability_percentage: 90

workflows:
    my-workflow:
      input_labels:
        - my-app
      processors:
        - clustering
        - error
        - response_time
        - log
        - http
        - render-trace
        - top-api-requests
        - error-ratio
      destinations:
        - my-datadog-trial
```