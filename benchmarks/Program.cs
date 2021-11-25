﻿using System;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace NetworkPrimitives.Benchmarks
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var path = "../../../../BenchmarkResults";
            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, path));
            Directory.CreateDirectory(path);
            Environment.CurrentDirectory = path;
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)?.RunAll();
        }
    }
}