using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

namespace Serialization
{
    static class DataAdapter
    {
        //your connection string goes here, don't copy this one
        static SqlConnection oConn = new SqlConnection("Data Source=stusql.ckwia8qkgyyj.us-east-1.rds.amazonaws.com;Initial Catalog=YourDatabase;User ID=YourUsername;Password=YourPassword");

        public static void Connect()
        {
            try
            {
                oConn.Open();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void Disconnect()
        {
            try
            {
                oConn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// This method inserts data into the database.  The name of the table it uses is Records.  
        /// Records has two columns, an id and Data.  Data is designed to hold a serialized blob
        /// </summary>
        /// <param name="data">A blob of serialized data</param>
        public static void Insert(string data)
        {
            string sqlIns = "INSERT INTO Records(Data) VALUES(@data)";

            try
            {
                Connect();

                SqlCommand cmdIns = new SqlCommand(sqlIns, oConn);
                cmdIns.Parameters.AddWithValue("@data", data);

                cmdIns.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// This method retrieves all serialized data from the database table Records.
        /// </summary>
        public static void Get()
        {
            string sql = "SELECT * FROM Records";

            SqlCommand cmd = new SqlCommand(sql, oConn);

            try
            {
                Connect();

                SqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    //while we are getting records from the DB
                    //we need to be deserializing them and
                    //creating objects
                    Serializer.DeSerializeNow(reader["Data"].ToString());
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }

        }
    }
}
