using System;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;

namespace NetworkPrimitives.Benchmarks
{
    internal static class Program
    {
        internal const int LAUNCH_COUNT = 1;
        internal const int WARMUP_COUNT = 0;
        internal const int TARGET_COUNT = 1;

        private static void Main(string[] args)
        {
            var path = "../../../../BenchmarkResults";
            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, path));
            Directory.CreateDirectory(path);
            Environment.CurrentDirectory = path;
            Console.WriteLine("===========================");
            Console.WriteLine("===========================");
            Console.WriteLine($"Output Path: {path}");
            Console.WriteLine("===========================");
            Console.WriteLine("===========================");
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)?.RunAll(new Config());
        }
    }
    
    public class Config : ManualConfig
    {
        public Config()
        {
            this.AddColumnProvider(DefaultColumnProviders.Instance!);
            this.AddExporter(
                MarkdownExporter.GitHub!
                , CsvMeasurementsExporter.Default!
                , RPlotExporter.Default!
            );
            this.AddDiagnoser(MemoryDiagnoser.Default!);
            this.AddLogger(ConsoleLogger.Default!);
            this.AddJob(
                Job.Dry!
                    .WithWarmupCount(Program.WARMUP_COUNT)!
                    .WithLaunchCount(Program.LAUNCH_COUNT)!
                    .WithRuntime(CoreRuntime.Core60!)!
                .WithRuntime(CoreRuntime.Core50!)!
                .WithRuntime(CoreRuntime.Core30!)!
                .WithRuntime(ClrRuntime.Net48!)!
            );
        }
    }
}