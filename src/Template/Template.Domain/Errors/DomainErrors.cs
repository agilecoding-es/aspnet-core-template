using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Entities.Shared;

namespace Template.Domain.Errors
{
    public static class DomainErrors
    {
        public static class SampleList
        {
            public static readonly Error UniqueName = new Error("Sample_UniqueName", "The name of the list already exist.");
        }
    }
}
