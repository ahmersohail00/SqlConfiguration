using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SqlConfiguration
{
    public class Connection
    {
        protected static (string, bool) ConnectionStringName { get; set; }
        private static string DecryptRijndael(string cipherText, string Password)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = pdb.GetBytes(32);
            alg.IV = pdb.GetBytes(16);
            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherBytes, 0, cipherBytes.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();
            return Encoding.Unicode.GetString(decryptedData);
        }

        protected static SqlConnection conn;

        protected static void Connect((string, bool) ConnectionName)
        {
            ConnectionStringName = ConnectionName;
            ConnectionString = ConnectionStringName.Item2 ? DecryptRijndael(ConfigurationManager.ConnectionStrings[ConnectionStringName.Item1].ConnectionString, "123") : ConfigurationManager.ConnectionStrings[ConnectionStringName.Item1].ConnectionString;
        }

        protected static string ConnectionString = "";
        static DataTable dt = new DataTable("ReceiveData");
        static SqlDataReader Reader;
        static SqlCommand Cmd;

        protected static DataTable GetData(SqlCommand Cmd)
        {
            try
            {
                dt = new DataTable();
                if (conn.State != ConnectionState.Broken && conn.State == ConnectionState.Closed && conn.State != ConnectionState.Connecting && conn.State != ConnectionState.Executing && conn.State != ConnectionState.Fetching && conn.State != ConnectionState.Open)
                    conn.Open();
                Reader = Cmd.ExecuteReader();
                dt.Load(Reader);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (conn.State != ConnectionState.Broken && conn.State != ConnectionState.Closed && conn.State != ConnectionState.Connecting && conn.State != ConnectionState.Executing && conn.State != ConnectionState.Fetching && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        protected static int ModifyData(SqlCommand Data)
        {
            try
            {
                if (conn.State != ConnectionState.Broken && conn.State == ConnectionState.Closed && conn.State != ConnectionState.Connecting && conn.State != ConnectionState.Executing && conn.State != ConnectionState.Fetching && conn.State != ConnectionState.Open)
                    conn.Open();
                Cmd = Data;
                return Cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (conn.State != ConnectionState.Broken && conn.State != ConnectionState.Closed && conn.State != ConnectionState.Connecting && conn.State != ConnectionState.Executing && conn.State != ConnectionState.Fetching && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
    }
}
