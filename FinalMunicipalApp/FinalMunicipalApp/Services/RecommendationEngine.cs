using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalMunicipalApp.Data;
using FinalMunicipalApp.Models;

namespace FinalMunicipalApp.Services
{
    public static class RecommendationEngine
    {
        private static Dictionary<string, int> categorySearchCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private static Dictionary<string, int> tagSearchCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        public static void LogSearch(string category, string query)
        {
            if (!string.IsNullOrWhiteSpace(category))
            {
                if (!categorySearchCounts.ContainsKey(category)) categorySearchCounts[category] = 0;
                categorySearchCounts[category]++;
            }

            if (!string.IsNullOrWhiteSpace(query))
            {
                var tokens = query.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var t in tokens)
                {
                    var token = t.Trim().ToLowerInvariant();
                    if (string.IsNullOrWhiteSpace(token)) continue;
                    if (!tagSearchCounts.ContainsKey(token)) tagSearchCounts[token] = 0;
                    tagSearchCounts[token]++;
                }
            }
        }

        public static IEnumerable<Event> Recommend(int max = 5)
        {
            var all = EventRepository.GetAll();
            var topCats = categorySearchCounts.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).Take(3).ToList();
            var topTags = tagSearchCounts.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).Take(5).ToList();

            var scored = all.Select(ev => new {
                Ev = ev,
                Score = (topCats.Contains(ev.Category) ? 5 : 0) + ev.Tags.Count(t => topTags.Contains(t.ToLowerInvariant())) - ((ev.Date - DateTime.Today).TotalDays / 30.0)
            })
            .OrderByDescending(x => x.Score)
            .ThenBy(x => (x.Ev.Date - DateTime.Today).TotalDays)
            .Select(x => x.Ev)
            .Take(max);

            return scored;
        }
    }
}
