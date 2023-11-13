// <copyright file="RecordResolver.cs" company="Allen Institute">
// Copyright (c) Allen Institute. All rights reserved.
// </copyright>

namespace TerraformCICD.GraphQL.Resolvers;

using System.Collections.Generic;
using AllenInstitute.Platform.Services.Common.Extensions;
using HotChocolate;
using TerraformCICD.Models;
using TerraformCICD.Repositories;

/// <summary>
/// Sets up the <see cref="Record"/> resolvers.
/// </summary>
public class RecordResolver
{
    /// <summary>
    /// Resolves query for GraphQl <see cref="Record"/>.
    /// </summary>
    /// <param name="dbContext">Database context.</param>
    /// <returns>Returns a list of <see cref="Record"/> records.</returns>
    public IQueryable<Record> GetRecords([ScopedService] TerraformCICDContext dbContext)
    {
        return dbContext.Record.GetAll();
    }

    /// <summary>
    /// Creates <see cref="Record"/>s and returns the newly created records.
    /// </summary>
    /// <param name="dbContext">Database context.</param>
    /// <param name="input">The input values to be saved to the record.</param>
    /// <returns>Returns a created <see cref="Record"/>s.</returns>
    public Record CreateRecord([ScopedService] TerraformCICDContext dbContext, Record input)
    {
        var saved = dbContext.Record.Create(input);
        SaveRecord(dbContext);

        return dbContext.Record.FindById(saved.Id);
    }

    /// <summary>
    /// Saves the <see cref="Record"/> entity.
    /// </summary>
    /// <param name="dbContext">Database context.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the <see cref="Record"/> entity is not found.</exception>
    private static void SaveRecord(TerraformCICDContext dbContext)
    {
        try
        {
            dbContext.SaveChanges();
        }
        catch (DbUpdateConcurrencyException duce)
        {
            var recordIds = duce.Entries.Select(p => ((Record)p.Entity).Id);
            throw new KeyNotFoundException(
                $"Could not find {nameof(Record)} with IDs: [{string.Join(',', recordIds)}]",
                duce);
        }
    }
}
