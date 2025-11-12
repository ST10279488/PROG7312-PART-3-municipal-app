using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalMunicipalApp.Models
{
    [Serializable]
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Priority { get; set; } // 1-high, 5-low
        public List<string> Tags { get; set; } = new List<string>();

        public override string ToString() => $"{Date:yyyy-MM-dd} - {Title} ({Category})";
    }
}

