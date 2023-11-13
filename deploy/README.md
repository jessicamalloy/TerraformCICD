# Deploying the Terraform CICD Service

To deploy the Terraform CICD Service into an AWS account the following variables need to be supplied.  The terraform version should be at least 1.0.4. Once Terraform has deployed the infrastructure the API can be accessed at https://<project_name>.<domain_name>/graphql/

| name | type | description |
| --- | --- | --- |
| aws_account_id | string | AWS account id where service is deployed. |
| docker_username | string | User name for Docker account to use during build. |
| docker_login_token | string | Docker login token for Docker account used in build. |
| domain_name | string | Domain name registered from AWS Route 53 used as basename for the API URL to the deployed service. |
| github_oauth_token | string | OAuth token allowing access to repository. |
| github_branch | string | GitHub branch for Terraform CICD service. |
| azure_ad_tenant_id | string | The tenant (directory) id of the application in Azure. |
| azure_ad_client_id | string | The client (application) id of the application in Azure. |
| azure_ad_audience | string | The audience of the application in Azure. (eg. 'api://<clientId>') |

## Deployment

Note: all commands in this section should be run from the `deploy/` directory.

1. Duplicate the `environments/example` directory to a new directory named for your environment: `environments/<your_env>`
1. Update `environments/<your_env>/variables.tfvars` to set whatever variables you'd like. Any not set in that file will use defaults or prompt for values when you deploy.
1. Run `terraform init` (it's okay if terraform warns about no explicit "local" backend)
1. To deploy, run `terraform apply -var-file="environments/<your_env>/variables.tfvars"`, inspect and approve the planned changes
    * If you get a certificate error during deployment, log in to the console, go to the AWS Certificate Manager console and approve and create routes for any pending certificates, then rerun the terraform apply command

Note:

* Some guides suggest using the `--auto-approve` flag on `terraform apply` commands to skip the manual approval and review step. This guide does not use that flag to prevent copy/paste oopsies.

### Destroying

When you're done with an environment, run the following command: `terraform apply -destroy -var-file="environments/<your_env>/variables.tfvars"`

## ACM Consideration

The ACM certificate created for this service will require a one-time validation. To validate the ACM certificate when creating the service for the first time:

1. Navigate to AWS certificate manager.
2. Click the carrot next to domain name <project_name.domain_name>
3. In status section click carrot next to domain name.
4. Click create record in route 53 button.

## Optional Variables

| name | type | default | description |
| --- | --- | --- | --- |
| project_name | string | `terraform-cicd` | Name of project used for naming all resources. Maximum 41 characters. |
| database_name | string | `terraform_cicd_main` | Name of Database. |
| database_username | string | `terraform_cicd_admin` | Database admin account name. |
| region | string | `us-west-2` | AWS region where service is deployed. |
| azure_ad_domain | string | `portal.azure.com` | The domain of the Azure AD instance. |
| azure_ad_instance | string | `https://login.microsoftonline.com/` | The URI of the Azure AD login. |
| web_app_domain | string | `''` | Web app domain, used for CORS restrictions |
