using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Fluent.ORM.Extensions
{
    internal static class StringExtensions
    {
        internal static string GetTableName(this string fullTableName)
        {
            if (fullTableName.Contains('.'))
            {
                return fullTableName.Split('.')[1].ToLowerInvariant();
            }
            return fullTableName;
        }
    }
}
