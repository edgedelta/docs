---
description: >-
  This document outlines the environment variable parameters that can be passed
  while installing and deploying the Edge Delta agent.
---

# Environment Variables

## Frequently Used Environment Variables

<table>
  <thead>
    <tr>
      <th style="text-align:left">Key</th>
      <th style="text-align:left">Description</th>
      <th style="text-align:left">Value</th>
      <th style="text-align:left">Examples</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td style="text-align:left">ED_API_KEY</td>
      <td style="text-align:left">API Key used to pull agent&apos;s configuration details (generated via
        ED Admin Portal)</td>
      <td style="text-align:left">xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx</td>
      <td style="text-align:left">0a3a6ca3-0df0-45f8-8ea2-d1329ee3de60</td>
    </tr>
    <tr>
      <td style="text-align:left">ED_WORKFLOWS</td>
      <td style="text-align:left">Colon (:) separated workflow names that will enable all matching workflows
        and disable the rest together with ED_WORKFLOW_PREFIXES</td>
      <td style="text-align:left">name:name:...</td>
      <td style="text-align:left">workflow_1:workflow_2</td>
    </tr>
    <tr>
      <td style="text-align:left">ED_WORKFLOW_PREFIXES</td>
      <td style="text-align:left">Colon (:) separated workflow prefixes that will enable all matching workflows
        according their prefixes and disable the rest together with ED_WORKFLOWS</td>
      <td style="text-align:left">prefix:prefix:...</td>
      <td style="text-align:left">workflow_prod_:workflow_cache_</td>
    </tr>
    <tr>
      <td style="text-align:left">ED_LEADER_ELECTION_ENABLED</td>
      <td style="text-align:left">Should be set to "1" in order to enable agent leader election mechanism in Kubernetes environment.
        If not enabled, Kubernetes event ingestion would be disabled even if it is activated through configuration.
      </td>
      <td style="text-align:left">0 or 1</td>
      <td style="text-align:left">1</td>
    </tr>
    <tr>
      <td style="text-align:left">HTTP_PROXY</td>
      <td style="text-align:left">Proxy details for routing Edge Delta agent&apos;s outbound traffic through
        an HTTP internal proxy</td>
      <td style="text-align:left">domain:port</td>
      <td style="text-align:left">
        <p>http://127.0.0.1:3128</p>
        <p>127.0.0.1:3128</p>
      </td>
    </tr>
    <tr>
      <td style="text-align:left">HTTPS_PROXY</td>
      <td style="text-align:left">Proxy details for routing Edge Delta agent&apos;s outbound traffic through
        an HTTPs internal proxy</td>
      <td style="text-align:left">domain:port</td>
      <td style="text-align:left">
        <p>https://127.0.0.1:3128</p>
        <p>127.0.0.1:3128</p>
      </td>
    </tr>
    <tr>
      <td style="text-align:left">NO_PROXY</td>
      <td style="text-align:left">Disable proxy for requests hitting a specific destination</td>
      <td style="text-align:left">domain:port</td>
      <td style="text-align:left">https://your-endpoint.com</td>
    </tr>
    <tr>
      <td style="text-align:left">STORE_PORT</td>
      <td style="text-align:left">Port number to expose agent metrics storage</td>
      <td style="text-align:left">port</td>
      <td style="text-align:left">6062</td>
    </tr>
  </tbody>
</table>

## Examples - Kubernetes \(yml configuration\)

Snippet pulled from: [https://edgedelta.github.io/k8s/edgedelta-agent.yml](https://edgedelta.github.io/k8s/edgedelta-agent.yml)

```yaml
spec:
  containers:
  - name: edgedelta
    image: docker.io/edgedelta/agent:latest
    env:
      - name: ED_API_KEY
        valueFrom:
          secretKeyRef:
            key: ed-api-key
            name: ed-api-key
      - name: HTTPS_PROXY
        value: <your proxy details>
```

Note that "ED\_API\_KEY" is not defined in the yaml as clear text. Using the Kubernetes secrets mechanism, secret value should be defined with below command within the Kubernetes cluster:

```bash
kubectl create secret generic ed-api-key --namespace=edgedelta --from-literal=ed-api-key="YOUR_API_KEY_VALUE"
```

## Examples - Kubernetes \(helm deployment\)

Download [values.yaml file](https://raw.githubusercontent.com/edgedelta/charts/master/edgedelta/values.yaml).

Edit the values.yaml file to set optional parameters by commenting out and editing relevant sections such as:

* Secret API key instead of clear text API key
* Frequently used environment variables for agent
* One or more custom environment variables which can be referred to in the config as [Config Variables](../configuration/variables.md)
* One or more custom secret environment variables which can be also used as custom environment variables but kept as Kubernetes secrets

After the file is updated follow [Helm Installation](helm.md) steps.

Do not forget to create secrets as instructed in values.yaml after namespace is created in above step, otherwise agent will fail to start.

## Examples - Linux/MacOSX

ED\_ENV\_VARS is a special variable used during installation where multiple environment variables specified in following comma separated format: "variable1=value1,variable2=value2"

```bash
sudo ED_API_KEY=<your api key> \
ED_ENV_VARS="HTTPS_PROXY=<your proxy details>" \
bash -c "$(curl -L https://release.edgedelta.com/release/install.sh)"
```

## Examples - Docker

```bash
docker run --rm -d --name edgedelta \
-v /var/run/docker.sock:/var/run/docker.sock:ro \
-e "ED_API_KEY=<your api key>" \
-e "HTTPS_PROXY=<your proxy details>" \
edgedelta/agent:latest
```

## Example - Windows

On Windows systems use the following command to define environment variables globally. Agent service needs to be restarted after setting the variable.

`[System.Environment]::SetEnvironmentVariable('HTTP_PROXY', '<your proxy details>',[System.EnvironmentVariableTarget]::Machine)`

