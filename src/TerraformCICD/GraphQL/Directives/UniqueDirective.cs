// <copyright file="UniqueDirective.cs" company="Allen Institute">
// Copyright (c) Allen Institute. All rights reserved.
// </copyright>

namespace TerraformCICD.GraphQL.Directives;

/// <summary>
/// Configures the GraphQL <see cref="UniqueDirective"/>.
/// </summary>
public class UniqueDirective : DirectiveType
{
    /// <summary>
    /// Defines the GraphQL <see cref="UniqueDirective"/> type which annotates fields that are unique.
    /// Method to add and configure the directive under the GraphQL <see cref="UniqueDirective"/> type.
    /// </summary>
    /// <param name="descriptor">Object used to describe fields added under the GraphQL <see cref="UniqueDirective"/> type.</param>
    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        descriptor.Name("unique");
        descriptor.Description("Annotates a field as being `unique`.");
        descriptor.Location(DirectiveLocation.FieldDefinition);
    }
}
