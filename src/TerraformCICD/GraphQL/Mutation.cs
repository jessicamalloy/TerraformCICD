// <copyright file="Mutation.cs" company="Allen Institute">
// Copyright (c) Allen Institute. All rights reserved.
// </copyright>

namespace TerraformCICD.GraphQL;

using AllenInstitute.Platform.Security.Authorization;
using HotChocolate.Types;
using TerraformCICD.GraphQL.Inputs;
using TerraformCICD.GraphQL.Resolvers;

/// <summary>
/// Configures the GraphQL <see cref="Mutation"/> type.
/// </summary>
public class Mutation : ObjectType
{
    /// <summary>
    /// Defines the GraphQL <see cref="Mutation"/> type which contains all mutations on this API.
    /// Method to add and configure mutations under the root GraphQL <see cref="Mutation"/> type.
    /// </summary>
    /// <param name="descriptor">Object used to describe mutations added under the GraphQL <see cref="Mutation"/> type.</param>
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
        descriptor
            .Field("createRecords")
            .Argument("input", input =>
            {
                input
                    .Type<NonNullType<RecordInput>>()
                    .Description("The input values to be saved to the record.");
            })
            .Description("Creates `Record`s and returns the newly created records.")
            .ResolveWith<RecordResolver>(a => a.CreateRecord(default!, default!))
            .UseDbContext<TerraformCICDContext>()
            .UseProjection()
            .Authorize(nameof(AuthorizationPolicy.ReadWrite));
    }
}
