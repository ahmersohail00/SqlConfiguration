using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SqlConfiguration
{
    public class SqlData : Connection
    {
        public static (string, bool) ConnectionName { get; set; }

        public static DataTable GetData(string QueryString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ConnectionName.Item1))
                    throw new NullReferenceException();
                Connect(ConnectionName);
                conn = new SqlConnection(ConnectionString);
                SqlCommand Cmd = new SqlCommand(QueryString, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                return GetData(Cmd);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DataTable GetData(string QueryString, Dictionary<string, object> Data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ConnectionName.Item1))
                    throw new NullReferenceException();
                Connect(ConnectionName);
                conn = new SqlConnection(ConnectionString);
                SqlCommand Cmd = new SqlCommand(QueryString, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                foreach (var item in Data)
                {
                    Cmd.Parameters.AddWithValue("@" + item.Key, item.Value);
                }
                return GetData(Cmd);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int ModifyData(string QueryString, Dictionary<string, object> Data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ConnectionName.Item1))
                    throw new NullReferenceException();
                Connect(ConnectionName);
                conn = new SqlConnection(ConnectionString);
                SqlCommand Cmd = new SqlCommand(QueryString, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                foreach (var item in Data)
                {
                    Cmd.Parameters.AddWithValue("@" + item.Key, item.Value);
                }
                return ModifyData(Cmd);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int ModifyData(string QueryString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ConnectionName.Item1))
                    throw new NullReferenceException();
                Connect(ConnectionName);
                conn = new SqlConnection(ConnectionString);
                SqlCommand Cmd = new SqlCommand(QueryString, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                return ModifyData(Cmd);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
