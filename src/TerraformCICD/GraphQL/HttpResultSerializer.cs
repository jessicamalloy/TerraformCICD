// <copyright file="HttpResultSerializer.cs" company="Allen Institute">
// Copyright (c) Allen Institute. All rights reserved.
// </copyright>

namespace TerraformCICD.GraphQL;

using HotChocolate.AspNetCore.Serialization;
using HotChocolate.Execution;

/// <summary>
/// Configures a custom http result serialize of <see cref="HttpResultSerializer"/> type.
/// </summary>
public class HttpResultSerializer : DefaultHttpResultSerializer
{
    /// <inheritdoc/>
    public override HttpStatusCode GetStatusCode(IExecutionResult result)
    {
        if (result is IQueryResult queryResult &&
            queryResult.Errors?.Any(error => error.Code == HttpStatusCode.BadRequest.ToString()) == true
        )
        {
            return HttpStatusCode.BadRequest;
        }

        return base.GetStatusCode(result);
    }
}
