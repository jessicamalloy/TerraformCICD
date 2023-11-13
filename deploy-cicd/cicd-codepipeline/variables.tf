variable "project_name" {
  description = "Name of project."
  type        = string
  validation {
    condition     = length(var.project_name) < 42
    error_message = "Maximum length for project name is 41."
  }
}

variable "github_username" {
  description = "GitHub service account username."
  type        = string
  default     = "aibsgithub"
}

variable "github_owner" {
  description = "GitHub repo owner."
  type        = string
  default     = "AllenInstitute"
}

variable "github_oauth_token" {
  description = "OAuth token allowing access to repository."
  type        = string
  sensitive   = true
}

variable "github_branch" {
  description = "GitHub git branch."
  type        = string
}

variable "github_repo" {
  description = "GitHub git repo."
  type        = string
}

variable "variable_bucket" {
  description = "AWS S3 bucket that contains backend config and variable values"
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