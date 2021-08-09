---
description: Benchmark results of Edge Delta agent.
---

# Benchmark Test Results

Vector Test Harness tool which was developed by Timberio Company was used to get benchmark results. This is an open source tool, and you can find it in [https://github.com/timberio/vector-test-harness](https://github.com/timberio/vector-test-harness) public code repository. Below list contains the test cases we run to benchmark Edge Delta agent.

Performance test cases:

* TCP to Blackhole: In this case test flow starts with messages from producers and terminates with the agent when they are ingested to itself.
* TCP to TCP: In this case agent ingests messages from TCP producers and pushes the processed messages to a TCP consumer.
* TCP to HTTP: In this case agent ingests messages from TCP producers and pushes the processed messages to a HTTP consumer.
* File to TCP: In this case agent ingests messages from file pushes the processed messages to a TCP consumer.
* Regex Parsing: In this case agent ingests messages from TCP and applies an apache common regex processor then pushes the processed messages to a TCP consumer.

Vector already did benchmark tests for the performance of different APM tools including Vector, Filebeat, FluentBit, FluentD, Logstash, Splunk UF and Splunk HF and published the results. We stick to the results obtained by Vector and extend the results table by adding Edge Delta agent results. Therefore, the table of the results took the shape below.

| Test | Edge Delta | Vector | Filebeat | FluentBit | FluentD | Logstash | SplunkUF | SplunkHF |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| TCP to Blackhole | 490.3 mib/s | 86mib/s | n/a | 64.4mib/s | 27.7mib/s | 40.6mib/s | n/a | n/a |
| File to TCP | 110.2 mib/s | 76.7mib/s | 7.8mib/s | 35mib/s | 26.1mib/s | 3.1mib/s | 40.1mib/s | 39mib/s |
| Regex Parsing | 17.1 mib/s | 13.2mib/s | n/a | 20.5mib/s | 2.6mib/s | 4.6mib/s | n/a | 7.8mib/s |
| TCP to HTTP | 211.2 mib/s | 26.7mib/s | n/a | 19.6mib/s | &lt;1mib/s | 2.7mib/s | n/a | n/a |
| TCP to TCP | 212.7 mib/s | 69.9mib/s | 5mib/s | 67.1mib/s | 3.9mib/s | 10mib/s | 70.4mib/s | 7.6mib/s |

The results except Edge Delta [https://github.com/timberio/vector\#performance](https://github.com/timberio/vector#performance)

