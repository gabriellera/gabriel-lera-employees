using gabriel_lera_employees.Models;

namespace gabriel_lera_employees.Services
{
    public class CollaborationService
    {
        public IEnumerable<((int emp1, int emp2) pair, int days)> GetAllPairsTotalDays(IEnumerable<WorkRecord> records)
        {
            var projects = records
                .GroupBy(r => r.ProjectID)
                .ToDictionary(
                    g => g.Key,
                    g => g.GroupBy(r => r.EmpID)
                          .ToDictionary(
                              eg => eg.Key,
                              eg => MergeIntervals(eg.Select(x => (from: x.DateFrom, to: x.DateTo)).ToList())
                          )
                );

            var pairTotals = new Dictionary<(int, int), int>();

            foreach (var projectEntry in projects)
            {
                var empIntervals = projectEntry.Value;
                var empIds = empIntervals.Keys.OrderBy(x => x).ToList();

                for (int i = 0; i < empIds.Count; i++)
                {
                    for (int j = i + 1; j < empIds.Count; j++)
                    {
                        var empA = empIds[i];
                        var empB = empIds[j];
                        var intervalsA = empIntervals[empA];
                        var intervalsB = empIntervals[empB];

                        int overlapDays = GetTotalOverlapBetweenIntervalLists(intervalsA, intervalsB);

                        if (overlapDays > 0)
                        {
                            var key = (empA < empB) ? (empA, empB) : (empB, empA);
                            if (!pairTotals.ContainsKey(key)) pairTotals[key] = 0;
                            pairTotals[key] += overlapDays;
                        }
                    }
                }
            }
            return pairTotals.OrderByDescending(kv => kv.Value)
                             .Select(kv => (kv.Key, kv.Value));
        }

        public static List<(DateTime from, DateTime to)> MergeIntervals(List<(DateTime from, DateTime to)> ranges)
        {
            if (ranges == null || ranges.Count == 0) return new List<(DateTime, DateTime)>();
            var sorted = ranges.OrderBy(r => r.from).ThenBy(r => r.to).ToList();
            var result = new List<(DateTime from, DateTime to)>();
            var current = sorted[0];

            for (int i = 1; i < sorted.Count; i++)
            {
                var next = sorted[i];
                if (next.from <= current.to.AddDays(1))
                {
                    current.to = current.to > next.to ? current.to : next.to;
                }
                else
                {
                    result.Add(current);
                    current = next;
                }
            }
            result.Add(current);
            return result;
        }
        private static int GetTotalOverlapBetweenIntervalLists(
            List<(DateTime from, DateTime to)> a,
            List<(DateTime from, DateTime to)> b)
        {
            int total = 0;
            int i = 0, j = 0;
            while (i < a.Count && j < b.Count)
            {
                var aFrom = a[i].from;
                var aTo = a[i].to;
                var bFrom = b[j].from;
                var bTo = b[j].to;

                var start = aFrom > bFrom ? aFrom : bFrom;
                var end = aTo < bTo ? aTo : bTo;

                if (end >= start)
                {
                    total += (end - start).Days + 1;
                }

                if (aTo < bTo) i++;
                else j++;
            }
            return total;
        }
        public class PairProjectOverlap
        {
            public int Emp1 { get; set; }
            public int Emp2 { get; set; }
            public int ProjectID { get; set; }
            public int DaysWorked { get; set; }
        }

        public List<PairProjectOverlap> GetAllPairProjectOverlaps(List<WorkRecord> records)
        {
            var results = new List<PairProjectOverlap>();

            var projects = records.GroupBy(r => r.ProjectID);

            foreach (var projectGroup in projects)
            {
                var employeeIntervals = projectGroup
                    .GroupBy(r => r.EmpID)
                    .ToDictionary(
                        g => g.Key,
                        g => MergeIntervals(g.Select(r => (r.DateFrom, r.DateTo)).ToList())
                    );

                var empIds = employeeIntervals.Keys.OrderBy(x => x).ToList();

                for (int i = 0; i < empIds.Count; i++)
                {
                    for (int j = i + 1; j < empIds.Count; j++)
                    {
                        int totalOverlap = GetTotalOverlapBetweenIntervalLists(employeeIntervals[empIds[i]], employeeIntervals[empIds[j]]);
                        if (totalOverlap > 0)
                        {
                            results.Add(new PairProjectOverlap
                            {
                                Emp1 = empIds[i],
                                Emp2 = empIds[j],
                                ProjectID = projectGroup.Key,
                                DaysWorked = totalOverlap
                            });
                        }
                    }
                }
            }

            return results;
        }
    }
}
