using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Configuration
{
    public record TemplateConfiguration(
        bool EnableAspNetIdentity = true
        );
}
