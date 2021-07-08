---
description: >-
  This document describes template values possible for the payload contents of triggers.
---

# Template Values

Template values can be defined as "variables that are defined by our agent and can be substituted according to their matching template names". For instance, if an agent is running with a config ID `ed832df2-740b-41c5-863e-3aa4b03932f1` and if someone defined a trigger payload like `... {{ .ConfigID }} ...`, incoming trigger message will become `... ed832df2-740b-41c5-863e-3aa4b03932f1 ...`. These "placeholders" can be defined as "template values" and in the below table, template values and their definitions are shown.

<table>
  <thead>
    <tr>
      <th style="text-align:left; width: 275px;">Template Value</th>
      <th style="text-align:left">Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td style="text-align:left">{% raw %} {{ .Title }} {% endraw %}</td>
      <td style="text-align:left">Pre-generated title for triggers</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .Message }} {% endraw %}</td>
      <td style="text-align:left">Pre-generated message for triggers</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .Tag }} {% endraw %}</td>
      <td style="text-align:left">Tag which is defined in the configuration file</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .ConfigID }} {% endraw %}</td>
      <td style="text-align:left">API Key that is consumed by the agent</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .Host }} {% endraw %}</td>
      <td style="text-align:left">Host name of the machine that agent is working on</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .EDAC }} {% endraw %}</td>
      <td style="text-align:left">Unique identifier for alert type observations</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .Source }} {% endraw %}</td>
      <td style="text-align:left">Source of the observation</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .SourceType }} {% endraw %}</td>
      <td style="text-align:left">Type of the source such as K8s, Docker, ECS and etc.</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .SourceAttributes }} {% endraw %}</td>
      <td style="text-align:left">Attributes of the source such as pod name, namespace, controller name etc. for Kubernetes as well as the source name</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .MetricName }} {% endraw %}</td>
      <td style="text-align:left">Metric name that caused the alert</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .Epoch }} {% endraw %}</td>
      <td style="text-align:left">Timestamp of alert ingestion</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .Timestamp }} {% endraw %}</td>
      <td style="text-align:left">Timestamp of the alert</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .MatchedTerm }} {% endraw %}</td>
      <td style="text-align:left">Log that caused the alert</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .ProcessorDescription }} {% endraw %}</td>
      <td style="text-align:left">Processor description from the config</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .ThresholdDescription }} {% endraw %}</td>
      <td style="text-align:left">Threshold description from the threshold alert</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .ThresholdValue }} {% endraw %}</td>
      <td style="text-align:left">Threshold value that current metric value will be checked against</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .ThresholdType }} {% endraw %}</td>
      <td style="text-align:left">Threshold type (such as upper and lower) that current metric value will be checked against</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .CurrentValue }} {% endraw %}</td>
      <td style="text-align:left">Current value of the metric</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .ECSCluster }} {% endraw %}</td>
      <td style="text-align:left">For a source with "ECS" type, cluster name of the replica that agent listens</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .ECSContainerName }} {% endraw %}</td>
      <td style="text-align:left">For a source with "ECS" type, container name of the replica that agent listens</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .ECSTaskFamily }} {% endraw %}</td>
      <td style="text-align:left">For a source with "ECS" type, task family of the replica that agent listens</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .ECSTaskVersion }} {% endraw %}</td>
      <td style="text-align:left">For a source with "ECS" type, task version of the replica that agent listens</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .K8sNamespace }} {% endraw %}</td>
      <td style="text-align:left">For a source with "Kubernetes" type, namespace of the replica that agent listens</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .K8sControllerKind }} {% endraw %}</td>
      <td style="text-align:left">For a source with "Kubernetes" type, controller type (such as ReplicaSet, DaemonSet etc.) of the replica that agent listens</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .K8sControllerLogicalName }} {% endraw %}</td>
      <td style="text-align:left">For a source with "Kubernetes" type, logical name of the replica's controller that agent listens</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .K8sPodName }} {% endraw %}</td>
      <td style="text-align:left">For a source with "Kubernetes" type, pod name of the replica that agent listens</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .K8sContainerName }} {% endraw %}</td>
      <td style="text-align:left">For a source with "Kubernetes" type, container name of the replica that agent listens</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .K8sContainerImage }} {% endraw %}</td>
      <td style="text-align:left">For a source with "Kubernetes" type, image name of the replica's container that agent listens</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .DockerContainerName }} {% endraw %}</td>
      <td style="text-align:left">For a source with "Docker" type, container name of the replica that agent listens</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .DockerContainerName }} {% endraw %}</td>
      <td style="text-align:left">For a source with "Docker" type, image name of the replica that agent listens</td>
    </tr>
    <tr>
      <td style="text-align:left">{% raw %} {{ .FileGlobPath }} {% endraw %}</td>
      <td style="text-align:left">For a source with "File" type, full path of the file that agent listens</td>
    </tr>
  </tbody>
</table>