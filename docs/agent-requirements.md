---
description: >-
  The following document outlines the basic requirements for agent installation.
---

## Overview

Before you install the Edge Delta Agent, review the following software and hardware requirements.

***

## Operating System Compatibility
  
| Operating system | Architecture    | Distributions                                                                                                        |
|------------------|-----------------|----------------------------------------------------------------------------------------------------------------------|
| Windows          | x86_64 <p>x86   | Windows Server 2019 <p>Windows 10                                                                                    |
| Linux            | arm64 <p>x86_64 | Amazon Linux 2<p>Centos 8<p>Centos 7<p>Debian 10 (Buster)<p>Debian 9 (Stretch)<p>Nixos<p>Ubuntu<p>Raspbian    |
| Darwin           | arm64 <p>x86_64 | Not applicable                                                                                                       |

***

## Resource Requirements

| Resource type | Minimum requirement |
|---------------|---------------------|
| CPU           | 0.2 core            |
| RAM           | 256MB               |

***

## Browser Support

The Edge Delta Admin portal supports the current version of the following browsers:

  * Chrome
  * Firefox

> **Note**
> 
> There are reported compatibility issues with Safari. 

> **Note**
> 
> Edge Delta does not recommend that you use a mobile device to access the Edge Delta Admin portal.

***

## Firewall Rules

To successfully integrate with Edge Delta, you must update your network policies for outgoing access.

While the Edge Delta Agent can be deployed in restricted network environments where outgoing network traffic to the internet is strictly moderated, you must update network policies to enable outgoing access to these destinations and to bypass HTTP/HTTPS proxies.

Review the following network access requirements to ensure a successful connection with Edge Delta:

| Traffic  | Port | Destination URI                                                      | Service                            |
|----------|------|----------------------------------------------------------------------|------------------------------------|
| Outbound | 443  | https://api.edgedelta.com                                            | Backend Access to Edge Delta       |
| Outbound | 443  | https://us-west-2-1.aws.cloud2.influxdata.com                        | Metric Collection                  |
| Outbound | 443  | https://53f55016dbcb4e5f8b7fdf4a14a4cacc.us-west-2.aws.found.io:9243 | Log Pattern/Sample Collection      |
| Outbound | 443  | https://docs.aws.amazon.com/general/latest/gr/aws-ip-ranges.html     | Amazon S3 access for Log Archiving |


***
