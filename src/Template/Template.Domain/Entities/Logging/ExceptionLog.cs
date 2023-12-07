using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Template.Domain.Entities.Logging
{
    public class ExceptionLog
    {
        public long Id { get; set; }
        public DateTime InsertDate { get; set; }
        public int IdLevelError { get; set; }
        public string LevelError { get; set; }
        public string SiteName { get; set; }
        public string ProcessName { get; set; }
        public string Message { get; set; }
        public string InnerException { get; set; }
        public string StackTrace { get; set; }
        public string UserAgent { get; set; }
        public string Headers { get; set; }
        public string Properties { get; set; }
        public string ClassMethod { get; set; }
        public string MachineName { get; set; }
        public string MachineIP { get; set; }
        public string Logger { get; set; }
        public string IdUser { get; set; }
        public string RemoteAddress { get; set; }
        public string FullReferer { get; set; }
        public string FullUrl { get; set; }
    }
}
