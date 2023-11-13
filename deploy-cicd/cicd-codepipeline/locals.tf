locals {
  plan-buildspec              = "${path.module}/plan-buildspec.yml"
  apply-buildspec             = "${path.module}/apply-buildspec.yml"
  plan-project-name            = "${var.project_name}-tfplan"
  apply-project-name            = "${var.project_name}-tfapply"
}