---
description: >-
  The following document covers the network access requirements for the Edge
  Delta Agent.
---

# Agent Network Access Requirements

Edge Delta Agent can be deployed in restricted network environments where outgoing network traffic to the Internet is strictly moderated. In such an environment network policies should be updated by enabling outgoing access to these destinations and bypassing HTTP/HTTPS proxy.

## Edge Delta Backend

The agent requires outbound network access to Edge Delta central systems to be fully functionally. These destinations are account specific. If you have restricted outbound network policy contact [support@edgedelta.com](mailto:support@edgedelta.com) to the get list of destination addresses for your account.

## Outputs

[Outputs](../configuration/outputs.md) to 3rd party or on premise systems also require outbound network access. While configuring agents make sure **endpoint** of configured output is also accessible by agent through the network.