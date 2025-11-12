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
    public static class IssueRepository
    {
        private static readonly string DataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "issues.xml");
        private static readonly string AttachDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Attachments");

        public static List<Issue> Issues { get; private set; } = Load();

        private static List<Issue> Load()
        {
            try
            {
                if (!File.Exists(DataFile)) return new List<Issue>();
                var serializer = new XmlSerializer(typeof(List<Issue>));
                using (var stream = File.OpenRead(DataFile))
                {
                    return (List<Issue>)serializer.Deserialize(stream);
                }
            }
            catch
            {
                return new List<Issue>();
            }
        }

        public static void SaveAll()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<Issue>));
                using (var stream = File.Create(DataFile))
                {
                    serializer.Serialize(stream, Issues);
                }
            }
            catch { }
        }

        public static string EnsureAttachmentsFolder()
        {
            if (!Directory.Exists(AttachDir)) Directory.CreateDirectory(AttachDir);
            return AttachDir;
        }

        public static void Add(Issue issue)
        {
            Issues.Add(issue);
            SaveAll();
        }


    }
}
