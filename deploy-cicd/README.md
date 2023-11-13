# Terraform CICD Demo

This service provides an example of how an automated Terraform deployment can be set up and run in AWS. It consists of two sets of Terraform files:

- **Service Terraform:** This works the same as all of our other services' pipelines, where it builds the service and deploys it. The Terraform consists of the database, VPC, etc. This Terraform exists in the `deploy/` directory and can be deployed independently, similar to our other services.
- **CICD Terraform:** These files create a pipeline on AWS that runs the terraform commands - `init`, `plan`, and `apply` - that deploy the service Terraform. The CICD Terraform exists in the `deploy-cicd` directory.

This README will walk through how to deploy the pipeline and the major components of the architecture.

## Deployment

To deploy the Terraform CICD Service into an AWS account the following variables need to be supplied.  The terraform version should be at least 1.0.4. Once Terraform has deployed the infrastructure the API can be accessed at https://<project_name>.<domain_name>/graphql/

| name | type | description |
| --- | --- | --- |
| aws_account_id | string | AWS account id where service is deployed. |
| github_username | string | Username for the Github account used to deploy |
| github_branch | string | GitHub branch for Terraform service pipeline. |
| github_owner | string | Owner of the git repo (defaults to AllenInstitute) |
| github_oauth_token | string | OAuth token allowing access to repository. |
| github_access_token | string | Token allowing package access. |
| project_name | string | Name of the deployed project |
| environment | string | The environment folder being deployed in (used for finding terraform files) |
| service_pipeline | string | Name of the pipeline being created by *this* pipeline. Should be "<deploy/variables.tfvars project_name>-pipeline" |


Note: all commands in this section should be run from the `deploy-cicd/` directory.

1. Duplicate the `environments/example` directory to a new directory named for your environment: `environments/<your_env>`
1. Update `environments/<your_env>/variables.tfvars` to set whatever variables you'd like. Any not set in that file will use defaults or prompt for values when you deploy. Make sure to set the `environment` variable to `<your_env>` here.
1. Run `terraform init -backend-config="environments/<your_env>/backend.config"`
1. To deploy, run `terraform apply -var-file="environments/<your_env>/variables.tfvars" -var-file="environments/<your_env>/backend.config"`. The `backend.config` file is treated like a second variable file here because the pipeline needs to know what S3 bucket and DynamoDB table you're using for managing state. It's assumed that these values are the same for both pipelines, but if that's not the case then sub in `-var-file="../deploy/environments/<your_env>/backend.config"` for the second var file.
1. Make sure to inspect and approve the planned changes. Running this pipeline should work like following the equivalent steps in the `deploy/` readme.


### Destroying

When you're done with an environment, run the following command: `terraform apply -destroy -var-file="environments/<your_env>/variables.tfvars -var-file="environments/<your_env>/backend.config"`


## Major components

The `deploy-cicd` Terraform has 2 major components:

- **s3:** When the Terraform is applied, the local `backend.config` and `variables.tfvars` files for *both* Terraform deploys are uploaded to a config files bucket. The `deploy/` config files are used within the pipeline to run Terraform commands. The `deploy-cicd` files are never used - they are uploaded to be used as a reference for what config values were used for the current version deployed.
- **codepipeline:** The codepipeline is set to trigger when a change is pushed to `github_branch` specified. It runs the `terraform plan` command and stores the output in a plan-artifacts bucket. Examine the plan output from the plan step (click 'View logs'), then approve if it's as expected in the manual review step. After, the apply step will run `terraform apply`, then trigger the potentially-newly-created or updated pipeline.


## Troubleshooting and things to note

- This demo uses a copied-over `platform-terraform-modules@v1.0.0`, with 2 differences:
  - the `ecs-codepipeline-core` pipeline has source config updated to include this flag: `PollForSourceChanges = false`. This is so that the pipeline gets triggered by the cicd pipeline, rather than changes in the repo. Updating the source block for the codepipelines that use CodeStar Connections so that they don't pull automatically from the repo might be different.
  - `ecs-codepipeline` module passes `github_owner` to `ecs-codepipeline-core`, since this repo isn't owned by the Allen Institute (this shouldn't matter for updating Platform team repos)



