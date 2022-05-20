using Common;
using System;
using System.Data.SqlClient;

namespace Service
{
    public class TestService
    {

        public static void TestConnection()
        {
            try
            {
                using var context = new SqlConnection(Parameters.ConnectionString);
                context.Open();
                Console.WriteLine("Connection Success");

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Sql Server error: {ex.Message}");
            }
        }
    }
}
