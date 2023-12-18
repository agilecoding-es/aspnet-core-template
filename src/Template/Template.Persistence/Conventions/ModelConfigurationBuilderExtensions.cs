using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Template.Persistence.Conventions
{
    public static class ModelConfigurationBuilderExtensions : IConvention
    {
        public static void Apply(this ModelConfigurationBuilder configurationBuilder)
        {
            foreach (var property in configurationBuilder.Properties())
            {

            }
                // Aplica la convención de nombres a las propiedades
                foreach (var property in entityType.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName(entityType).PascalCaseToSnakeCase());
                }

                // Aplica la convención de nombres a la tabla
                entityType.SetTableName(entityType.GetTableName().PascalCaseToSnakeCase());
        }
    }
}
