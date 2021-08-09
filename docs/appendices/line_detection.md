# MultiLine Detection

There are lots of different log formats that applications use, therefore, one of the most challenging thing is to properly grouping multiline logs as a single log message when collecting, parsing and ingesting.

One primary example of multiline log messages is stack traces in languages. These logs can provide a wealth of information that is invaluable for troubleshooting and getting insight into application problems.

## Example of Multiline

Let’s use a sample java stack trace:

```java
Feb 09, 2021 3:23:29 PM com.google.devtools.search.cloud.feeder.MakeLog: RuntimeException: Run from this message!
    at com.my.app.Object.do$a1(MakeLog.java:50)
    at java.lang.Thing.call(Thing.java:10)
    at com.my.app.Object.help(MakeLog.java:40)
    at sun.javax.API.method(API.java:100)
    at com.jetty.Framework.main(MakeLog.java:30)
```

If no multiline processing applied, this sample stack trace would produce the following log lines while collecting and to be used for further processing \(parsing and ingesting\):

```text
log: {
Feb 09, 2021 3:23:29 PM com.google.devtools.search.cloud.feeder.MakeLog: RuntimeException: Run from this message!
}
log: {
    at com.my.app.Object.do$a1(MakeLog.java:50)
}
log: {
    at java.lang.Thing.call(Thing.java:10)
}
log: {
    at com.my.app.Object.help(MakeLog.java:40)
}
log: {
    at sun.javax.API.method(API.java:100)
}
log: {
    at com.jetty.Framework.main(MakeLog.java:30)
}
```

## Turning on Multiline processing

We use 2 methods to turn these multiple logs into a single log:

  - [Specifying Line Pattern](#specifying-line-pattern)
  - [Auto Line Detection](#auto-line-detection)

## Specifying Line Pattern

If "line\_pattern" regex rule is specified in the agent config, then agent process lines not for New Line\("\n"\) but for this specific line separation rule. Accumulates the logs between these line patterns as multiline logs and further processing is made based on this accumulated multiline logs.

```go
...

  files:
    - path: "/var/log/service_a.log"
      labels: "app,service_a"
      line_pattern: "^MMM dd, yyyy hh:mm:ss"

...
```

## Auto Line Detection

Detects line patterns automatically based on the Ragel FSM Based Lexical Recognition process. No need to specify line\_pattern explicitly. Accumulates the logs between these patterns as multiline logs and further processing is made based on this accumulated multiline logs.

```go
...

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

...
```

After turning on multiline processing, above sample stack trace produces the following log lines while collecting and to be used for further processing \(parsing and ingesting\):

```text
log: {Feb 09, 2021 3:23:29 PM com.google.devtools.search.cloud.feeder.MakeLog: RuntimeException: Run from this message!
    at com.my.app.Object.do$a1(MakeLog.java:50)
    at java.lang.Thing.call(Thing.java:10)
    at com.my.app.Object.help(MakeLog.java:40)
    at sun.javax.API.method(API.java:100)
    at com.jetty.Framework.main(MakeLog.java:30)
}
```

