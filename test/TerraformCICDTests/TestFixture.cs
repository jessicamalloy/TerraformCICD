// <copyright file="TestFixture.cs" company="Allen Institute">
// Copyright (c) Allen Institute. All rights reserved.
// </copyright>

namespace TerraformCICDTests;

using System.Collections.Generic;
using AllenInstitute.Platform.Security.SecretsManager;
using AllenInstitute.Platform.TestUtilities;
using Amazon;
using Amazon.SecretsManager;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Test fixture to only be run once per test class.
/// </summary>
public class TestFixture : PlatformTestFixture<Program>
{
    private readonly EFPostgresDockerSetup postgresDocker;

    /// <summary>
    /// The database context.
    /// </summary>
    public readonly TerraformCICDContext DbContext;

    /// <summary>
    /// Constructor for initializing the <see cref="TestFixture"/> class.
    /// </summary>
    public TestFixture()
        : base()
    {
        var contextFactory = Server.Services.GetService<IDbContextFactory<TerraformCICDContext>>()!;

        DbContext = contextFactory.CreateDbContext();
        postgresDocker = new EFPostgresDockerSetup(DbContext);
    }

    /// <inheritdoc />
    public override void ConfigureWebHostEnvironment(IWebHostBuilder builder)
    {
        base.ConfigureWebHostEnvironment(builder);

        var config = new AmazonSecretsManagerConfig()
        {
            RegionEndpoint = RegionEndpoint.USWest2,
            ServiceURL = "http://localhost:4566",
        };

        builder.ConfigureTestServices(services =>
        {
            // Override the secrets manager config set in Program.cs
            services.AddSingleton(new AwsSecretsManager(config));
        });
    }

    /// <inheritdoc />
    public async override Task InitializeTestFixtureAsync()
    {
        await postgresDocker.InitializeAsync();
    }

    /// <inheritdoc />
    public override async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await postgresDocker.DisposeAsync();
        await base.DisposeAsync();
    }

    private void AddRangeAndSave(IEnumerable<object> entities)
    {
        DbContext.AddRange(entities);
        DbContext.SaveChanges("Test System");
    }
}
