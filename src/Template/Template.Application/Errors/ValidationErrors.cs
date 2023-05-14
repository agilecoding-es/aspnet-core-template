using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Entities.Shared;

namespace Template.Application.Errors
{
    public static class ValidationErrors
    {
        public static class Shared
        {
            public static string EntityNotFound(string entityName) => $"The {entityName} could not be found.";
        }

        public static class Sample
        {
            public static class GetSampleListById
            {
                public static readonly string ListWithSameNameAlreadyExists = "The name of the list already exist.";
            }
        }
    }
}
