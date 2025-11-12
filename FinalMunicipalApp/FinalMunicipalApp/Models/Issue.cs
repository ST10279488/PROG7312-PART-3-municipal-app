using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalMunicipalApp.Models
{
    [Serializable]
    public class Issue
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public List<string> AttachmentFileNames { get; set; } = new List<string>();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
