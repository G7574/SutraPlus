using SutraPlus_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SutraPlus_DAL.Data
{
    public class DBContext
    {
        //    private DatabaseHandlerFactory dbFactory;
        private IDatabaseHandler database;
        private string providerName;
        public IDatabaseHandler _IDatabaseHandler;
        public DBContext(IDatabaseHandler DatabaseHandler)
        {
            //   dbFactory = new DatabaseHandlerFactory(settings);
            // database = dbFactory.CreateDatabase();
            database = DatabaseHandler;
            //  providerName = dbFactory.GetProviderName();

        }
        public IDbConnection GetDatabasecOnnection()
        {
            return database.CreateConnection();
        }

        public void CloseConnection(IDbConnection connection)
        {
            database.CloseConnection(connection);
        }
        public IDbDataParameter CreateParameter(string name, object value, DbType dbType)
        {
            return DataParameterManager.CreateParameter(name, value, dbType, ParameterDirection.Input);
        }
        public IDbDataParameter CreateParameterOut(string name, object value, DbType dbType)
        {
            return DataParameterManager.CreateParameter(name, value, dbType, ParameterDirection.Output);
        }
        public IDbDataParameter CreateParameter(string name, int size, object value, DbType dbType)
        {
            return DataParameterManager.CreateParameter(providerName, name, size, value, dbType, ParameterDirection.Input);
        }
        public IDbDataParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection direction)
        {
            return DataParameterManager.CreateParameter(providerName, name, size, value, dbType, direction);
        }
        public DataTable GetDataTable(string commandText, CommandType commandType, IDbDataParameter[] parameters = null)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    var dataset = new DataSet();
                    var dataAdaper = database.CreateAdapter(command);
                    dataAdaper.Fill(dataset);
                    return dataset.Tables[0];
                }
            }
        }
        public DataSet GetDataSet(string commandText, CommandType commandType, IDbDataParameter[] parameters = null)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    var dataset = new DataSet();
                    var dataAdaper = database.CreateAdapter(command);
                    dataAdaper.Fill(dataset);
                    return dataset;
                }
            }
        }



        public IDataReader GetDataReader(string commandText, CommandType commandType, IDbDataParameter[] parameters, out IDbConnection connection)
        {
            IDataReader reader = null;
            connection = database.CreateConnection();
            connection.Open();
            var command = database.CreateCommand(commandText, commandType, connection);
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            reader = command.ExecuteReader();

            return reader;
        }
        public void Delete(string commandText, CommandType commandType, IDbDataParameter[] parameters = null)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        public int DeleteWithRecordsAffected(string commandText, CommandType commandType, IDbDataParameter[] parameters = null)
        {
            int res = 0;
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }
        public void Insert(string commandText, CommandType commandType, IDbDataParameter[] parameters)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    command.ExecuteNonQuery();
                }
            }
        }


        public int Insert(string commandText, CommandType commandType, IDbDataParameter[] parameters, int itemId)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    var id = string.Empty;
                    //object newId = command.ExecuteReader();
                    using (IDataReader rdr = command.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            itemId = Convert.ToInt32(rdr["iden"].ToString());
                        }
                        rdr.Close();
                    }
                }
            }
            return itemId;
        }

        public long Insert(string commandText, CommandType commandType, IDbDataParameter[] parameters, out long lastId)
        {
            lastId = 0;
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    object newId = command.ExecuteScalar();
                    lastId = Convert.ToInt64(newId);
                }
            }
            return lastId;
        }
        public void InsertWithTransaction(string commandText, CommandType commandType, IDbDataParameter[] parameters)
        {
            IDbTransaction transactionScope = null;
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                transactionScope = connection.BeginTransaction();
                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    try
                    {
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public void InsertWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, IDbDataParameter[] parameters)
        {
            IDbTransaction transactionScope = null;
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                transactionScope = connection.BeginTransaction(isolationLevel);
                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    try
                    {
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public void Update(string commandText, CommandType commandType, IDbDataParameter[] parameters)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        public int UpdateWithRecordsAffected(string commandText, CommandType commandType, IDbDataParameter[] parameters)
        {
            int res = 0;
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }
        public void UpdateWithTransaction(string commandText, CommandType commandType, IDbDataParameter[] parameters)
        {
            IDbTransaction transactionScope = null;
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                transactionScope = connection.BeginTransaction();
                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    try
                    {
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public void UpdateWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, IDbDataParameter[] parameters)
        {
            IDbTransaction transactionScope = null;
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                transactionScope = connection.BeginTransaction(isolationLevel);
                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    try
                    {
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public object GetScalarValue(string commandText, CommandType commandType, IDbDataParameter[] parameters = null)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    return command.ExecuteScalar();
                }
            }
        }



        public List<T> RetrieveEntities<T>(string commandText, CommandType commandType, IDbDataParameter[] parameters = null) where T : class, new()
        {
            //command.CommandTimeout = 100;

            DataSet dataSet = new DataSet();

            if ((commandType == null) || (commandText == null) || (commandText.Length == 0))
            {
                throw new Exception("Invailid command");
            }
            //  dataSet = ExecDataSet(command);
            dataSet = GetDataSet(commandText, commandType, parameters);
            // Return null in case dataset returns no result
            if (dataSet.Tables.Count == 0)
            {
                return null;
            }
            if (dataSet.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            string[] columnNames = dataSet.Tables[0].Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();
            try
            {
                List<T> list = new List<T>();

                //  foreach (var row in dataSet.Tables[0].AsEnumerable())
                // {
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {

                    //var row = dataSet.Tables[0].Rows[i];
                    //   }
                    T obj = new T();
                    DataRow row = dataSet.Tables[0].Rows[i];


                    // foreach (var prop in obj.GetType().GetProperties())
                    foreach (var prop in columnNames)
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop);
                            var type = propertyInfo.PropertyType;
                            //if (type.Name == "HttpPostedFileBase")
                            //{
                            //    propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                            //}
                            //else
                            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                var result = System.Convert.ChangeType(row[prop], Nullable.GetUnderlyingType(propertyInfo.PropertyType));
                                propertyInfo.SetValue(obj, result, null);
                            }
                            else
                            {
                                propertyInfo.SetValue(obj, Convert.ChangeType(row[prop], propertyInfo.PropertyType), null);
                            }
                        }
                        catch (Exception ex)
                        {
                            //continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Executes the command supplied and returns a Data Reader
        /// </summary>
        /// <param name="command">Command to be executed</param>
        /// <returns>Data reader object</returns>
        /// <remarks>The DbDataReader Retrieves multiple rows</remarks>
        //public object RetrieveEntities(SqlCommand command, Type type, Type entityType)
        //{
        //    DataSet dataSet = new DataSet();
        //    return RetrieveEntities(command, type, entityType, ref dataSet);
        //}

        /// <summary>
        /// Executes the command supplied and returns a Data Reader
        /// </summary>
        /// <param name="command">Command to be executed</param>
        /// <returns>Data reader object</returns>
        /// <remarks>The DbDataReader Retrieves multiple rows</remarks>
        //public object RetrieveEntities(SqlCommand command, Type type, Type entityType, ref DataSet dataSet)
        //{
        //    if ((command == null) || (command.CommandText == null) || (command.CommandText.Length == 0) || (type == null))
        //    {
        //        throw new Exception("Invailid command");
        //    }
        //    dataSet = ExecDataSet(command);
        //    // Return null in case dataset returns no result
        //    if (dataSet.Tables[0].Rows.Count == 0)
        //    {
        //        return null;
        //    }
        //    // If the desired object is entity collection
        //    if (type.GetInterface("IList") != null)
        //    {
        //        // Set default name for dataset
        //        dataSet.DataSetName = "List";
        //        dataSet.Tables[0].TableName = entityType.Name;
        //    }
        //    else
        //    {
        //        // If the type is entity
        //        if (dataSet.Tables[0].Rows.Count > 1)
        //        {
        //            throw new Exception("The Query returns multiple records.");
        //        }
        //        // Set table name as the class name
        //        dataSet.Tables[0].TableName = entityType.Name;
        //    }

        //    // Get XML from dataset
        //    string dataSetXml = dataSet.GetXml();
        //    // Clean the XML retrieved fron dataset
        //    XmlDocument xmlDocument = new XmlDocument();
        //    xmlDocument.LoadXml(dataSetXml);
        //    dataSetXml = (type.GetInterface("IList") != null) ? xmlDocument.DocumentElement.OuterXml : xmlDocument.DocumentElement.InnerXml;
        //    // Deserialize the XML into object
        //    return Deserialize(dataSetXml, type);
        //}


        /// <summary>
        /// Deserializes XML string into object
        /// </summary>
        /// <param name="xmlString">XML as retrieved from dataset</param>
        /// <param name="type">Type of the desired object</param>
        /// <returns>Deserialized object</returns>
        private Object Deserialize(string xmlString, System.Type type)
        {
            XmlSerializer xsObject = new XmlSerializer(type);
            // Create a new memory stream
            MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xmlString));
            //  object obj = (Object)xsObject.Deserialize(memoryStream);
            object obj = (object)xsObject.Deserialize(memoryStream);
            memoryStream.Close();
            return obj;
        }

        private Byte[] StringToUTF8ByteArray(string xmlString)
        {
            // Create UTF8 encoding object
            UTF8Encoding encoding = new UTF8Encoding();
            // Get xml as array of bytes
            Byte[] byteArray = encoding.GetBytes(xmlString);
            return byteArray;
        }


    }

}
