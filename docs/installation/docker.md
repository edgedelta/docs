---
description: >-
  The following document covers the process for deploying the Edge Delta service
  as a Docker container for containerized environments.
---

# Docker

## Overview 

You can use this document to learn how to install the Edge Delta Agent for your Docker-based software platform. 

Edge Delta has a Docker container image that can be deployed as a Sidecar or DaemonSet architecture to analyze telemetry from other Docker containers running on the host, while also providing isolation and encapsulation.

> **Note**
>
> This document is designed for existing users. If you have not created an account with Edge Delta, then see [Basic Onboarding](../basic-onboarding.md).

***

## Step 1: Create a Configuration and Download the Agent

1. In the Edge Delta App, on the left-side navigation, click **Agent Settings**.
2. Click **Create Configuration**.
3. Select **Docker**.
4. Click **Save**.  
5. In the table, locate the newly created configuration, and then click the corresponding green rocket to deploy additional instructions.
6. Click **Docker**.
7. In the window that appears, copy the command.
  - This window also displays your API key. Copy this key for a later step. 
8. Paste and then run the command on the host where you want to deploy Edge Delta. The installation process will begin. 

***

## Step 2: Run the Container 

When you run the Edge Delta container, you can either have the Edge Delta container fetch its configuration from the Edge Delta Central Configuration Backend (recommended) or use a local configuration file.

### Option 1: Run with an API Key Utilizing Central Configuration Backend \(CCB\)

> **Note**
>
> To learn more about Central Configuration Backend, see [CCB](../configuration/ccb.md).

> **Note**
>
> The container must have internet access to fetch the configuration.

1. Run the following command. Replace the &lt;YOUR\_API\_KEY&gt; field with the key you copied earlier. 

```
docker run -it \
-v /var/run/docker.sock:/var/run/docker.sock:ro \
-e "ED_API_KEY=<YOUR_API_KEY>" \
docker.io/edgedelta/edgedelta/agent:latest
```

### Option 2: Run with a Local Configuration File

1. Run the following command. Replace `$PWD/config.yml`with the absolute path of the local configuration file on host.

```
docker run -it \
-v /var/run/docker.sock:/var/run/docker.sock:ro \
-v $PWD/config.yml:/edgedelta/config.yml \
docker.io/edgedelta/edgedelta/agent:latest
```
***

## Limit Resource Consumption

You can limit the CPU or memory resources that the Edge Delta container consumes. 

The following example limits the Edge Delta container to 25% CPU and 256 MB of memory.

```
docker run -it --cpus=".25" --memory="256m" \
-v /var/run/docker.sock:/var/run/docker.sock:ro \
-v $PWD/config_docker.yml:/edgedelta/config.yml \
docker.io/edgedelta/edgedelta/agent:latest
```

***

## Troubleshoot the Agent

To verify that the agent's container is running, run the following Docker command. If the container is running, then a container containing **edgedelta** in the IMAGE name should display. 

```
docker ps
```

***

To check the agent's log file for any errors that may indicate an issue with the agent, configuration, or deployment settings, run the following command to view all containers \(whether running or not\). This command will display the CONTAINER ID of the Edge Delta Agent.

```
docker ps -a
```

Copy the CONTAINER ID of the agent, which should be listed at the top of the list of containers, and then run the following command with the agent's CONTAINER ID:

```
docker logs CONTAINERID
```

***
