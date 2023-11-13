provider "aws" {
  region = var.region
}

terraform {
  required_version = ">= 1.0.4"
  required_providers {
    aws  = "~> 4.11.0"
  }
  backend "s3" {
    encrypt        = true
  }
}
