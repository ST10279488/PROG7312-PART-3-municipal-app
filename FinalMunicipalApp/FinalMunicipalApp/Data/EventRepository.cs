using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FinalMunicipalApp.Models;

namespace FinalMunicipalApp.Data
{
    public static class EventRepository
    {
        private static readonly string DataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "events.xml");
        private static List<Event> events = Load();

        public static SortedDictionary<DateTime, List<Event>> EventsByDate { get; private set; } = new SortedDictionary<DateTime, List<Event>>();
        public static Dictionary<string, List<Event>> EventsByCategory { get; private set; } = new Dictionary<string, List<Event>>(StringComparer.OrdinalIgnoreCase);
        public static HashSet<string> UniqueCategories { get; private set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public static HashSet<DateTime> UniqueDates { get; private set; } = new HashSet<DateTime>();
        public static Stack<Event> RecentViewedStack { get; private set; } = new Stack<Event>();
        public static Queue<Event> UpcomingQueue { get; private set; } = new Queue<Event>();
        private static SortedDictionary<int, Queue<Event>> priorityQueues = new SortedDictionary<int, Queue<Event>>();

        static EventRepository() => RebuildIndexes();

        private static List<Event> Load()
        {
            try
            {
                if (!File.Exists(DataFile))
                {
                    var seed = SeedEvents();
                    SaveAll(seed);
                    return seed;
                }

                var serializer = new XmlSerializer(typeof(List<Event>));
                using (var stream = File.OpenRead(DataFile))
                    return (List<Event>)serializer.Deserialize(stream);
            }
            catch
            {
                return new List<Event>();
            }
        }

        private static List<Event> SeedEvents()
        {
            return new List<Event>
            {
                new Event{ Title = "Community Clean-Up", Category="Sanitation", Description="Join neighbours to clean up the park.", Date=DateTime.Today.AddDays(3), Priority=2, Tags=new List<string>{"community","cleanup"}},
                new Event{ Title = "Road Safety Workshop", Category="Roads", Description="Workshop about road safety.", Date=DateTime.Today.AddDays(7), Priority=1, Tags=new List<string>{"safety","roads"}},
                new Event{ Title = "Electricity Maintenance Notice", Category="Utilities", Description="Scheduled maintenance for suburb.", Date=DateTime.Today.AddDays(2), Priority=3, Tags=new List<string>{"electricity","maintenance"}},
                new Event{ Title = "Tree Planting Day", Category="Parks & Recreation", Description="Plant trees in the community park.", Date=DateTime.Today.AddDays(14), Priority=2, Tags=new List<string>{"trees","environment"}},
                new Event{ Title = "Market Day", Category="Community", Description="Local artisans market.", Date=DateTime.Today.AddDays(5), Priority=4, Tags=new List<string>{"market","community"}}
            };
        }

        public static void RebuildIndexes()
        {
            EventsByDate.Clear();
            EventsByCategory.Clear();
            UniqueCategories.Clear();
            UniqueDates.Clear();
            priorityQueues.Clear();
            RecentViewedStack = new Stack<Event>();
            UpcomingQueue = new Queue<Event>();

            foreach (var ev in events)
            {
                var key = ev.Date.Date;
                if (!EventsByDate.ContainsKey(key)) EventsByDate[key] = new List<Event>();
                EventsByDate[key].Add(ev);

                if (!EventsByCategory.ContainsKey(ev.Category)) EventsByCategory[ev.Category] = new List<Event>();
                EventsByCategory[ev.Category].Add(ev);

                UniqueCategories.Add(ev.Category);
                UniqueDates.Add(key);

                if (!priorityQueues.ContainsKey(ev.Priority)) priorityQueues[ev.Priority] = new Queue<Event>();
                priorityQueues[ev.Priority].Enqueue(ev);

                UpcomingQueue.Enqueue(ev);
            }

            foreach (var k in EventsByDate.Keys.ToList())
                EventsByDate[k] = EventsByDate[k].OrderBy(e => e.Priority).ThenBy(e => e.Title).ToList();
        }

        public static IEnumerable<Event> GetEventsByDate(DateTime date)
        {
            var key = date.Date;
            if (EventsByDate.ContainsKey(key)) return EventsByDate[key];
            return Enumerable.Empty<Event>();
        }

        public static IEnumerable<Event> GetEventsByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category)) return events;
            if (EventsByCategory.ContainsKey(category)) return EventsByCategory[category];
            return Enumerable.Empty<Event>();
        }

        public static IEnumerable<Event> Search(string category, DateTime? date, string query)
        {
            IEnumerable<Event> results = events;
            if (!string.IsNullOrWhiteSpace(category)) results = results.Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            if (date.HasValue) results = results.Where(e => e.Date.Date == date.Value.Date);
            if (!string.IsNullOrWhiteSpace(query))
            {
                var q = query.ToLowerInvariant();
                results = results.Where(e => (e.Title != null && e.Title.ToLowerInvariant().Contains(q)) || (e.Description != null && e.Description.ToLowerInvariant().Contains(q)) || (e.Tags != null && e.Tags.Any(t => t.ToLowerInvariant().Contains(q))));
            }
            return results.OrderBy(e => e.Date).ThenBy(e => e.Priority);
        }

        public static Event DequeueHighestPriority()
        {
            if (priorityQueues.Count == 0) return null;
            var firstKey = priorityQueues.Keys.First();
            var q = priorityQueues[firstKey];
            var ev = q.Dequeue();
            if (q.Count == 0) priorityQueues.Remove(firstKey);
            return ev;
        }

        public static void AddEvent(Event ev)
        {
            events.Add(ev);
            SaveAll(events);
            RebuildIndexes();
        }

        public static void SaveAll(List<Event> list)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<Event>));
                using (var stream = File.Create(DataFile)) serializer.Serialize(stream, list);
            }
            catch { }
        }

        public static List<Event> GetAll() => events;
    }
}
