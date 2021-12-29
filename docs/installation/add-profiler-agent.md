Overview
--------

You can use this document to learn how to deploy the Edge Delta Agent with a profiler.

The Edge Delta Agent is a Go-based binary that (when specified during deployment) can expose CPU and memory profiling information.

This information can be useful to understand specific activity, such as understanding why CPU usage is high.

After deployment, to obtain the data, you can run a command to download a report. 

* * * * *

Deploy the Agent with a Profiler
--------------------------------

> **Note**
>
> You can only add a profiler while you deploy an agent.
>
> You cannot add a profiler to an existing agent deployment.

To deploy the agent with a profiler, follow the regular installation instructions for each agent type; however, you must add **PROFILER_PORT** for port **6060** to the installation command.

Review the following examples.

> **Note**
>
> In the examples below, replace the ED API KEY with your own key.

**Linux Example**

For instance, for **Linux**, the regular deployment command is: 

<code>ED_API_KEY=f1111e-e1d1-1ad1-b11d-d1a11111b1 bash -c "$(curl -L https://release.edgedelta.com/release/install.sh)"</code>

To deploy with a profiler, the command would be:

<code>ED_ENV_VARS=PROFILER_PORT=6060 ED_API_KEY=f1111e-e1d1-1ad1-b11d-d1a11111b1 bash -c "$(curl -L https://release.edgedelta.com/release/install.sh)"</code>

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

> **Note**
>
> You can also view the data using the ppof tool.
> <br> 1. Run the following command to install pprof, which requires golang installed: <code>go install github.com/google/pprof@latest</code>
> <br> 2. Run the following command to visualize pprof results: <code>pprof -web /tmp/cpu.pb.gz</code>



Based on the information you want to obtain, run the following command: 

For information on heap, run: 

<code>curl -sK -v <http://localhost:8080/debug/pprof/heap> --output /tmp/heap.pb.gz</code>

For information on CPU, run:

<code>curl <http://localhost:6060/debug/pprof/profile?seconds=60> --output /tmp/cpu.pb.gz</code>

For information on goroutine, run: 

<code>curl <http://localhost:6060/debug/pprof/goroutine> --output /tmp/goroutine.pb.gx</code>

* * * * *
