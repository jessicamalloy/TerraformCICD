variable "project_name" {
  description = "Name of project."
  type        = string
  validation {
    condition     = length(var.project_name) < 42
    error_message = "Maximum length for project name is 41."
  }
}

variable "build_timeout" {
  description = "Timeout for build in minutes"
  type        = number
  default     = 10
}

variable "environment_variables" {
  description = "List of additional environment variables to pass to build."
  type = list(object({
    name  = string
    value = string
    type  = string
  }))
  default = []
}

variable "s3_bucket_resources" {
  description = "List of additional s3 bucket resources that codebuild needs access to."
  type        = list(string)
  default     = []
}

variable "buildspec" {
  description = "File contents of the buildspec."
  type        = string
}

variable "build_env_image" {
  description = "Docker image to use for this build project"
  type        = string
  default     = "aws/codebuild/standard:7.0"
}

variable "variable_bucket" {
  description = "Name of the AWS S3 bucket that contains backend config and variable values"
  type = string
}

variable "backend_config_bucket" {
  description = "Name of the AWS S3 bucket that stores the Terraform state"
  type = string
}

variable "backend_config_dynamodb_table" {
  description = "The DynamoDB table for locking Terraform state"
  type = string
}

variable "region" {
  description = "AWS region where service is deployed."
  type        = string
  default     = "us-west-2"
}

variable "aws_account_id" {
  description = "AWS Account ID"
  type        = string
}
