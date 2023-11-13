// <copyright file="RecordInput.cs" company="Allen Institute">
// Copyright (c) Allen Institute. All rights reserved.
// </copyright>

namespace TerraformCICD.GraphQL.Inputs;

using AllenInstitute.Platform.Services.Common.GraphQL.Inputs;
using HotChocolate.Types;
using TerraformCICD.Models;

/// <summary>
/// Configures the GraphQL <see cref="RecordInput"/>.
/// </summary>
public class RecordInput : AddEntityInput<Record>
{
    /// <summary>
    /// Defines the GraphQL <see cref="Record"/> type which contains all the fields for this object.
    /// Method to add and configure fields under the GraphQL <see cref="Record"/> type.
    /// </summary>
    /// <param name="descriptor">Object used to describe fields added under the GraphQL <see cref="Record"/> type.</param>
    protected override void ConfigureInput(IInputObjectTypeDescriptor<Record> descriptor)
    {
        descriptor
            .Name("RecordInput")
            .Description("Meaningless temp entity for testing.");

        descriptor
            .Field(d => d.Description)
            .Description("Text to describe the record.")
            .Type<StringType>();
    }
}
