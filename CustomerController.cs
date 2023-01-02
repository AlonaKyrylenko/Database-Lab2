using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Controllers
{
    public class CustomerController : BaseController
    {
        public CustomerController(string connectionString) : base(connectionString) { }
        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select customer_id, name, order_id from customer";

            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Customer Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Name: {0}", rdr.GetValue(1));
                    Console.WriteLine("Order Id: {0}", rdr.GetValue(2));
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            finally
            {
                sqlConnection.Close();
            }

            Console.ReadLine();
        }
        public override void Create()
        {
            string sqlInsert = "Insert into customer(name, order_id) VALUES(@name, @order_id)";

            string name = null;
            int order_id = 0;
            
            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter customer properties:");
                Console.WriteLine("Name:");
                name = Console.ReadLine();
                if (name.Length > 40)
                {
                    correct = false;
                    Console.WriteLine("Length of name > 40. It is wrong.");
                    Console.ReadLine();
                    continue;
                }

                Console.WriteLine("Order id:");
                correct = Int32.TryParse(Console.ReadLine(), out order_id);
                if (correct == false)
                {
                    Console.WriteLine("Order id must be a number!");
                    Console.ReadLine();
                }
                correct = true;
            } while (correct == false);

            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("order_id", order_id);
            cmd.Prepare();

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"New recorder can't be created: {ex.Message}");
                Console.ReadLine();
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        public override void Delete()
        {
            base.Delete("Delete from customer where customer_id = ");
        }
        public override void Update()
        {
            base.Update("Update customer ");
        }
        public override void Generate()
        {
            Console.WriteLine("How many records do you want?");
            bool correct = false;
            int recordsAmount;

            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);

            string sqlGenerate = "Insert into customer(name, order_id) (select "
                + base.sqlRandomString
                + ", "
                + base.sqlRandomInteger
                + " from generate_series(1, 1000000)  limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }
    }
}
