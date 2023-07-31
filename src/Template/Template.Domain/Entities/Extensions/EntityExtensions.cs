using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Entities.Abastractions;
using Template.Domain.Exceptions;

namespace Template.Domain.Entities.Extensions
{
    public static class EntityExtensions
    {
        public static void RecreateList<K, T>(this List<T> existingList, List<T> updatedList)
            where T : Entity<K>
            where K : IEquatable<K>
        {
            if (existingList == null)
                throw new InvalidOperationException("List<T> property is null or not of type IList.");

            // Eliminar los T que ya no existen
            var itemsToRemove = existingList.Where(e => !updatedList.Any(e2 => e2.Id.Equals(e.Id))).ToList();
            foreach (var itemToRemove in itemsToRemove)
            {
                if (typeof(T).IsAssignableFrom(typeof(ISoftDelete)))
                {
                    (itemsToRemove as ISoftDelete).Delete();
                }
                else
                {
                    existingList.Remove(itemToRemove);
                }
            }
            existingList.AddRange(updatedList.Where(e => !existingList.Any(e2 => e2.Id.Equals(e.Id))).ToList());
        }

        public static void UpdateList<K, T>(this List<T> existingList, List<T> updatedList)
            where T : Entity<K>
            where K : IEquatable<K>
        {
            if (existingList == null)
                throw new InvalidOperationException("List<T> property is null or not of type IList.");

            var existingItemIds = existingList.Select(i => i.Id).ToList();

            foreach (var updatedItem in updatedList)
            {
                if (existingItemIds.Contains(updatedItem.Id))
                {
                    // Actualizar propiedades de T 
                    var existingItem = existingList.First(i => i.Id.Equals(updatedItem.Id));
                    existingItem.Update(updatedItem);
                }
                else
                {
                    existingList.Add(updatedItem);
                }
            }

            // Eliminar los T que ya no existen
            var itemsToRemove = existingList.Where(e => !updatedList.Any(e2 => e2.Id.Equals(e.Id))).ToList();
            foreach (var itemToRemove in itemsToRemove)
            {
                if (typeof(T).IsAssignableFrom(typeof(ISoftDelete)))
                {
                    (itemsToRemove as ISoftDelete).Delete();
                }
                else
                {
                    existingList.Remove(itemToRemove);
                }
            }
            existingList.AddRange(updatedList.Where(e => !existingList.Any(e2 => e2.Id.Equals(e.Id))).ToList());

        }
    }
}
