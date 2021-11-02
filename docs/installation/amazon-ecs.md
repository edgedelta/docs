---
description: >-
  The following document covers the process for deploying the Edge Delta agent
  as a Daemon Service  on your Amazon ECS cluster.
---

# Amazon ECS

## Overview

You can use this document to learn how to deploy the Edge Delta Agent as a Daemon service on your Amazon ECS cluster.

***

## Step 1: Create a Configuration 

1. In the Edge Delta Admin Portal, on the left-side navigation, click **Agent Settings**.
2. Click **Create Configuration**.
3. Select the desired platform.
4. Click **Save**.  
5. In the table, locate the newly created configuration, and then click the corresponding green rocket to deploy additional instructions.
6. Click the desired platform. 
7. In the window that appears, copy the API key. 

***

## Step 2: Create an ECS Task Definition

1. Download the [https://release.edgedelta.com/edgedelta-ecs.json](https://release.edgedelta.com/edgedelta-ecs.json) task definition.
2. In the task definition, replace &lt;YOUR\_ED\_API\_KEY&gt; with your copied API key.
3. Create the agent task definition via the AWS CLI or via the AWS console. 
  - Via the AWS CLI:

```
aws ecs register-task-definition --cli-input-json file://path_to_edgedelta-ecs.json
```

  - Via the AWS Console: 
    - Navigate to the Elastic Container Service (ECS) section. 
    - On the left-side navigation, click **Task Definitions**.
    - Click **Create new Task Definition**.
    - Select a launch type compatibility, and then click Next step. 
    - Navigate to the bottom of the page, and then click **Configure via JSON**.
    - Paste the paste task definition JSON, and then click **Save**. 
    - Click **Create**. 

***

## Step 3: Run the Agent as an ECS Daemon Service

While you can use the agent as an ordinary ECS task, you will only collect logs from the containers running on the EC2 instance the task is running. As a result, to properly collect all logs from containers on all instances in your cluster, you need to run the agent as a Daemon Service.

1. In the AWS Console, navigate to the Elastic Container Service (ECS) section, and then find your cluster.
2. Under **Services**, click the **Create**.
3. Select **EC2 Launch Type**.
4. In the drop-down menu, select the task definition you created.  
5. Select **DAEMON service type**, set a service name, and then click **Next step**.
6. Select **None for load balance**r, click **Next step**.
7. By default, auto scaling is disabled. click **Next step**.
8. Review the summary, and then click **Create Service**.

## Step 4: Verify Defintions and Configurations

1. Verify that the container definition for other tasks or services does not have the`logConfiguration.logDriver` parameter. Without a logging driver, logs are written to standard output and collected by the agent service.
2. In the Edge Delta Admin portal, in the left-side navigation, click **Agent Settings**. Locate the agent configuration, and then click the edit icon. 
3. In the agent YAML configuration file, verify that you have the container input source is enabled. Review the following example to view how to collect container logs:

```yaml
inputs:
  container_stats:
    enabled: true
    labels: "docker"
  containers:
    - labels: "containers"
      include:
        - "image=.*"
 ...
```

Containers should be referred in your workflow:

```yaml
workflows:
  my-workflow:
    input_labels:
      - containers
    ...
```

***

## File Monitoring

As an optional step, you can monitor for additional log files on your EC2 instances in your ECS cluster.

To do so, update the `mountpoints` and `volumes` section in the [edgedeleta-ecs.json](https://release.edgedelta.com/edgedelta-ecs.json).

Review the following example to understand how to monitor log files in `/var/log/ecs/` on an EC2 instance.

Additionally, you must add a file input in the agent YAML configuration file to monitor the mounted path. In the example below, the path is /host/var/log/ecs/. 


```
   "mountPoints": [
        {
          "containerPath": "/var/run/docker.sock",
          "sourceVolume": "docker_sock",
          "readOnly": true
        },
        {
          "containerPath": "/host/var/log/ecs/",
          "sourceVolume": "ecs_logs",
          "readOnly": true
        },
      ],
 ...

"volumes": [
  {
    "host": {
      "sourcePath": "/var/run/docker.sock"
    },
    "name": "docker_sock"
  },
  {
    "host": {
      "sourcePath": "/var/log/ecs/"
    },
    "name": "ecs_logs"
  }
],
...
```


***
