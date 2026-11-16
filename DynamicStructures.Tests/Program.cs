using DynamicStructures.Tests;

RegressionChecks.Run();
ConsoleChecks.Run();
Console.WriteLine($"\nPassed: {Check.Passed}; failed: {Check.Failed}; assertions: {Check.Assertions}");
return Check.Failed == 0 ? 0 : 1;
