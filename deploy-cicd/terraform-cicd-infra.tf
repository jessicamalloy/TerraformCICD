
module "terraform_cicd_config_bucket" {
    source = "./cicd-s3-config"
    project_name = var.project_name
    environment = var.environment
    region = var.region
    aws_account_id = var.aws_account_id
    github_access_token = var.github_access_token
    github_oauth_token = var.github_oauth_token
}

module "terraform_cicd_codepipeline" {
  source = "./cicd-codepipeline"
  project_name        = var.project_name
  github_owner        = var.github_owner
  github_oauth_token  = var.github_oauth_token
  github_repo         = local.github_project_name
  github_branch       = var.github_branch
  github_username     = var.github_username
  variable_bucket     = module.terraform_cicd_config_bucket.config_bucket
  backend_config_dynamodb_table = var.dynamodb_table
  backend_config_bucket = var.bucket
  aws_account_id = var.aws_account_id
  service_pipeline = var.service_pipeline

  depends_on = [ module.terraform_cicd_config_bucket ]
}

resource "aws_secretsmanager_secret" "example" {
  name_prefix = "example"
}