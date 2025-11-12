using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalMunicipalApp.Models
{
        public enum RequestStatus { Submitted, InProgress, Completed, Rejected }

        public class ServiceRequest : IComparable<ServiceRequest>
        {
            public string RequestId { get; set; } 
            public string Title { get; set; }
            public string Description { get; set; }
            public int Priority { get; set; } 
            public RequestStatus Status { get; set; }
            public DateTime Timestamp { get; set; }

            public ServiceRequest(string requestId, string title, string description, int priority, RequestStatus status)
            {
                RequestId = requestId;
                Title = title;
                Description = description;
                Priority = priority;
                Status = status;
                Timestamp = DateTime.Now;
            }

            public int CompareTo(ServiceRequest other)
            {
                return string.Compare(RequestId, other.RequestId, StringComparison.Ordinal);
            }

            public override string ToString()
            {
                return $"{RequestId} | {Title} | {Status} | Priority:{Priority} | {Timestamp:yyyy-MM-dd HH:mm}";
            }
        }
    }
