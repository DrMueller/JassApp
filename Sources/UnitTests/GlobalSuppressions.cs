using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP006:Implement IDisposable", Justification = "Just Tests")]
[assembly: SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP003:Dispose previous before re-assigning", Justification = "Just Tests")]
[assembly: SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created", Justification = "Just Tests")]
[assembly: SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "Tests are more readable without async suffix")]
