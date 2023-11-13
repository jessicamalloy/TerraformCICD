module "terraform_cicd_vpc" {
  source = "github.com/AllenInstitute/platform-terraform-modules//vpc?ref=v1.0.0"
  project_name       = var.project_name
  create_private_subnets = false
}

module "terraform_cicd_db" {
  source = "github.com/AllenInstitute/platform-terraform-modules//rds?ref=v1.0.0"
  project_name       = var.project_name
  database_name      = var.database_name
  database_username  = var.database_username
  vpc_id             = module.terraform_cicd_vpc.id
  vpc_public_subnets = module.terraform_cicd_vpc.public_subnets
  region             = var.region
}

module "terraform_cicd_ecs" {
  source = "github.com/AllenInstitute/platform-terraform-modules//ecs?ref=v1.0.0"
  project_name       = var.project_name
  domain_name        = var.domain_name
  aws_account_id     = var.aws_account_id
  region             = var.region
  vpc_id             = module.terraform_cicd_vpc.id
  vpc_public_subnets = module.terraform_cicd_vpc.public_subnets

  env_vars = [
    { 
      "name": "DB_SECRET", 
      "value": "${module.terraform_cicd_db.db_secret_id}"
    },
    {
      name  = "AzureAd__Instance"
      value = "${var.azure_ad_instance}"
    },
    {
      name  = "AzureAd__Domain"
      value = "${var.azure_ad_domain}"
    },
    {
      name  = "AzureAd__TenantId"
      value = "${var.azure_ad_tenant_id}"
    },
    {
      name  = "AzureAd__ClientId"
      value = "${var.azure_ad_client_id}"
    },
    {
      name  = "AzureAd__Audience"
      value = "${var.azure_ad_audience}"
    },
    {
      name  = "WebAppDomain"
      value = "${var.web_app_domain}"
    }
  ]
}

module "terraform_cicd_codepipeline" {
  source = "github.com/AllenInstitute/platform-terraform-modules//ecs-codepipeline?ref=v1.0.0"
  project_name        = var.project_name
  aws_account_id      = var.aws_account_id
  region              = var.region
  ecs_cluster         = module.terraform_cicd_ecs.ecs_cluster
  ecs_service         = module.terraform_cicd_ecs.ecs_service
  ecs_container       = module.terraform_cicd_ecs.ecs_container
  github_oauth_token  = var.github_oauth_token
  github_repo         = local.github_project_name
  github_branch       = var.github_branch
  github_access_token = var.github_access_token
  github_username     = var.github_username
  docker_username     = var.docker_username
  docker_login_token  = var.docker_login_token
  db_secret           = module.terraform_cicd_db.db_secret_id
  project_directory   = local.github_project_name
  
  depends_on = [ 
    module.terraform_cicd_db 
  ]
}
