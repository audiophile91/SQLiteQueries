using System.Reflection;

namespace SQLiteQueries
{
    static class QueryHelper
    {
        internal static string SelectAll(string tableName) => $"SELECT * FROM {tableName}"; 
        internal static string SelectOne(string tableName, int id) => $"SELECT * FROM {tableName} WHERE Id = {id}";
        internal static string SelectCollection(string tableName, IEnumerable<int> ids) => $"SELECT * FROM {tableName} WHERE Id IN ({string.Join(',', ids)})";

        internal static string Delete(string tableName, int id) => $"DELETE FROM {tableName} WHERE Id = {id}";
        internal static string DeleteCollection(string tableName, IEnumerable<int> ids) => $"DELETE FROM {tableName} WHERE Id IN ({string.Join(',', ids)})";
        internal static string Insert<T>(string tableName)
        {
            var names = GetPropertiesNames<T>();

            var set = string.Join(", ", names);
            var setAt = string.Join(", ", names.Select(p => $"@{p}"));

            return $"INSERT INTO {tableName} ({set}) VALUES ({setAt})";
        }
        internal static string Update<T>(string tableName)
        {
            var names = GetPropertiesNames<T>();
            var namesSet = names.Select(name => $"{name} = @{name}");
            var update = string.Join(',', namesSet);

            return $"UPDATE {tableName} SET {update} WHERE Id = ";
        }

        internal static string Last(string tableName) => $"SELECT * FROM {tableName} ORDER BY Id DESC LIMIT 1";

        static IEnumerable<string> GetPropertiesNames<T>()
        {
            return GetProperties<T>().Select(p => p.Name);
        }
        static PropertyInfo[] GetProperties<T>()
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }
    }
}