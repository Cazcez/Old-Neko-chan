using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neko_Chan.Sql
{
    public class SQLite
    {
        public struct DatabaseCreationBlock
        {
            public string DatabaseName;
            public string TableName;
            public string[] TableParams;
        }
        #region Varibles
        public bool Opened
        {
            get
            {
                if (sqlConn == null)
                    return false;

                if (sqlConn.State != System.Data.ConnectionState.Closed)
                    return true;
                else
                    return false;
            }
        }
        private string _connectionString { get; set; }
        private string _tableName { get; set; }
        public SQLiteConnection sqlConn;
        #endregion
        public SQLite(string ConnectionString, string Table = null)
        {
            _connectionString = ConnectionString;

            _tableName = Table == null ? string.Empty : Table;

            sqlConn = new SQLiteConnection(_connectionString);
            sqlConn.Open();
        }
        #region Row Commands
        public bool AddSingleRow(string[] Columns, params object[] Values)
        {
            CheckConnection();

            string colums = string.Empty;
            string values = string.Empty;

            foreach (string col in Columns)
                colums = colums + "," + col;

            for (int i = 0; i < Values.Length; i++)
                if (Values[i].GetType() != typeof(string))
                    if (i == 0)
                        values = "'" + values + ",'" + Values[i].ToString() + "'";
                    else
                        values = values + ",'" + Values[i].ToString() + "'";
                else
                    values = values + ",'" + Values[i] + "'";

            values = values.Remove(0, 2);
            colums = colums.Remove(0, 1);

            string queryString = $"INSERT INTO {_tableName}({colums}) VALUES({values})";

            try
            {
                SQLiteCommand sqlComm = new SQLiteCommand(queryString, sqlConn);
                sqlComm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public object ReturnSingleCell(string Collumn, string Condition)
        {
            CheckConnection();

            string queryString = $"SELECT {Collumn} From {_tableName} WHERE {Condition}";

            if (Condition == string.Empty)
                queryString = queryString.Substring(0, queryString.LastIndexOf("WHERE"));

            SQLiteCommand sqlComm = new SQLiteCommand(queryString, sqlConn);
            SQLiteDataReader sqlReader = sqlComm.ExecuteReader();


            sqlReader.Read();
            return sqlReader[0];
        }
        public string[] ReturnSingleRow(string[] Collumns, string Condition = "")
        {
            CheckConnection();


            string colums = string.Empty;
            bool AllRows = Collumns[0] == "*" ? true : false;

            if (AllRows == true)
            {
                colums = "*";
            }
            else
            {

                foreach (string col in Collumns)
                    colums = colums + "," + col;

                colums = colums.Remove(0, 1);

            }

            string queryString = $"SELECT {colums} From {_tableName} " + (Condition != string.Empty ? $"WHERE {Condition}" : string.Empty);

            //if (Condition == string.Empty)
            //    queryString = queryString.Substring(0, queryString.LastIndexOf("WHERE"));

            SQLiteCommand sqlComm = new SQLiteCommand(queryString, sqlConn);
            SQLiteDataReader sqlReader = sqlComm.ExecuteReader();
            List<string> list = new List<string>();

            while (sqlReader.Read())
            {

                for (int i = 0; i < sqlReader.FieldCount; i++)
                {
                    list.Add(Convert.ToString(sqlReader[i]));
                }
            }
            sqlReader.Close();
            return list.ToArray();
        }
        public bool DeleteSingleRow(string Condition)
        {
            try
            {

                string queryString = $"DELETE FROM {_tableName} WHERE {Condition}";


                if (Condition == string.Empty)
                    queryString = queryString.Substring(0, queryString.LastIndexOf("WHERE"));


                SQLiteCommand sqlComm = new SQLiteCommand(queryString, sqlConn);
                sqlComm.ExecuteNonQuery();

            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                return false;
            }
            return true;
        }
        public bool RowIsExist(string Condition)
        {
            CheckConnection();

            string queryString = $"SELECT COUNT(Id) From {_tableName} WHERE {Condition}";

            if (Condition == string.Empty)
                queryString = queryString.Substring(0, queryString.LastIndexOf("WHERE"));

            try
            {
                SQLiteCommand sqlComm = new SQLiteCommand(queryString, sqlConn);
                SQLiteDataReader sqlReader = sqlComm.ExecuteReader();
                sqlReader.Read();
                int val = sqlReader.GetInt32(0);
                sqlReader.Close();
                return val == 1 ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ConditionIsMet(string condition)
        {
            CheckConnection();

            string queryString = $"SELECT COUNT(Id) From {_tableName} {condition}";

            if (condition == string.Empty)
                queryString = queryString.Substring(0, queryString.LastIndexOf("WHERE"));

            try
            {
                SQLiteCommand sqlComm = new SQLiteCommand(queryString, sqlConn);
                SQLiteDataReader sqlReader = sqlComm.ExecuteReader();
                sqlReader.Read();
                int val = sqlReader.GetInt32(0);
                sqlReader.Close();
                return val == 1 ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public int UsersCount(string Collumn)
        {
            CheckConnection();
            int val;
            string queryString = $"SELECT COUNT({Collumn}) From {_tableName}";



            try
            {
                SQLiteCommand sqlComm = new SQLiteCommand(queryString, sqlConn);
                SQLiteDataReader sqlReader = sqlComm.ExecuteReader();
                sqlReader.Read();
                val = (int)sqlReader[0];
                sqlReader.Close();
            }
            catch (Exception)
            {
                return 0;
            }

            return val;
        }
        public bool RunSqlQuery(string query)
        {
            try
            {
                SQLiteCommand sqlComm = new SQLiteCommand(query, sqlConn);
                SQLiteDataReader sqlReader = sqlComm.ExecuteReader();
                sqlReader.Read();
                int val = sqlReader.GetInt32(0);
                sqlReader.Close();
                return val == 1 ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        #region General

        public bool TableIsExist()
        {
            CheckConnection();
            bool exists = false;
            try
            {
                var cmd = new SQLiteCommand("select case when exists((select * from information_schema.tables where table_name = '" + _tableName + "')) then 1 else 0 end", sqlConn);

                return exists = (int)cmd.ExecuteScalar() == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void CheckConnection()
        {
            if (!Opened)
                sqlConn.Open();
        }
        public bool CreateTable(string TableName, params string[] CollumnParams)
        {
            try
            {
                string collumnString = "(";

                for (int i = 0; i < CollumnParams.Length; i += 2)
                {
                    collumnString = collumnString + CollumnParams[i] + " " + CollumnParams[i + 1] + ",";
                }

                collumnString = collumnString.Substring(0, collumnString.Length - 1) + ")";

                SQLiteCommand cmd = new SQLiteCommand($"create table {TableName}{collumnString}", sqlConn);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Table Created Successfully...");
            }
            catch (Exception e)
            {
                Console.WriteLine("exception occured while creating table:" + e.Message + "\t" + e.GetType());
            }
            return true;
        }
        #endregion
        #region Statics
        public static bool CreateDBObsolote(string dbName, string dbFileName)
        {
            SQLiteConnection connection = new SQLiteConnection(@"server=(localdb\MSSQLLocalDB");
            using (connection)
            {
                connection.Open();

                string sql = string.Format(@"CREATE DATABASE {0} ON (NAME = N'{0}', FILENAME = '{1}')", dbName, dbFileName
                );

                SQLiteCommand command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
            }

            return true;
        }
        public static bool CreateDB(string dbPath, DatabaseCreationBlock dbCreationBlock)
        {
            SQLiteConnection.CreateFile(dbPath);

            //if (File.Exists(dbPath))
            //    throw new Exception("File cannot created");

            SQLiteConnection m_dbConnection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            m_dbConnection.Open();

            string collumnString = "(";

            for (int i = 0; i < dbCreationBlock.TableParams.Length; i += 2)
            {
                collumnString = collumnString + dbCreationBlock.TableParams[i] + " " + dbCreationBlock.TableParams[i + 1] + ",";
            }

            collumnString = collumnString.Substring(0, collumnString.Length - 1) + ")";


            SQLiteCommand cmd = new SQLiteCommand($"create table {dbCreationBlock.TableName}{collumnString}", m_dbConnection);
            cmd.ExecuteNonQuery();

            m_dbConnection.Close();
            return true;
        }
        public static bool CheckDatabaseExists(string databaseName)
        {
            string sqlCreateDBQuery;
            bool result = false;
            SQLiteConnection tmpConn;
            try
            {
                tmpConn = new SQLiteConnection(@"server=(localdb)\MSSQLLocalDB;Integrated Security=True");

                sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", databaseName);

                using (tmpConn)
                {
                    using (SQLiteCommand sqlCmd = new SQLiteCommand(sqlCreateDBQuery, tmpConn))
                    {
                        tmpConn.Open();

                        object resultObj = sqlCmd.ExecuteScalar();

                        int databaseID = 0;

                        if (resultObj != null)
                        {
                            int.TryParse(resultObj.ToString(), out databaseID);
                        }

                        tmpConn.Close();

                        result = (databaseID > 0);
                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                result = false;
            }

            return result;
        }
        public static bool DetachDatabase(string dbName)
        {
            try
            {
                string connectionString = String.Format(@"server=(localdb)\MSSQLLocalDB;Integrated Security=True");
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandText = String.Format("DROP DATABASE {0}", dbName);
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

    }
}
