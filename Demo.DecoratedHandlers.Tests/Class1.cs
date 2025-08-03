//using System;

//namespace Demo.DecoratedHandlers.Tests;

//[AssemblyFixture]
//public class EnvironmentVariableFixture : IDisposable
//{
//    private readonly string _originalValue;

//    public EnvironmentVariableFixture()
//    {
//        // Backup existing value (if any)
//        _originalValue = Environment.GetEnvironmentVariable("MY_ENV_VAR");

//        // Set new value
//        Environment.SetEnvironmentVariable("MY_ENV_VAR", "my_value");
//    }

//    public void Dispose()
//    {
//        // Restore original value
//        Environment.SetEnvironmentVariable("MY_ENV_VAR", _originalValue);
//    }
//}

//[CollectionDefinition("Global Test Collection")]
//public class GlobalTestCollection : ICollectionFixture<EnvironmentVariableFixture>
//{
//    // No code needed here
//}