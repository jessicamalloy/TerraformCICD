resource "aws_s3_bucket" "terraform_config_bucket" {
  bucket        = "${var.project_name}-terraform-config-files"
  force_destroy = true
  
  tags = {
    ProjectName = var.project_name
  }
}

resource "aws_s3_object" "cicd-backend-config-file" {
  bucket = aws_s3_bucket.terraform_config_bucket.bucket
  key    = "cicd/backend.config"
  source = "environments/${var.environment}/backend.config"

  etag = filemd5("environments/${var.environment}/backend.config")

  depends_on = [ aws_s3_bucket.terraform_config_bucket ]
}

resource "aws_s3_object" "cicd-variables-file" {
  bucket = aws_s3_bucket.terraform_config_bucket.bucket
  key    = "cicd/variables.tfvars"
  source = "environments/${var.environment}/variables.tfvars"

  etag = filemd5("environments/${var.environment}/variables.tfvars")

  depends_on = [ aws_s3_bucket.terraform_config_bucket ]
}

resource "aws_s3_object" "service-backend-config-file" {
  bucket = aws_s3_bucket.terraform_config_bucket.bucket
  key    = "service/backend.config"
  source = "../deploy/environments/${var.environment}/backend.config"

  etag = filemd5("../deploy/environments/${var.environment}/backend.config")

  depends_on = [ aws_s3_bucket.terraform_config_bucket ]
}

resource "aws_s3_object" "service-variables-file" {
  bucket = aws_s3_bucket.terraform_config_bucket.bucket
  key    = "service/variables.tfvars"
  source = "../deploy/environments/${var.environment}/variables.tfvars"

  etag = filemd5("../deploy/environments/${var.environment}/variables.tfvars")

  depends_on = [ aws_s3_bucket.terraform_config_bucket ]
}
