//using System;
//using System.Configuration;
//using System.Data;
//using System.Data.Common;
//using System.Data.SQLite;
//using System.IO;

//namespace Small.Data.Core.Dat
//{
//    public class Db
//    {
//        private static readonly string dataProvider = ConfigurationManager.AppSettings.Get("DataProvider");
//        private static readonly DbProviderFactory factory = DbProviderFactories.GetFactory(dataProvider);

//        private static readonly string connectionStringName = ConfigurationManager.AppSettings.Get("ConnectionStringName");
//        private static readonly string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

//        public static void CreateDatabase()
//        {
//            // Check if the database exists
//            string conStrPath = connectionString.Split(';')[0].Replace("Data Source=", "");
//            SQLiteConnection connection = new SQLiteConnection();

//            if (!File.Exists(conStrPath))
//            {
//                try
//                {
//                    System.Data.SQLite.SQLiteConnection.CreateFile(connection.DataSource);
//                    connection.ConnectionString = connectionString;
//                    connection.Open();
//                    CreateTable();
//                    connection.Close();
//                }
//                catch (Exception ex)
//                {

//                }
//                finally
//                {
//                    if (connection.State != ConnectionState.Closed)
//                    {
//                        connection.Close();
//                    }
//                }
//            }
//            else
//            {
//                try
//                {
//                    connection.ConnectionString = connectionString;
//                    connection.Open();
//                    CreateTable();
//                    connection.Close();
//                }
//                catch (Exception ex)
//                {
//                }
//                finally
//                {
//                    if (connection.State != ConnectionState.Closed)
//                    {
//                        connection.Close();
//                    }
//                }
//            }
//        }

//        private static void CreateTable()
//        {
//            // See if the table exists
//            object lok = GetScalar("select * from sqlite_master where type = 'table' and name = 'Movie';");
//            if (lok == null)
//            {
//                // CURRENT_TIMESTAMP doesn't store milleseconds, therefore it has been changes to the 'strftime' function.
//                string tableStr = "create table Movie (movie_pk integer primary key autoincrement, movie_date datetime, " +
//                     " movie_title text, movie_desc text, movie_genre text, when_created datetime default (strftime('%Y-%m-%d %H:%M:%f', 'now')), " +
//                     " is_deleted bool default (0), user_modified int default (0));";

//                Insert(tableStr, false);
//            }

//            lok = GetScalar("select * from sqlite_master where type = 'table' and name = 'SimpleNote';");
//            if (lok == null)
//            {
//                // CURRENT_TIMESTAMP doesn't store milleseconds, therefore it has been changes to the 'strftime' function.
//                string tableStr = "create table SimpleNote (note_pk integer primary key autoincrement, note_name text, note_detail text, note_category text, " +
//                     " when_created datetime default (strftime('%Y-%m-%d %H:%M:%f', 'now')), is_deleted bool default (0), user_modified int default (0));";

//                Insert(tableStr, false);
//            }
//        }




//        /// <summary>
//        /// Executes Insert statements in the database. Optionally returns new identifier.
//        /// </summary>
//        /// <param name="sql">Sql statement.</param>
//        /// <param name="getId">Value indicating whether newly generated identity is returned.</param>
//        /// <returns>Newly generated identity value (autonumber value).</returns>
//        public static int Insert(string sql, bool getId)
//        {
//            using (DbConnection connection = factory.CreateConnection())
//            {
//                connection.ConnectionString = connectionString;

//                using (DbCommand command = factory.CreateCommand())
//                {
//                    command.Connection = connection;
//                    command.CommandText = sql;

//                    connection.Open();
//                    command.ExecuteNonQuery();

//                    int id = -1;

//                    // Check if new identity is needed.
//                    if (getId)
//                    {
//                        // Execute db specific autonumber or identity retrieval code
//                        // SELECT SCOPE_IDENTITY() -- for SQL Server
//                        // SELECT @@IDENTITY -- for MS Access
//                        // SELECT MySequence.NEXTVAL FROM DUAL -- for Oracle
//                        string identitySelect;
//                        switch (dataProvider)
//                        {
//                            // Access
//                            case "System.Data.OleDb":
//                                identitySelect = "SELECT @@IDENTITY";
//                                break;
//                            // Sql Server
//                            case "System.Data.SqlClient":
//                                identitySelect = "SELECT SCOPE_IDENTITY()";
//                                break;
//                            // Oracle
//                            case "System.Data.OracleClient":
//                                identitySelect = "SELECT MySequence.NEXTVAL FROM DUAL";
//                                break;
//                            case "System.Data.SQLite":
//                                identitySelect = "SELECT last_insert_rowid();";
//                                break;
//                            default:
//                                identitySelect = "SELECT @@IDENTITY";
//                                break;
//                        }
//                        command.CommandText = identitySelect;
//                        id = int.Parse(command.ExecuteScalar().ToString());
//                    }
//                    return id;
//                }
//            }
//        }

//        /// <summary>
//        /// Executes Insert statements in the database.
//        /// </summary>
//        /// <param name="sql">Sql statement.</param>
//        public static void Insert(string sql)
//        {
//            Insert(sql, false);
//        }

//        /// <summary>
//        /// Populates a DataTable according to a Sql statement.
//        /// </summary>
//        /// <param name="sql">Sql statement.</param>
//        /// <returns>Populated DataTable.</returns>
//        public static DataTable GetDataTable(string sql)
//        {
//            using (DbConnection connection = factory.CreateConnection())
//            {
//                connection.ConnectionString = connectionString;

//                using (DbCommand command = factory.CreateCommand())
//                {
//                    command.Connection = connection;
//                    command.CommandType = CommandType.Text;
//                    command.CommandText = sql;

//                    using (DbDataAdapter adapter = factory.CreateDataAdapter())
//                    {
//                        adapter.SelectCommand = command;

//                        DataTable dt = new DataTable();
//                        adapter.Fill(dt);

//                        return dt;
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// Populates a DataRow according to a Sql statement.
//        /// </summary>
//        /// <param name="sql">Sql statement.</param>
//        /// <returns>Populated DataRow.</returns>
//        public static DataRow GetDataRow(string sql)
//        {
//            DataRow row = null;

//            DataTable dt = GetDataTable(sql);
//            if (dt.Rows.Count > 0)
//            {
//                row = dt.Rows[0];
//            }

//            return row;
//        }

//        /// <summary>
//        /// Executes a Sql statement and returns a scalar value.
//        /// </summary>
//        /// <param name="sql">Sql statement.</param>
//        /// <returns>Scalar value.</returns>
//        public static object GetScalar(string sql)
//        {
//            using (DbConnection connection = factory.CreateConnection())
//            {
//                connection.ConnectionString = connectionString;

//                using (DbCommand command = factory.CreateCommand())
//                {
//                    command.Connection = connection;
//                    command.CommandText = sql;

//                    connection.Open();
//                    return command.ExecuteScalar();
//                }
//            }
//        }

//    }
//}
