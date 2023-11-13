resource "aws_iam_role" "codebuild_service_role" {
  name = "${var.project_name}-codebuild-service-role"

  managed_policy_arns = ["arn:aws:iam::aws:policy/SecretsManagerReadWrite"]
  assume_role_policy  = <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Principal": {
        "Service": "codebuild.amazonaws.com"
      },
      "Action": "sts:AssumeRole"
    },
    {
      "Effect": "Allow",
      "Principal": {
        "Service": "codebuild.amazonaws.com"
      },
      "Action": "sts:AssumeRole"
    }
  ]
}
EOF
}

resource "aws_iam_role_policy" "codebuild_service_policy" {
  role = aws_iam_role.codebuild_service_role.name

  policy = <<POLICY
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Resource": [
        "*"
      ],
      "Action": [
        "route53:*",
        "iam:*",
        "logs:CreateLogGroup",
        "logs:DeleteLogGroup",
        "logs:CreateLogStream",
        "logs:PutLogEvents",
        "logs:PutRetentionPolicy",
        "logs:Describe*",
        "logs:List*",

        "cloudformation:Describe*",
        "cloudformation:Delete*",
        "cloudformation:CreateStack",
        "cloudformation:Get*",

        "acm:RequestCertificate",
        "acm:DeleteCertificate",
        "acm:Describe*",
        "acm:List*",

        "codebuild:CreateProject",
        "codebuild:DeleteProject",
        "codebuild:BatchGetProjects",
        "codepipeline:CreatePipeline",
        "codepipeline:DeletePipeline",
        "codepipeline:Get*",
        "codepipeline:TagResource",
        "codepipeline:List*",

        "ec2:Describe*",
        "ec2:Create*",
        "ec2:Delete*",
        "ec2:ModifyVpcAttribute",
        "ec2:RevokeSecurityGroupEgress",
        "ec2:AttachInternetGateway",
        "ec2:AuthorizeSecurityGroupIngress",
        "ec2:AuthorizeSecurityGroupEgress",
        "ec2:AssociateRouteTable",
        "ec2:ReplaceNetworkAclAssociation",

        "ecs:Create*",
        "ecs:Describe*",
        "ecs:Delete*",
        "ecs:RegisterTaskDefinition",
        "ecs:DeregisterTaskDefinition",

        "ecr:Describe*",
        "ecr:DeleteRepository",
        "ecr:List*",
        "ecr:CreateRepository",
        "ecr:TagResource",

        "elasticloadbalancing:Describe*",
        "elasticloadbalancing:Create*",
        "elasticloadbalancing:Delete*",
        "elasticloadbalancing:ModifyTargetGroupAttributes",
        "elasticloadbalancing:ModifyLoadBalancerAttributes",
        "elasticloadbalancing:AddTags",
        "elasticloadbalancing:SetSecurityGroups",

        "lambda:DeleteFunction",
        "lambda:CreateFunction",
        "lambda:InvokeFunction",
        "lambda:Get*",
        "lambda:TagResource",
        "lambda:AddPermission",
        "lambda:RemovePermission",

        "rds:Describe*",
        "rds:List*",
        "rds:Create*",
        "rds:AddTagsToResource",

        "s3:Create*",
        "s3:List*",
        "s3:Put*",
        "s3:Get*",
        "s3:Delete*"
      ]
    },
    {
      "Effect": "Allow",
      "Action": [
        "dynamodb:*"
      ],
      "Resource": [
        "arn:aws:dynamodb:${var.region}:${var.aws_account_id}:table/${var.backend_config_dynamodb_table}"
      ]
    },
    {
      "Effect": "Allow",
      "Action": [
        "s3:*"
      ],
      "Resource": ${jsonencode(concat(
  [
    "${aws_s3_bucket.project_bucket.arn}", # bucket with terraform artifacts
    "${aws_s3_bucket.project_bucket.arn}/*",
    "arn:aws:s3:::${var.variable_bucket}", # bucket with terraform config files
    "arn:aws:s3:::${var.variable_bucket}/*",
    "arn:aws:s3:::${var.backend_config_bucket}", # bucket that stores the terraform state
    "arn:aws:s3:::${var.backend_config_bucket}/*"
  ],
  var.s3_bucket_resources
))}
    },
    {
      "Effect": "Allow",
      "Action": [
        "ssm:GetParameters"
      ],
      "Resource": [
        "*"
      ]
    }
  ]
}
POLICY
}