resource "aws_s3_bucket" "project_bucket" {
  bucket        = "${var.project_name}-artifacts"
  force_destroy = true

  tags = {
    ProjectName = var.project_name
  }
}

resource "aws_codebuild_project" "cicd_codebuild" {
  name          = var.project_name
  description   = "Infrastructure codebuild for project: ${var.project_name}"
  build_timeout = var.build_timeout
  service_role  = aws_iam_role.codebuild_service_role.arn

  artifacts {
    type = "CODEPIPELINE"
  }

  source {
    type      = "CODEPIPELINE"
    buildspec = var.buildspec
  }

  environment {
    compute_type    = "BUILD_GENERAL1_SMALL"
    image           = var.build_env_image
    type            = "LINUX_CONTAINER"
    privileged_mode = true
  }
}
