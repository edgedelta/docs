---
description: >-
  The following document covers the process for deploying the Edge Delta agent
  as a Daemon Service  on your Amazon ECS cluster.
---

# Amazon ECS

## Create ECS Task Definition

Download [https://release.edgedelta.com/edgedelta-ecs.json](https://release.edgedelta.com/edgedelta-ecs.json) task definition

Change &lt;YOUR\_ED\_API\_KEY&gt; in task definition with your Edge Delta config API key.

Create agent task definition using AWS CLI:

```text
aws ecs register-task-definition --cli-input-json file://path_to_edgedelta-ecs.json
```

Or Create agent task definition using AWS Console UI:

Navigate to ECS/Task Definitions/Create new Task Definition, scroll bottom select Configure via JSON and paste task definition JSON.

## Run agent as an ECS Daemon Service

You may run agent as an ordinary ECS task which will only collect logs from the containers running on the EC2 instance the task is running.

To properly collect all logs from containers on all instances in your cluster you need to run agent as a Daemon Service.

In AWS Console UI navigate to ECS and find your cluster, then follow below steps:

* Under Services click the Create
* Select EC2 Launch Type 
* Select the task definition created above in dropdown
* Select DAEMON service type and set a service name, click Next Step
* Select None for load balancer, click Next Step
* Auto Scaling is already disabled, click Next Step
* Review summary and finish by clicking Create Service

Make sure your container definition for other tasks or services does not have`logConfiguration.logDriver` parameter. With no logging driver logs are written to standard output and collected by the agent service.

In agent configuration yaml on [https://admin.edgedelta.com](https://admin.edgedelta.com) make sure you have container input source is enabled as seen below to collect container logs:

```text
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

```text
workflows:
  my-workflow:
    input_labels:
      - containers
    ...
```

## Useful Tips

### File Monitoring

You may want to monitor additional log files on EC2 instances in your ECS cluster.

Update `mountpoints` and `volumes` section in [edgedeleta-ecs.json](https://release.edgedelta.com/edgedelta-ecs.json).

Below example change allows you to also monitor log files in `/var/log/ecs/` on EC2 instance which are about ECS system events.

```text
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

Do not forget to add file input in agent yaml config to monitor mounted path which is `/host/var/log/ecs/` in above sample. See [files](https://docs.edgedelta.com/configuration/inputs#files) for further details.

