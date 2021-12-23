Overview
--------

You can use this document to learn how to deploy the Edge Delta Agent with a profiler.

> **Note**
>
> This document only applies to Go-based web applications.

For Go-based web applications, you can add a profiler during agent deployment. A profiler is useful to obtain data about instances where the agent is deployed, such as reviewing why CPU usage is high. 

Afterwards, to obtain the data data, you can run a command to download a report. 

* * * * *

Deploy the Agent with a Profiler
--------------------------------

> **Note**
>
> You can only add a profiler while you deploy an agent.
>
> You cannot add a profiler to an existing agent deployment.

To deploy the agent with a profiler, follow the installation instructions for each agent type; however, you must add **PROFILER_PORT** for port **6060** to the installation command.

**Linux Example**

For instance, for **Linux**, the regular deployment command is: 

<code>ED_API_KEY=f1111e-e1d1-1ad1-b11d-d1a11111b1 bash -c "$(curl -L https://release.edgedelta.com/release/install.sh)"</code>

To deploy with a profiler, the command would be:

<code>PROFILER_PORT=6060 ED_API_KEY=f1111e-e1d1-1ad1-b11d-d1a11111b1 bash -c "$(curl -L https://release.edgedelta.com/release/install.sh)"</code>

**Docker Example**

In another example, for **Docker**, the regular deployment is:

<code>docker run --rm -d --name edgedelta -v /var/run/docker.sock:/var/run/docker.sock:ro -e "ED_API_KEY=f1111e-e1d1-1ad1-b11d-d1a11111b1" gcr.io/edgedelta/agent:latest</code>

To deploy with a profiler, the command would be: 

<code>docker run --rm -d --name edgedelta -v /var/run/docker.sock:/var/run/docker.sock:ro -e "ED_API_KEY=f1111e-e1d1-1ad1-b11d-d1a11111b1" -e "PROFILER_PORT=6060" gcr.io/edgedelta/agent:latest</code>

Review the following installation documents. Remember to add **PROFILER_PORT** for port **6060** to the installation command:

-   [Install the Agent for Windows](./windows.md)
-   [Install the Agent for MacOS](macos.md) 
-   [Install the Agent for Linux](linux.md)
-   [Install the Agent for Docker](docker.md)
-   [Install the Agent for Kubernetes](kubernetes.md)
-   [Install the Agent for Kubernetes with Helm](helm.md)
-   [Install the Agent for AWS ECS](amazon-ecs.md)

* * * * *

Obtain a Profiler Report 
-------------------------

When you obtain a profiler report, you can send the report to Edge Delta for analysis. 

Based on the information you want to obtain, run the following command: 

For information on Heap, run: 

<code>curl -sK -v <http://localhost:8080/debug/pprof/heap> > heap.out</code>

For information on CPU, run:

<code>curl <http://localhost:6060/debug/pprof/profile?seconds=60> --output /tmp/cpu.pb.gz</code>

For information on goroutine, run: 

  <code>curl <http://localhost:6060/debug/pprof/goroutine> > goroutine.out</code>

* * * * *
