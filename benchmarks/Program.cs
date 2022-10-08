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
                CreateJob(CoreRuntime.Core70),
                CreateJob(CoreRuntime.Core60),
                CreateJob(CoreRuntime.Core50),
                CreateJob(CoreRuntime.Core31),
                CreateJob(ClrRuntime.Net48)
            );
        }

        private static Job CreateJob(Runtime runtime)
            => Job.Default.WithRuntime(runtime);
    }
}