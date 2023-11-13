// <copyright file="TestCollection.cs" company="Allen Institute">
// Copyright (c) Allen Institute. All rights reserved.
// </copyright>

namespace TerraformCICDTests.Tests;

using Xunit;

/// <summary>
/// This class has no code, and is never created. Its purpose is simply
/// to be the place to apply [CollectionDefinition] and all the
/// ICollectionFixture interfaces.
/// </summary>
[CollectionDefinition("TerraformCICDTestCollection")]
public class TestCollection : ICollectionFixture<TestFixture>
{
}
