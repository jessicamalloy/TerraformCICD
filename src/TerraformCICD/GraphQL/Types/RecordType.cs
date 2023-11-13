// <copyright file="RecordType.cs" company="Allen Institute">
// Copyright (c) Allen Institute. All rights reserved.
// </copyright>

namespace TerraformCICD.GraphQL.Types;

using AllenInstitute.Platform.Services.Common.GraphQL.Types;
using HotChocolate.Types;
using TerraformCICD.Models;

/// <summary>
/// Configures the GraphQL <see cref="RecordType"/>.
/// </summary>
public class RecordType : EntityType<Record>
{
    /// <summary>
    /// Defines the GraphQL <see cref="Record"/> type which contains all the fields for this object.
    /// Method to add and configure fields under the GraphQL <see cref="Record"/> type.
    /// </summary>
    /// <param name="descriptor">Object used to describe fields added under the GraphQL <see cref="Record"/> type.</param>
    protected override void ConfigureType(IObjectTypeDescriptor<Record> descriptor)
    {
        descriptor
            .Description("Meaningless entity for testing.");

        descriptor
            .Field(d => d.Description)
            .Description("It's a description.")
            .Type<StringType>();
    }
}
