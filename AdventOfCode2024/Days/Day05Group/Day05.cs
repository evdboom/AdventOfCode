using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using System.Linq;

namespace AdventOfCode2024.Days
{
    public class Day05 : Day
    {

        public Day05(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 5;

        protected override long ProcessPartOne(string[] input)
        {
            var (pages, orders) = GetPagesAndOrders(input);
            return ValidateOrders(pages, orders, valid: true)
                .Aggregate(0L, (acc, order) =>
                {
                    return acc + order[order.Count / 2];
                });

        }

        protected override long ProcessPartTwo(string[] input)
        {
            var (pages, orders) = GetPagesAndOrders(input);
            return ValidateOrders(pages, orders, valid: false)
                .Aggregate(0L, (acc, order) =>
                {
                    return acc + Reorder(pages, order)
                        .ElementAt(order.Count / 2);
                });
        }

        private IEnumerable<int> Reorder(Dictionary<int, HashSet<int>> pages, List<int> order)
        {    
            HashSet<int> returned = [];
            HashSet<int> skipped = [];
            bool itemAdded = false;
            for (int i = 0; i < order.Count; i++)
            {
                if (itemAdded)
                {
                    foreach (var skip in skipped)
                    {
                        var canReturnSkipped = order
                            .Intersect(pages.TryGetValue(skip, out var skippedValue) ? skippedValue : [])
                            .All(returned.Contains);
                        if (canReturnSkipped)
                        {
                            itemAdded = true;
                            skipped.Remove(skip);
                            returned.Add(skip);
                            yield return skip;
                        }
                    }
                }

                var page = order[i];
                var canReturn = order
                    .Intersect(pages.TryGetValue(page, out var value) ? value : [])
                    .All(returned.Contains);
                if (canReturn)
                {
                    itemAdded = true;
                    returned.Add(page);
                    yield return page;
                }
                else
                {
                    itemAdded = false;
                    skipped.Add(page);
                }
            }    
            
            while (skipped.Count > 0)
            {
                foreach (var skip in skipped)
                {
                    var canReturn = order
                        .Intersect(pages.TryGetValue(skip, out var skippedValue) ? skippedValue : [])
                        .All(returned.Contains);
                    if (canReturn)
                    {
                        skipped.Remove(skip);
                        returned.Add(skip);
                        yield return skip;
                    }
                }
            }
        }

        private IEnumerable<List<int>> ValidateOrders(Dictionary<int, HashSet<int>> pages, List<List<int>> orders, bool valid)
        {
            foreach (var order in orders)
            {
                var seen = new HashSet<int>();
                var isValid = true;
                for (int i = order.Count - 1; i >= 0; i--)
                {
                    var dependents = pages.TryGetValue(order[i], out var known) ? known : [];
                    if (seen.Any(dependents.Contains))
                    {
                        isValid = false;
                        break;
                    }
                    seen.Add(order[i]);
                }

                if (valid == isValid)
                {
                    yield return order;
                }
            }
        }

        private (Dictionary<int, HashSet<int>> Pages, List<List<int>> Orders) GetPagesAndOrders(string[] input)
        {
            var pages = new Dictionary<int, HashSet<int>>();
            var orders = new List<List<int>>();

            var ordersStarted = false;

            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    ordersStarted = true;
                    continue;
                }

                if (!ordersStarted)
                {
                    var parts = line
                        .Split('|')
                        .Select(int.Parse)
                        .ToArray();
                    var dependent = parts[0];
                    var page = parts[1];

                    if (!pages.TryGetValue(page, out var dependents))
                    {
                        pages[page] = [dependent];
                    }
                    else
                    {
                        pages[page].Add(dependent);
                    }
                }
                else
                {
                    orders.Add(line
                            .Split(',')
                            .Select(int.Parse)
                            .ToList());
                }
            }

            return (pages, orders);
        }

    }
}
