variable "project_name" {
  description = "Name of project."
  type        = string
  default     = "terraform-cicd-poc-cicd"
  validation {
    condition     = length(var.project_name) < 42
    error_message = "Maximum length for project name is 41."
  }
}

variable "environment" {
  description = "AWS environment to deploy to"
  type = string
  default = "sandbox"
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