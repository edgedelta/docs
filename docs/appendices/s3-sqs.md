## Adding S3 input

1. Create an SQS queue to receive messages from S3. SQS queue must have a policy that allows s3 to send messages to it.
Policy example:
```
{
  "Version": "2008-10-17",
  "Id": "__default_policy_ID",
  "Statement": [
    {
      "Sid": "__owner_statement",
      "Effect": "Allow",
      "Principal": {
        "AWS": "arn:aws:iam::<account id>:root"
      },
      "Action": "SQS:*",
      "Resource": "arn:aws:sqs:us-west-2:<account id>:"
    },
    {
	   "Sid": "s3_send_statement",
	   "Effect": "Allow",
	   "Principal": {
	    "Service": "s3.amazonaws.com"  
	   },
	   "Action": [
	    "SQS:SendMessage"
	   ],
	   "Resource": "arn:aws:sqs:us-west-2:<account id>:my-sqs",
	   "Condition": {
	      "ArnLike": { "aws:SourceArn": "arn:aws:s3:*:*:my-bucket" },
	      "StringEquals": { "aws:SourceAccount": "<account id>" }
      }
    }
  ]
}
```

2. Configure the s3 bucket to send notifications to the SQS queue. This is documented [here](https://docs.aws.amazon.com/AmazonS3/latest/userguide/NotificationHowTo.html).

3. Create a new IAM user with programmatic access type. It will be used by agents to access sqs and s3. It should have a policy like this:
```
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Sid": "VisualEditor0",
            "Effect": "Allow",
            "Action": [
                "sqs:DeleteMessage",
                "s3:GetObject",
                "sqs:DeleteMessageBatch",
                "sqs:ReceiveMessage"
            ],
            "Resource": [
                "arn:aws:s3:::my-bucket/*",
                "arn:aws:sqs:us-west-2:<account id>:my-sqs"
            ]
        }
    ]
}
```

4. Create access key for the IAM user. The access key id and secret will be used to configure the agent in next step.


5. Add an s3 input to the agent
```
inputs:
  s3_sqs:
    - labels: "errorcheck"
      sqs_url: "https://sqs.us-west-2.amazonaws.com/<account id>/my-sqs"
      access_key_id: ""
      access_secret: ""
      # region where the bucket and sqs queue located
      region: "us-west-2"
```
