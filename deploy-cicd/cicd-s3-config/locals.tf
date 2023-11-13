locals {
  github-actions-tfplan              = <<EOF
name: terraform plan

on:
  # Run the build for all pull requests
  pull_request:
    branches:
        - main # change for staging, prod, whatever
    paths:
        - deploy/**

permissions:
    contents: read
    pull-requests: write # to be able to comment terraform plan
    id-token: write # AWS auth

jobs:
    plan:
        name: Terraform
        runs-on: ubuntu-latest
        defaults:
            run:
                working-directory: ./deploy

        steps:
            - name: Checkout
              uses: actions/checkout@v3

            - name: Configure AWS Credentials
              uses: aws-actions/configure-aws-credentials@v2
              with:
                  role-to-assume: arn:aws:iam::${var.aws_account_id}:role/GitHubAction-AssumeRoleWithAction
                  aws-region: ${var.region}

            - name: Setup Terraform
              uses: hashicorp/setup-terraform@v2
              with:
                terraform_wrapper: false

            - name: Terraform Init
              id: init
              run: |
                aws s3 cp --recursive s3://tfpoc-demo-terraform-config-files/service .
                git config --global url."https://git:${var.github_access_token}@github.com/AllenInstitute".insteadOf "https://github.com/AllenInstitute"
                git config --global url."https://${var.github_access_token}:x-oauth-basic@github.com/AllenInstitute".insteadOf ssh://git@github.com/AllenInstitute
                terraform init --backend-config="backend.config"
              shell: sh

            - name: Terraform Plan
              id: plan
              run: terraform plan --var-file="variables.tfvars"

            - name: Terraform Show
              id: show
              run: terraform show -no-color tf.plan 2>&1 > /tmp/plan.txt # writes to local file for pr comment

            - uses: actions/github-script@v6
              if: github.event_name == 'pull_request'
              with:
                github-token: ${var.github_oauth_token}
                script: |
                  const fs = require("fs");
                  const plan = fs.readFileSync("/tmp/plan.txt", "utf8");
                  const maxGitHubBodyCharacters = 65536;
          
                  function chunkSubstr(str, size) {
                    const numChunks = Math.ceil(str.length / size)
                    const chunks = new Array(numChunks)
                    for (let i = 0, o = 0; i < numChunks; ++i, o += size) {
                      chunks[i] = str.substr(o, size)
                    }
                    return chunks
                  }
          
                  // Split the Terraform plan into chunks if it's too big and can't fit into the GitHub Action
                  var plans = chunkSubstr(plan, maxGitHubBodyCharacters); 
                  for (let i = 0; i < plans.length; i++) {
                    const output = `### Terraform CICD Plan Part # $${i + 1}
                    #### Terraform Initialization ⚙️\`$${{ steps.init.outcome }}\`
                    #### Terraform Plan 📖\`$${{ steps.plan.outcome }}\`
                    <details><summary>Show Plan</summary>
                    \`\`\`\n
                    $${plans[i]}
                    \`\`\`
                    </details>
                    *Pusher: @$${{ github.actor }}, Action: \`$${{ github.event_name }}\`, Workflow: \`$${{ github.workflow }}\`*`;   
          
                    await github.rest.issues.createComment({
                      issue_number: context.issue.number,
                      owner: context.repo.owner,
                      repo: context.repo.repo,
                      body: output
                    })
                  }

EOF
}