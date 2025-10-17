using gabriel_lera_employees.Services;
using gabriel_lera_employees.Utils;
using Microsoft.Extensions.Logging;

namespace gabriel_lera_employees
{
    class Program
    {
        static int Main(string[] args)
        {
            var input = args.Length > 0 ? args[0] : "employees.csv";

            if (!File.Exists(input))
            {
                Console.WriteLine($"Input file not found: {input}");
                Console.WriteLine("Usage: dotnet run -- <path-to-csv>");
                return 1;
            }
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("Starting processing of {file}", input);

            try
            {
                var records = CsvParser.ParseCsv(input).ToList();
                if (!records.Any())
                {
                    logger.LogWarning("No records parsed from {file}", input);
                    return 0;
                }
                var svc = new CollaborationService();
                var pairs = svc.GetAllPairsTotalDays(records).ToList();

                if (!pairs.Any())
                {
                    Console.WriteLine("No overlapping collaborations found.");
                    return 0;
                }

                var top = pairs.First();
                Console.WriteLine($"{top.pair.emp1}, {top.pair.emp2}, {top.days}");
                Console.WriteLine();
                Console.WriteLine("Top 10 pairs (emp1, emp2, days):");
                foreach (var p in pairs.Take(10))
                {
                    Console.WriteLine($"{p.pair.emp1}, {p.pair.emp2}, {p.days}");
                }

                logger.LogInformation("Completed processing. Found {count} collaborating pairs.", pairs.Count);
                return 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error");
                return 2;
            }

        }
    }
}