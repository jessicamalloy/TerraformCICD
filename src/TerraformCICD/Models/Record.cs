// <copyright file="Record.cs" company="Allen Institute">
// Copyright (c) Allen Institute. All rights reserved.
// </copyright>

namespace TerraformCICD.Models;

using AllenInstitute.Platform.Services.Common.Models;

/// <summary>
/// A test entity.
/// </summary>
public record Record : Entity
{
    /// <summary>
    /// A description :)
    /// </summary>
    public string Description { get; set; } = null!;
}
