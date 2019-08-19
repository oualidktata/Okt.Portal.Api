using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Api.Repositories
{
    public static class RepoExtensions
    {
        public static string GetValueOfProperty<T>(this T item, string propName)
        {
            return item.GetType().GetProperty(propName).GetValue(item, null) as string;
        }
    }
}
