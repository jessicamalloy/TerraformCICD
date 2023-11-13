variable "project_name" {
  description = "Name of project."
  type        = string
  default     = "terraform-cicd-poc-cicd"
  validation {
    condition     = length(var.project_name) < 42
    error_message = "Maximum length for project name is 41."
  }
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


// Github 
variable "github_branch" {
  description = "GitHub git branch."
  type        = string
}

variable "github_username" {
  description = "GitHub service account username. "
  type        = string
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

variable "github_access_token" {
  description = "GitHub personal access token for nuget package access."
  type        = string
  sensitive   = true
}

variable "dynamodb_table" {
  description = "DynamoDB table used for managing Terraform backend state (gotten from backend.config)"
  type = string
}

variable "bucket" {
  description = "Name of the S3 bucket used for managing Terraform backend state (gotten from backend.config)"
  type = string
}

variable "environment" {
  description = "AWS environment to deploy to"
  type = string
  default = "sandbox"
}

variable "service_pipeline" {
  description = "Name of the codepipeline being created by this codepipeline"
  type        = string
}
