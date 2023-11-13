// <copyright file="Program.cs" company="Allen Institute">
// Copyright (c) Allen Institute. All rights reserved.
// </copyright>

using AllenInstitute.Platform.Security.Authentication;
using AllenInstitute.Platform.Security.Authorization;
using AllenInstitute.Platform.Security.Exceptions;
using AllenInstitute.Platform.Security.SecretsManager;
using AllenInstitute.Platform.Services.Common.GraphQL.Filters;
using AllenInstitute.Platform.Services.Common.GraphQL.Logging;
using HotChocolate.Types.Pagination;


var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var config = builder.Configuration;
var services = builder.Services;

services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        if (env.IsProduction())
        {
            // Get the WebAppDomain name from the config/environment variable.
            // It will be used by the CORS policy.
            var webAppDomain = config.GetValue<string>("WebAppDomain");

            if (webAppDomain == null)
            {
                throw new InvalidConfigurationException("The WebAppDomain environment or configuration variable must be set for CORS.");
            }

            builder.WithOrigins(webAppDomain);
        }
        else
        {
            builder.AllowAnyOrigin();
        }

        builder
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

services.AddControllers();
services.AddHealthChecks();
services.AddHttpContextAccessor();
services.AddHttpResultSerializer<HttpResultSerializer>();

services.AddPlatformAuthentication(config);
services.AddPlatformAuthorization();

services.AddSingleton<AwsSecretsManager>();
services.AddSingleton<PostgresSecretsManager>();

services.AddDbContextFactory<TerraformCICDContext>();

services
    .AddGraphQLServer()
    .AddDiagnosticEventListener<GraphQLEventListener>()
    .AddAuthorization()
    .AddProjections()
    .AddFiltering()
    .SetPagingOptions(new PagingOptions
    {
        IncludeTotalCount = true,
    })
    .ModifyRequestOptions(options =>
    {
        options.IncludeExceptionDetails = env.IsDevelopment();
    })
    .ModifyOptions(options =>
    {
        options.EnableOneOf = true;
    })
    .AddSorting()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddErrorFilter<ErrorFilter>();

using var app = builder.Build();

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks("/health");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGraphQL();
});

app.Run();

public partial class Program
{
}
