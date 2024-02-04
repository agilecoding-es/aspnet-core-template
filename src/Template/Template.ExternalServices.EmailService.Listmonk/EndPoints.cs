using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.ExternalServices.EmailService.Listmonk
{
    public static class EndPoints
    {
        public static string Transactional() => "tx";
        public static string GetTemplates() => "templates";
        public static string GetTemplate(int templateId) => $"templates/{templateId}";
        public static string PreviewTemplate(int templateId) => $"templates/{templateId}/preview";
    }
}
