// <copyright file="DeletePayload.cs" company="Allen Institute">
// Copyright (c) Allen Institute. All rights reserved.
// </copyright>

namespace TerraformCICD.GraphQL.Payloads;

/// <summary>
/// Custom delete response payload
/// </summary>
public class DeletePayload
{
    /// <summary>
    /// The record identifier represented as a UUID that has been deleted.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The custom delete message of the <see cref="DeletePayload"/>.
    /// </summary>
    public string Message { get; set; } = null!;
}
