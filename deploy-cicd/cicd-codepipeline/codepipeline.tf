module "plan_code_build" {
  source = "../cicd-codebuild"
  project_name = local.plan-project-name
  buildspec = data.template_file.plan-buildspec.rendered
  backend_config_dynamodb_table = var.backend_config_dynamodb_table
  backend_config_bucket = var.backend_config_bucket
  variable_bucket = var.variable_bucket
  region = var.region
  aws_account_id = var.aws_account_id
}

module "apply_code_build" {
  source = "../cicd-codebuild"
  project_name = local.apply-project-name
  buildspec = data.template_file.apply-buildspec.rendered
  backend_config_dynamodb_table = var.backend_config_dynamodb_table
  backend_config_bucket = var.backend_config_bucket
  variable_bucket = var.variable_bucket
  region = var.region
  aws_account_id = var.aws_account_id
  build_timeout = 15
  s3_bucket_resources = [
    "${module.plan_code_build.project_bucket.arn}",
    "${module.plan_code_build.project_bucket.arn}/*"]
}

data "template_file" "plan-buildspec" {
  template = file(local.plan-buildspec)
}

data "template_file" "apply-buildspec" {
  template = file(local.apply-buildspec)
}

resource "aws_codepipeline" "codepipeline" {
  name     = "${var.project_name}-cicd-pipeline"
  role_arn = aws_iam_role.codepipeline_role.arn

  artifact_store {
    location = module.plan_code_build.project_bucket.bucket
    type     = "S3"
  }

  stage {
    name = "Source"

    action {
      name             = "Source"
      category         = "Source"
      owner            = "ThirdParty"
      provider         = "GitHub"
      version          = "1"
      output_artifacts = ["source_output"]

      configuration = {
        Owner      = var.github_owner
        Repo       = var.github_repo
        Branch     = var.github_branch
        OAuthToken = var.github_oauth_token
      }
    }
  }

  stage {
    name = "Plan"

    action {
      name             = "Build"
      category         = "Build"
      owner            = "AWS"
      provider         = "CodeBuild"
      input_artifacts  = ["source_output"]
      output_artifacts = ["plan_output"]
      version          = "1"

      configuration = {
        ProjectName = local.plan-project-name
        EnvironmentVariables = jsonencode([{
          name = "VARIABLE_BUCKET"
          value = "${var.variable_bucket}"
          type = "PLAINTEXT"
        },
        {
          name = "GITHUB_TOKEN"
          value = "${var.github_oauth_token}"
          type = "PLAINTEXT"
        },
        {
          name = "GITHUB_USERNAME"
          value = "${var.github_username}"
          type = "PLAINTEXT"
        },
        {
          name = "PROJECT_NAME"
          value = "${var.project_name}"
          type = "PLAINTEXT"
        }])
      }
    }
  }

  stage {
    name = "Review"

    action {
      name             = "Review"
      category         = "Approval"
      owner            = "AWS"
      provider         = "Manual"
      version          = "1"

      configuration = {
        CustomData = "Review the Terraform plan"
      }
    }
  }

  stage {
    name = "Apply"

    action {
      name             = "Build"
      category         = "Build"
      owner            = "AWS"
      provider         = "CodeBuild"
      input_artifacts  = ["source_output", "plan_output"]
      version          = "1"

      configuration = {
        ProjectName = local.apply-project-name
        PrimarySource = "source_output"
        EnvironmentVariables = jsonencode([{
          name = "VARIABLE_BUCKET"
          value = "${var.variable_bucket}"
          type = "PLAINTEXT"
        },
        {
          name = "GITHUB_TOKEN"
          value = "${var.github_oauth_token}"
          type = "PLAINTEXT"
        },
        {
          name = "GITHUB_USERNAME"
          value = "${var.github_username}"
          type = "PLAINTEXT"
        },
        {
          name = "PROJECT_NAME"
          value = "${var.project_name}"
          type = "PLAINTEXT"
        },
        {
          name = "SERVICE_PIPELINE"
          value = "${var.service_pipeline}"
          type = "PLAINTEXT"
        }])
      }
    }
  }
}
