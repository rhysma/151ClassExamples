using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient; //needed for SQL classes
using System.Collections; //needed for ArrayList collection

namespace SqlDelete
{
    static class DataAdapter
    {
        //create a new sqlconnection object so we can connect to the DB
        //Data Source is the server STUSQL
        //Initial Catalog is the name of the database (students use student ID)
        //Integrated Security is Windows Authentication and no username/password required
        static SqlConnection oConn = new SqlConnection("Data Source=stusql.ckwia8qkgyyj.us-east-1.rds.amazonaws.com:1433 ;Initial Catalog=fordt;User ID=fordt;Password=G0dSaveTiff2117");
        static List<Employee> response;

        //method to connect to the DB
        public static void Initialize()
        {
            try
            {
                oConn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        //method to disconnect from the DB
        public static void Disconnect()
        {
            try
            {
                oConn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        //Queries the DB and returns a set of data
        public static List<Employee> Get()
        {
            response = new List<Employee>();

            //new transaction to execute the SELECT query
            //this will recieve the data from the DB
            //and place it into our ArrayList
            string sqlSelect = "SELECT * FROM Employee JOIN FullTimeEmployee ON Employee.EmpId = FullTimeEmployee.EmpId";

            //create a sqlCommand object 
            SqlCommand cmdSelect = new SqlCommand(sqlSelect, oConn);

            //Open connection
            Initialize();

            //Set up a sqlDataReader to recieve the query
            //response from the DB
            SqlDataReader readerSelect = cmdSelect.ExecuteReader();

            FullTimeEmployee myFT;

            //Read() method will read the current record and
            //advance us to the next record.  While will be false
            //when there are no more records.
            while (readerSelect.Read())
            {
                myFT = new FullTimeEmployee(readerSelect["FirstName"].ToString(), readerSelect["LastName"].ToString(), readerSelect["empId"].ToString(), Convert.ToInt32(readerSelect["Age"]),
                    readerSelect["SocialSecurityNumber"].ToString(), readerSelect["Department"].ToString(), readerSelect["Address"].ToString(), readerSelect["AccessLevel"].ToString(),
                    Convert.ToDecimal(readerSelect["Salary"]), Convert.ToBoolean(readerSelect["HasBenefits"]), Convert.ToInt32(readerSelect["VacationDays"]));

                response.Add(myFT);
            }

            Disconnect();
            return response;

        }

        public static string Delete(string empId)
        {
            string sqlDelete = "DELETE Employee FROM Employee Emp JOIN FullTimeEmployee FT ON Emp.EmpId = FT.EmpId WHERE Emp.EmpId = @id";

            //create a sqlCommand object 
            SqlCommand cmdDelete = new SqlCommand(sqlDelete, oConn);

            //add the parameter
            cmdDelete.Parameters.AddWithValue("@id", empId);

            //Open connection
            Initialize();

            try
            {
                cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                Disconnect();
            }

            return "Delete Successful";
        }
    }
}
