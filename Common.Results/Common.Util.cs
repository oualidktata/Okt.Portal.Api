using Common.Result;
using System.Collections.Generic;
using System.Linq;

namespace Common.Results
{
    public static class GenericResultExtension
    {
        /// <summary>
        /// Tries to get a unique element from a list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listOfItems">List of items to check upon.</param>
        /// <param name="itemName">Item to look for.</param>
        /// <returns>if success,An IResult<T> with the unique item.
        /// if duplicate is found, returns false and an message.
        /// if item was not found, returns false and an message.
        /// </returns>
        public static IResult<T> TryGetUniqueItemFromList<T>(this IEnumerable<T> listOfItems, string itemName) where T : class, new()
        {

            if (listOfItems == null || !listOfItems.Any())
            {
                return new Result<T>(false, new T(), $"{itemName} was not found");
            }
            if (listOfItems.Count() > 1)
            {
                return new Result<T>(false, new T(), $"More than one {itemName} were found");
            }
            var uniqueItem = listOfItems.Single();
            if (uniqueItem == null)
            {
                return new Result<T>(false, uniqueItem, $"{itemName} was not found");
            }
            else
            {
                return new Result<T>(true, uniqueItem);
            }
        }
    }
}
