---
description: >-
  This document outlines elastic index configuration that works optimally with
  edgedelta agents.
---

# Elastic Index Setup

1. Create lifecycle policy

Index lifecycle policies manage indices rollover/retention requirements and other aspects of index lifecycle. See official documentation [here](https://www.elastic.co/guide/en/elasticsearch/reference/current/index-lifecycle-management.html) for further reading on the topic

Edgedelta provides a simple lifecycle policy which creates a new index every day and keeps last 15 days of data. You can run the below command as is in your Elastic's dev console. Alternatively the Index Lifecycle Policies can be used. Feel free to change retention or any other fields as desired.

```text
PUT _ilm/policy/ed-agent-log-policy
{
  "policy": {
    "phases": {
      "hot": {
        "min_age": "0ms",
        "actions": {
          "rollover": {
            "max_age": "1d",
            "max_size": "5gb"
          },
          "set_priority": {
            "priority": 100
          }
        }
      },
      "delete": {
        "min_age": "15d",
        "actions": {
          "delete": {}
        }
      }
    }
  }
}
```

Now, you should have an index lifecycle policy named 'ed-agent-log-policy'.

1. Create index template

   Index templates are useful to configure elastic indices before they are created.

Edgedelta agents are capable of streaming various types of observations to Elastic Search destination if configured so. The target index should ideally be created with our recommended index template. You can create the edgedelta elastic index template by running the command below in your Elastic's dev console. It will create an index template named 'ed-agent-log' with proper field mappings and refers to the lifecycle policy 'ed-agent-log-policy'.

```text
PUT _template/ed-agent-log?include_type_name
{
  "order": 0,
  "index_patterns": [
    "ed-agent-log-*"
  ],
  "settings": {
    "index": {
      "lifecycle": {
        "name": "ed-agent-log-policy",
        "rollover_alias": "ed-agent-log"
      },
      "number_of_shards": "1",
      "number_of_replicas": "1"
    }
  },
  "mappings": {
    "_doc": {
      "_source": {
        "excludes": [],
        "includes": [],
        "enabled": true
      },
      "_meta": {},
      "_routing": {
        "required": false
      },
      "dynamic": true,
      "numeric_detection": false,
      "date_detection": true,
      "dynamic_date_formats": [
        "strict_date_optional_time",
        "yyyy/MM/dd HH:mm:ss Z||yyyy/MM/dd Z"
      ],
      "dynamic_templates": [],
      "properties": {
        "msg": {
          "type": "text"
        },
        "alert_def_id": {
          "type": "keyword"
        },
        "k8s_namespace": {
          "type": "keyword"
        },
        "merge_level": {
          "type": "keyword"
        },
        "ecs_task_family": {
          "eager_global_ordinals": false,
          "norms": false,
          "index": true,
          "store": false,
          "type": "keyword",
          "index_options": "docs",
          "split_queries_on_whitespace": false,
          "doc_values": true
        },
        "k8s_controller_kind": {
          "type": "keyword"
        },
        "k8s_container_image": {
          "type": "keyword"
        },
        "title": {
          "eager_global_ordinals": false,
          "norms": false,
          "index": true,
          "store": false,
          "type": "keyword",
          "index_options": "docs",
          "split_queries_on_whitespace": false,
          "doc_values": false
        },
        "type": {
          "type": "keyword"
        },
        "src_name": {
          "type": "keyword"
        },
        "k8s_container_name": {
          "type": "keyword"
        },
        "score": {
          "type": "double"
        },
        "sub_type": {
          "type": "keyword"
        },
        "host": {
          "type": "keyword"
        },
        "capture_flush_mode": {
          "eager_global_ordinals": false,
          "norms": false,
          "index": false,
          "store": false,
          "type": "keyword",
          "split_queries_on_whitespace": false,
          "doc_values": false
        },
        "tag": {
          "type": "keyword"
        },
        "k8s_controller_logical_name": {
          "type": "keyword"
        },
        "timestamp_end": {
          "type": "date"
        },
        "value": {
          "type": "double"
        },
        "timestamp": {
          "index": true,
          "ignore_malformed": false,
          "store": false,
          "type": "date",
          "doc_values": true
        },
        "app": {
          "type": "keyword"
        },
        "capture_size": {
          "coerce": true,
          "index": false,
          "ignore_malformed": false,
          "store": false,
          "type": "long",
          "doc_values": false
        },
        "ecs_task_version": {
          "eager_global_ordinals": false,
          "norms": false,
          "index": true,
          "store": false,
          "type": "keyword",
          "split_queries_on_whitespace": false,
          "index_options": "docs",
          "doc_values": true
        },
        "stat_type": {
          "type": "keyword"
        },
        "docker_container_name": {
          "type": "keyword"
        },
        "conf_id": {
          "type": "keyword"
        },
        "edac_id": {
          "type": "keyword"
        },
        "ip": {
          "type": "ip"
        },
        "k8s_pod_name": {
          "type": "keyword"
        },
        "logical_source": {
          "type": "keyword"
        },
        "environment": {
          "type": "keyword"
        },
        "event_id": {
          "type": "keyword"
        },
        "capture_duration": {
          "eager_global_ordinals": false,
          "norms": false,
          "index": false,
          "store": false,
          "type": "keyword",
          "split_queries_on_whitespace": false,
          "doc_values": false
        },
        "ecs_container": {
          "eager_global_ordinals": false,
          "norms": false,
          "index": true,
          "store": false,
          "type": "keyword",
          "index_options": "docs",
          "split_queries_on_whitespace": false,
          "doc_values": true
        },
        "capture_bytesize": {
          "coerce": true,
          "index": false,
          "ignore_malformed": false,
          "store": false,
          "type": "long",
          "doc_values": false
        },
        "group_id": {
          "type": "keyword"
        },
        "org_id": {
          "type": "keyword"
        },
        "name": {
          "type": "keyword"
        },
        "alert_def_name": {
          "type": "keyword"
        },
        "ecs_cluster": {
          "eager_global_ordinals": false,
          "norms": false,
          "index": true,
          "store": false,
          "type": "keyword",
          "index_options": "docs",
          "split_queries_on_whitespace": false,
          "doc_values": true
        },
        "threshold_description": {
          "eager_global_ordinals": false,
          "norms": false,
          "index": false,
          "store": false,
          "type": "keyword",
          "split_queries_on_whitespace": false,
          "doc_values": false
        },
        "threshold_type": {
          "eager_global_ordinals": false,
          "norms": false,
          "index": false,
          "store": false,
          "type": "keyword",
          "split_queries_on_whitespace": false,
          "doc_values": false
        },
        "src_type": {
          "type": "keyword"
        },
        "region": {
          "type": "keyword"
        },
        "properties": {
          "eager_global_ordinals": false,
          "norms": false,
          "index": false,
          "store": false,
          "type": "keyword",
          "split_queries_on_whitespace": false,
          "doc_values": false
        },
        "docker_image": {
          "type": "keyword"
        }
      }
    }
  }
}
```

1. Create the first index to kick off daily index generation

So far we have created an index lifecycle policy and an index template. Now we will be creating the first index which inherits its field mapping and policy from the template.

Run below command in your Elastic's dev console

```text
PUT /%3Ced-agent-log-%7Bnow%2Fd%7D-000001%3E
{
 "aliases": {
   "ed-agent-log": {
     "is_write_index": true
   }
 }
}
```

Now visit the Index Management &gt; Indices and you should see a new index similar to this 'ed-agent-log-2020.10.22-000000'. \(the date should be today\)

Congratulations! Your Elastic environment is ready for Edge Delta. Now you can deploy an agent with Elastic destination pointing to index 'ed-agent-log'.