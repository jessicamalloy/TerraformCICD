// <copyright file="CredentialsRotationExecutionStrategy.cs" company="Allen Institute">
// Copyright (c) Allen Institute. All rights reserved.
// </copyright>

namespace TerraformCICD.Repositories;

using System.Net.Sockets;
using AllenInstitute.Platform.Security.SecretsManager;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;

/// <summary>
/// Execution strategy to determine when to retry to connect to the database.
/// </summary>
public class CredentialsRotationExecutionStrategy : ExecutionStrategy
{
    private readonly PostgresSecretsManager dbSecretsManager;

    /// <summary>
    /// Creates an instance of <see cref="CredentialsRotationExecutionStrategy"/>.
    /// </summary>
    /// <param name="context">The context on which the operations will be invoked.</param>
    /// <param name="dbSecretsManager">Secrets manager for the database.</param>
    /// <param name="maxRetryCount">The maximum number of retry attempts.</param>
    /// <param name="maxRetryDelay">The maximum delay between retries.</param>
    public CredentialsRotationExecutionStrategy(
        DbContext context,
        PostgresSecretsManager dbSecretsManager,
        int? maxRetryCount = null,
        TimeSpan? maxRetryDelay = null
    )
        : base(context, maxRetryCount ?? DefaultMaxRetryCount, maxRetryDelay ?? DefaultMaxDelay)
    {
        this.dbSecretsManager = dbSecretsManager;
    }

    /// <summary>
    /// When retrying, set the connection string to the new value from the secrets manager.
    /// </summary>
    protected override void OnRetry()
    {
        var connectionString = dbSecretsManager.DbConnectionString().Result;
        Dependencies.CurrentContext.Context.Database.SetConnectionString(connectionString);
        base.OnRetry();
    }

    /// <summary>
    /// Determines whether the specified exception represents a connection or login failure with PostgreSQL and can be retried.
    /// If it can be retried, the credentials cache will be updated.
    /// </summary>
    /// <param name="exception">The exception object to be verified.</param>
    /// <returns>True if the specified exception is due to a connection or login failure with a PostgreSQL database, otherwise false.</returns>
    protected override bool ShouldRetryOn(Exception exception)
    {
        if (exception is not NpgsqlException pgException)
        {
            return false;
        }

        var errorCode = pgException.SqlState;

        if (errorCode == null)
        {
            if (IsRetryableSocketException(pgException) || IsRetryableTimeoutException(pgException))
            {
                return dbSecretsManager.RefreshNowAsync().Result;
            }

            return false;
        }

        if (IsRetryablePostgresError(errorCode))
        {
            return dbSecretsManager.RefreshNowAsync().Result;
        }

        return false;
    }

    private static bool IsRetryablePostgresError(string code)
    {
        return code == PostgresErrorCodes.InvalidPassword ||
            code == PostgresErrorCodes.ConnectionFailure ||
            code == PostgresErrorCodes.ConnectionDoesNotExist ||
            code == PostgresErrorCodes.ConnectionException ||
            code == PostgresErrorCodes.InvalidCatalogName; // Database does not exist
    }

    private static bool IsRetryableSocketException(NpgsqlException exception)
    {
        var code = (exception.InnerException as SocketException)?.SocketErrorCode;

        return code == SocketError.ConnectionRefused ||
            code == SocketError.AddressNotAvailable;
    }

    private static bool IsRetryableTimeoutException(NpgsqlException exception)
    {
        return exception.InnerException is TimeoutException;
    }
}
