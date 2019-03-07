using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace TheGreatGroupModules.Modules
{

    public class DBHelper
    {
        private static string szDbUser = "admin";
        private static string szDbPassword = "password";

        public static MySqlConnection ConnectDb(ref string errMsg)
        {
            string connStrFmt = "Data Source={0}; Initial Catalog={1};User ID={2}; Password={3};";
            string connString = string.Format(connStrFmt, ConfigurationManager.AppSettings["DBServer"], ConfigurationManager.AppSettings["DBName"], szDbUser, szDbPassword);
            try
            {
                var conn = new MySqlConnection(connString);
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                errMsg = ex.ToString();
                return null;
            }
        }

        public static DataTable List(string query, MySqlConnection conn)
        {
            var dt = new DataTable();
            var dataAdapter = new MySqlDataAdapter(query, conn);
            dataAdapter.SelectCommand.CommandTimeout = 0;
            dataAdapter.Fill(dt);
            return dt;
        }

        public static DataTable List(string query, MySqlConnection conn, MySqlTransaction sqlTransaction)
        {
            var dt = new DataTable();
            var sqlCommand = new MySqlCommand(query, conn, sqlTransaction);
            var dataAdapter = new MySqlDataAdapter(sqlCommand);
            dataAdapter.SelectCommand.CommandTimeout = 0;
            dataAdapter.Fill(dt);
            return dt;
        }

        public static int Execute(string query, MySqlConnection conn)
        {
            int i = 0;
            var sqlCommand = new MySqlCommand(query, conn);
            i = sqlCommand.ExecuteNonQuery();
            return (i > 0) ? i : 0;
        }

        public static int Execute(string query, MySqlConnection conn, MySqlTransaction sqlTransaction)
        {
            int i = 0;
            var sqlCommand = new MySqlCommand(query, conn, sqlTransaction);
            i = sqlCommand.ExecuteNonQuery();
            return (i > 0) ? i : 0;
        }

        public static void Close(MySqlConnection conn)
        {

            conn.Close();
        }
    }
}