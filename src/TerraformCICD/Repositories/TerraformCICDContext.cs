// <copyright file="TerraformCICDContext.cs" company="Allen Institute">
// Copyright (c) Allen Institute. All rights reserved.
// </copyright>

namespace TerraformCICD.Repositories;

using AllenInstitute.Platform.Security.SecretsManager;
using AllenInstitute.Platform.Services.Common.Models;
using EntityFramework.Exceptions.PostgreSQL;
using TerraformCICD.Models;

/// <summary>
/// Creates a bridge for <see cref="TerraformCICDContext"/> between the entity classes and the database.
/// </summary>
public class TerraformCICDContext : DbContext
{
    private readonly HttpContext httpContext;
    private readonly PostgresSecretsManager dbSecretsManager;

    /// <summary>
    /// The current user of the context.
    /// </summary>
    protected string CurrentUser => httpContext.User.Identity!.Name!;

    /// <summary>
    /// Gets or sets the database set for <see cref="Record"/>.
    /// </summary>
    /// <value>The <see cref="Record"/> model.</value>
    public DbSet<Record> Record => Set<Record>();

    /// <summary>
    /// Initializes static members of the <see cref="TerraformCICDContext"/> class.
    /// </summary>
    static TerraformCICDContext()
    {
    }

    /// <summary>
    /// Initializes the database context.
    /// </summary>
    /// <param name="options">Options to be used by <see cref="DbContext"/>.</param>
    /// <param name="httpContextAccessor">Http context accessor to get the current user.</param>
    /// <param name="dbSecretsManager">Database secrets manager to get the connection string.</param>
    public TerraformCICDContext(
        DbContextOptions<TerraformCICDContext> options,
        IHttpContextAccessor httpContextAccessor,
        PostgresSecretsManager dbSecretsManager
    ) : base(options)
    {
        httpContext = httpContextAccessor.HttpContext!;
        this.dbSecretsManager = dbSecretsManager;
    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options
            .UseNpgsql(dbSecretsManager.DbConnectionString().Result, npgsqlOptions =>
            {
                npgsqlOptions
                    .EnableRetryOnFailure()
                    .ExecutionStrategy(_ => new CredentialsRotationExecutionStrategy(this, dbSecretsManager));
            })
            .UseSnakeCaseNamingConvention()
            .UseExceptionProcessor();
    }

    /// <summary>
    /// Helps create entities in the database with Entity Framework
    /// </summary>
    /// <param name="modelBuilder">The builder for creating models in the database.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Overrides the SaveChanges method to add audit tracking information.
    /// Uses CurrentUser defined in the HttpContext.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    public override int SaveChanges()
    {
        UpdateAuditInfo(DateTime.UtcNow, CurrentUser);
        return base.SaveChanges();
    }

    /// <summary>
    /// Wraps the SaveChanges method to allow a current user to be passed in.
    /// </summary>
    /// <param name="currentUser">User that is saving the entities</param>
    /// <returns></returns>
    public int SaveChanges(string currentUser)
    {
        UpdateAuditInfo(DateTime.UtcNow, currentUser);
        return base.SaveChanges();
    }

    private void UpdateAuditInfo(DateTime updateTime, string currentUser)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is Dictionary<string, object>)
                continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    ((Entity)entry.Entity).CreatedAt = updateTime;
                    ((Entity)entry.Entity).CreatedBy = currentUser;
                    break;
                case EntityState.Modified:
                    entry.Property("CreatedAt").IsModified = false;
                    entry.Property("CreatedBy").IsModified = false;
                    ((Entity)entry.Entity).UpdatedAt = updateTime;
                    ((Entity)entry.Entity).UpdatedBy = currentUser;
                    break;
            }
        }
    }
}
