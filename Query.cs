using Dapper;
using Microsoft.Data.Sqlite;

namespace SQLiteQueries
{
    /// <summary>
    /// Class designed to ease operations on simple SQLite queries. To operate, data models must be designed with iherited "Id" property.<br></br>
    /// "Id" must be Primary Key and Autoinkrement in every table.<br></br><br></br>
    /// Delete needs only id.<br></br>
    /// Insert needs only object, and properties will be mapped to colums.<br></br>
    /// Update needs object and its "Id" passed from within your code.<br></br><br></br>
    /// Mapping only works with properties declared within your model (case sensitive). Thats why "Id" must be inherited.
    /// </summary>
    /// <param name="connectionString"></param>
    public class Query(string connectionString)
    {
        SqliteConnection SetConnection => new(connectionString);

        //   -      -      -      -      -   FUNCTIONS   -      -      -      -      -   

        /// <summary>
        /// Looks for last record in table.
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        /// <returns>Last model from table.</returns>
        public T GetLast<T>(string? tableName = null)
        {
            using var connection = SetConnection;

            var query = QueryHelper.Last(tableName ?? typeof(T).Name);

            return connection.Query<T>(query).First();
        }

        /// <summary>
        /// Looks for last record in table asynchronously.
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        /// <returns>Last model from table.</returns>
        public async Task<T> GetLastAsync<T>(string? tableName = null)
        {
            await using var connection = SetConnection;

            var query = QueryHelper.Last(tableName ?? typeof(T).Name);

            return (await connection.QueryAsync<T>(query)).First();
        }

        //   -      -      -      -      -   DELETE   -      -      -      -      -   

        /// <summary>
        /// Deletes record of provided "Id" from the table.
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="id"></param>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        public void DeleteRecord<T>(int id, string? tableName = null)
        {
            using var connection = SetConnection;

            var query = QueryHelper.Delete(tableName ?? typeof(T).Name, id);

            connection.Execute(query);
        }

        /// <summary>
        /// Deletes records of provided "Id" collection from the table.
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="id"></param>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        public void DeleteRecords<T>(IEnumerable<int> ids, string? tableName = null)
        {
            using var connection = SetConnection;

            var query = QueryHelper.DeleteCollection(tableName ?? typeof(T).Name, ids);

            connection.Execute(query);
        }

        /// <summary>
        /// Deletes record of provided "Id" from the table asynchronously.
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="id"></param>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        public async Task DeleteRecordAsync<T>(int id, string? tableName = null)
        {
            await using var connection = SetConnection;

            var query = QueryHelper.Delete(tableName ?? typeof(T).Name, id);

            await connection.ExecuteAsync(query);
        }

        /// <summary>
        /// Deletes records of provided "Id" collection from the table asynchronously.
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="id"></param>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        public async Task DeleteRecordsAsync<T>(IEnumerable<int> ids, string? tableName = null)
        {
            await using var connection = SetConnection;

            var query = QueryHelper.DeleteCollection(tableName ?? typeof(T).Name, ids);

            await connection.ExecuteAsync(query);
        }

        //   -      -      -      -      -   EXECUTE   -      -      -      -      -   

        /// <summary>
        /// Executes provided query.
        /// </summary>
        /// <param name="query">SQL query.</param>
        public void Execute(string query)
        {
            using var connection = SetConnection;
            connection.Execute(query);
        }

        /// <summary>
        /// Executes provided query asynchronously.
        /// </summary>
        /// <param name="query">SQL query.</param>
        public async void ExecuteAsync(string query)
        {
            await using var connection = SetConnection;
            await connection.ExecuteAsync(query);
        }

        //   -      -      -      -      -   INSERT   -      -      -      -      -   

        /// <summary>
        /// Inserts model into table by mapping its properties to columns (case sensitive).
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="model"></param>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        public void InsertRecord<T>(T model, string? tableName = null)
        {
            using var connection = SetConnection;

            var query = QueryHelper.Insert<T>(tableName ?? typeof(T).Name);

            connection.Execute(query, model);
        }

        /// <summary>
        /// Inserts models collection into table by mapping its properties to columns (case sensitive).
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="models"></param>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        public void InsertRecords<T>(IEnumerable<T> models, string? tableName = null)
        {
            using var connection = SetConnection;

            var query = QueryHelper.Insert<T>(tableName ?? typeof(T).Name);

            foreach (var model in models)
            {
                connection.Execute(query, model);
            }
        }

        /// <summary>
        /// Asynchronously inserts model into table by mapping its properties to columns (case sensitive).
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="model"></param>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        public async Task InsertRecordAsync<T>(T model, string? tableName = null)
        {
            await using var connection = SetConnection;
            connection.Execute(QueryHelper.Insert<T>(tableName ?? typeof(T).Name), model);
        }

        /// <summary>
        /// Asynchronously inserts models collection into table by mapping its properties to columns (case sensitive).
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="models"></param>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        public async Task InsertRecordsAsync<T>(IEnumerable<T> models, string? tableName = null)
        {
            await using var connection = SetConnection;

            var query = QueryHelper.Insert<T>(tableName ?? typeof(T).Name);

            foreach (var model in models)
            {
                connection.Execute(query, model);
            }
        }

        //   -      -      -      -      -   READ   -      -      -      -      -   

        /// <summary>
        /// Reads one record from database table.
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="id"></param>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        /// <returns>Model.</returns>
        public T GetRecord<T>(int id, string? tableName = null)
        {
            using var connection = SetConnection;

            var query = QueryHelper.SelectOne(tableName ?? typeof(T).Name, id);

            return connection.QuerySingle<T>(query);
        }

        /// <summary>
        /// Reads collection of records from database table.
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="ids"></param>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        /// <returns>Models collection.</returns>
        public IEnumerable<T> GetRecords<T>(IEnumerable<int> ids, string? tableName = null)
        {
            using var connection = SetConnection;

            var query = QueryHelper.SelectCollection(tableName ?? typeof(T).Name, ids);

            return connection.Query<T>(query);
        }

        /// <summary>
        /// Reads all records from database table.
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        /// <returns>Models collection.</returns>
        public IEnumerable<T> GetAllRecords<T>(string? tableName = null)
        {
            using var connection = SetConnection;

            var query = QueryHelper.SelectAll(tableName ?? typeof(T).Name);

            return connection.Query<T>(query);
        }

        /// <summary>
        /// Asynchronously reads one record from database table.
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="id"></param>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        /// <returns>Model.</returns>
        public async Task<T> GetRecordAsync<T>(int id, string? tableName = null)
        {
            await using var connection = SetConnection;

            var query = QueryHelper.SelectOne(tableName ?? typeof(T).Name, id);

            return await connection.QuerySingleAsync<T>(query);
        }

        /// <summary>
        /// Asynchronously reads collection of records from database table.
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="ids"></param>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        /// <returns>Models collection.</returns>
        public async Task<IEnumerable<T>> GetRecordsAsync<T>(IEnumerable<int> ids, string? tableName = null)
        {
            await using var connection = SetConnection;

            var query = QueryHelper.SelectCollection(tableName ?? typeof(T).Name, ids);

            return await connection.QueryAsync<T>(query);
        }

        /// <summary>
        /// Asynchronously reads all records from database table.
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        /// <returns>Models collection.</returns>
        public async Task<IEnumerable<T>> GetAllRecordsAsync<T>(string? tableName = null)
        {
            await using var connection = SetConnection;

            var query = QueryHelper.SelectAll(tableName ?? typeof(T).Name);

            return await connection.QueryAsync<T>(query);
        }

        //   -      -      -      -      -   UPDATE   -      -      -      -      -   

        /// <summary>
        /// Updates database record with model data of provided id.
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        public void UpdateRecord<T>(T model, int id, string? tableName = null)
        {
            using var connection = SetConnection;

            var query = QueryHelper.Update<T>(tableName ?? typeof(T).Name);

            connection.Execute(query + id, model);
        }

        /// <summary>
        /// Updates database records with models data of provided ids encapsulated into tuples.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="models"></param>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        public void UpdateRecords<T>(IEnumerable<(T, int)> models, string? tableName = null)
        {
            using var connection = SetConnection;

            var query = QueryHelper.Update<T>(tableName ?? typeof(T).Name);

            foreach (var tuple in models)
            {
                var model = tuple.Item1;
                var id = tuple.Item2;

                connection.Execute(query + id, model);
            }
        }

        /// <summary>
        /// Asynchrounously updates database record with model data of provided id.
        /// </summary>
        /// <typeparam name="T">Model type.</typeparam>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        public async Task UpdateRecordAsync<T>(T model, int id, string? tableName = null)
        {
            await using var connection = SetConnection;

            var query = QueryHelper.Update<T>(tableName ?? typeof(T).Name);

            await connection.ExecuteAsync(query + id, model);
        }

        /// <summary>
        /// Asynchrounously updates database records with models data of provided ids encapsulated into tuples.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="models"></param>
        /// <param name="tableName">Table name if diffrent than model name.</param>
        public async Task UpdateRecordsAsync<T>(IEnumerable<(T, int)> models, string? tableName = null)
        {
            await using var connection = SetConnection;

            var query = QueryHelper.Update<T>(tableName ?? typeof(T).Name);

            foreach (var tuple in models)
            {
                var obj = tuple.Item1;
                var id = tuple.Item2;

                await connection.ExecuteAsync(query + id, obj);
            }
        }
    }
}