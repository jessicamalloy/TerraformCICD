# TerraformCICD

[![build](https://github.com/AllenInstitute/TerraformCICD/actions/workflows/dotnet.yml/badge.svg)](https://github.com/AllenInstitute/TerraformCICD/actions/workflows/dotnet.yml)

<!-- Add a useful description here. -->

---

## Table of Contents

1. [Documentation](./docs)
1. [Architecture](./docs/architecture/README.md)
1. [Source Code](./src)

---

# Development

## Prerequisites

* [.NET 6](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-6)
* [EF Core 6](https://docs.microsoft.com/en-us/ef/core/)

## Install .NET tools

The following installs/restores the necessary tools for the Terraform CICD Service

```bash
$ dotnet tool restore
```

## Install .NET Packages

The following installs/restores the necessary packages for the Terraform CICD Service

```bash
$ dotnet restore
```

## Lint

To lint the project and check for code style errors, run (remove `--verify-no-changes` to automatically fix issues):

```bash
$ dotnet format --severity warn --verify-no-changes
```

## Build

To build the project and check for build errors, run:

```bash
$ dotnet build
```

## Run Tests

To run tests, run:

```bash
$ dotnet test --logger "console;verbosity=normal"
```

## Manage Database State

To create a new migration, run:

```bash
$ dotnet ef migrations add <MigrationName> --project src/TerraformCICD
```

To create apply migration files

```bash
$ dotnet ef database update --project src/TerraformCICD
```

## Run Locally

To run locally

```bash
$ dotnet run --project src/TerraformCICD
```

## Set Environment Locally

> :notebook: These steps are not required for setting the environment if the `ASPNETCORE_ENVIRONMENT` environment variable
> has bet configured in `launchSettings.json`.

The environment can be configured by setting the `ASPNETCORE_ENVIRONMENT` environment variable to one of these options:

* Production
* Staging
* Development

When using the environment variable, set the `--no-launch-profile` flag as seen in this example:

```bash
$ dotnet run --no-launch-profile
```

## Running in AWS or with Azure AD

* In production mode the Azure AD auth is enabled.
* In development mode using the Azure AD auth is enabled by default. It can be disabled by using an opt-in in-memory
authentication service, see the [in-memory authentication](https://github.com/AllenInstitute/Platform.Security/tree/main/src/Authentication/InMemory#in-memory-authentication) section for more information.

To support Azure AD auth, the following environment or configuration variables _must_ be set to match the applications
Azure AD (AAD) app via the Azure Portal:

> :warning: The environment or configuration variable values should NOT be checked into the GitHub repository.

|Environment Variable|Description|
|:---|:---|
|`WebAppDomain`|The domain name of the web app, e.g., [`http://localhost:5000`](http://localhost:5000). This is used for CORS.|
|`DB_SECRET`|The AWS secret ID for database credentials.|
|`AzureAd__Audience`|The Application ID URI from the services app registration, e.g., `api://<CLIENT_ID>`|
|`AzureAd__ClientId`|Application (client) ID that comes from the services app registration.|
|`AzureAd__TenantId`|Directory (tenant) ID that comes from the services app registration.|
|`AzureAd__Instance`|[`https://login.microsoftonline.com/`](https://login.microsoftonline.com/)|
|`AzureAd__Domain`|[`portal.azure.com`](portal.azure.com)|

## Obtaining an access token

If you are wanting to test the APIs authorization against a real identity service such as Azure AD (AAD) then follow
these steps to obtain an `access token`.

**Azure AD**

1. Install the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
1. Login using the following example command:
     `az login --tenant <TENANT_URL> --allow-no-subscriptions`
1. Now to obtain the access token use the following example command:
     `az account get-access-token --resource api://<CLIENT_ID>`

## GraphQL Schema

To view the GraphQL Schema (SDL), visit:
    https://localhost:5001/graphql?sdl

## GraphQL Playground

To interact with the GraphQL Playground, visit:
    https://localhost:5001/graphql
