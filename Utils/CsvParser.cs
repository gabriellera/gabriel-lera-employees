using gabriel_lera_employees.Models;
using System.Globalization;

namespace gabriel_lera_employees.Utils
{
    public static class CsvParser
    {
        public static IEnumerable<WorkRecord> ParseCsv(string path)
        {

            if (!File.Exists(path))
                throw new FileNotFoundException($"CSV file not found: {path}");


            var lines = File.ReadAllLines(path);
            int start = 0;
            if (lines.Length > 0 && lines[0].IndexOf("Emp", StringComparison.OrdinalIgnoreCase) >= 0) start = 1;

            for (int i = start; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',').Select(p => p.Trim()).ToArray();

                if (parts.Length < 4) continue;
                if (!int.TryParse(parts[0], out int empId)) continue;
                if (!int.TryParse(parts[1], out int projectId)) continue;

                var dateFrom = ParseDate(parts[2]);
                var dateTo = ParseDate(parts[3]);

                var from = dateFrom ?? DateTime.Today;
                var to = dateTo ?? DateTime.Today;

                if (from > to)
                {
                    var t = from;
                    from = to;
                    to = t;
                }
                yield return new WorkRecord
                {
                    EmpID = int.Parse(parts[0]),
                    ProjectID = int.Parse(parts[1]),
                    DateFrom = from.Date,
                    DateTo = to.Date,
                };
            }
        }

        private static DateTime? ParseDate(string date)
        {
            if (string.Equals(date, "NULL", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(date))
                return null;
            string[] formats = { "yyyy-MM-dd", "yyyy/MM/dd", "MM/dd/yyyy", "dd/MM/yyyy" };

            if (DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsed))
                return parsed;
            if (DateTime.TryParse(date, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out parsed))
                return parsed.Date;

            return null;
        }
    }
}
