// <copyright file="Query.cs" company="Allen Institute">
// Copyright (c) Allen Institute. All rights reserved.
// </copyright>

namespace TerraformCICD.GraphQL;

using AllenInstitute.Platform.Security.Authorization;
using TerraformCICD.GraphQL.Resolvers;
using TerraformCICD.GraphQL.Types;



/// <summary>
/// Configures the GraphQL <see cref="Query"/> type.
/// </summary>
public class Query : ObjectType
{
    /// <summary>
    /// Defines the GraphQL <see cref="Query"/> type which contains all queries on this API.
    /// Method to add and configure queries under the root GraphQL <see cref="Query"/> type.
    /// </summary>
    /// <param name="descriptor">Object used to describe queries added under the GraphQL <see cref="Query"/> type.</param>
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
        descriptor
            .Field("records")
            .Description("Gets a list of Records.")
            .Type<RecordType>()
            .ResolveWith<RecordResolver>(a => a.GetRecords(default!))
            .UseDbContext<TerraformCICDContext>()
            .UsePaging()
            .UseProjection()
            .UseFiltering()
            .UseSorting()
            .Authorize(nameof(AuthorizationPolicy.Read));
    }
}
