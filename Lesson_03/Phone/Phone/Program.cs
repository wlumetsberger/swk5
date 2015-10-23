using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phone
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Wolfgang\Source\Repos\SWK5\Lesson_03\Phone\PhoneTariff.mdf;Integrated Security=True;Connect Timeout=30"))
            {

                SqlCommand cmd = new SqlCommand("select * from Tariff", connection);

                connection.Open();
                
                using(var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader["Name"]);
                    }
                }

            }//conn.Dispose() --> conn.Close();
            Console.ReadKey();
        }
    }
}
