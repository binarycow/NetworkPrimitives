using System;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace NetworkPrimitives.Benchmarks
{
    internal static class Program
    {
        private static void Main()
        {
            var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "../../../../BenchmarkResults"));
            Directory.CreateDirectory(path);
            Environment.CurrentDirectory = path;


             _ = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
                     ?.RunAll() ?? Enumerable.Empty<Summary>();
        }
    }
}