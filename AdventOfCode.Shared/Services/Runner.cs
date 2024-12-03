using AdventOfCode.Shared.Days;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AdventOfCode.Shared.Services
{
    public static class Runner
    {
        public static IDayRunner CreateRunner(string? dayNamespace)
        {
            ServiceCollection services = new();
            services
                .AddSingleton<IFileImporter, FileImporter>()
                .AddSingleton<IScreenWriter, ScreenWriter>()
                .AddSingleton<IDayRunner, DayRunner>();

            var days = Assembly
                .GetEntryAssembly()?
                .GetTypes()
                .Where(p =>
                typeof(IDay).IsAssignableFrom(p) &&
                string.Equals(p.Namespace, dayNamespace) &&
                !p.IsInterface &&
                !p.IsAbstract);

            if (days != null)
            {
                foreach (var day in days)
                {
                    services.AddSingleton(typeof(IDay), day);
                }
            }

            return services
                .BuildServiceProvider()
                .GetRequiredService<IDayRunner>();
        }
    }
}
