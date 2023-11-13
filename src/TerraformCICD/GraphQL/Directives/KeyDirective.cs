// <copyright file="KeyDirective.cs" company="Allen Institute">
// Copyright (c) Allen Institute. All rights reserved.
// </copyright>

namespace TerraformCICD.GraphQL.Directives;

/// <summary>
/// Configures the GraphQL <see cref="KeyDirective"/>.
/// </summary>
public class KeyDirective : DirectiveType
{
    /// <summary>
    /// Defines the GraphQL <see cref="KeyDirective"/> type which annotates fields that are keys.
    /// Method to add and configure the directive under the GraphQL <see cref="KeyDirective"/> type.
    /// </summary>
    /// <param name="descriptor">Object used to describe fields added under the GraphQL <see cref="KeyDirective"/> type.</param>
    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        descriptor.Name("key");
        descriptor.Description("Annotates a field as being a `key`.");
        descriptor.Location(DirectiveLocation.FieldDefinition);
    }
}
