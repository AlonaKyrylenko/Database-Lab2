using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Controllers
{
    public class OrdersController : BaseController
    {
        public OrdersController(string connectionString) : base(connectionString) { }
        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select order_id, customer_id, data, good_id from orders";

            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Order Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Good Id: {0}", rdr.GetValue(1));
                    Console.WriteLine("Data of the order: {0}", rdr.GetValue(2));
                    Console.WriteLine("Customer Id: {0}", rdr.GetValue(3));
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
            string sqlInsert = "Insert into orders (customer_id, data of the order, good_id) VALUES(@customer_id, @data, @good_id)";

            int customer_id = 0;
            DateTime data = new DateTime(2022, 12, 22);
            int good_id = 0;
            
            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter customer goods properties:");
                Console.WriteLine("Customer id:");
                correct = Int32.TryParse(Console.ReadLine(), out customer_id);
                if (correct == false)
                {
                    Console.WriteLine("Customer id must be a number!");
                    Console.ReadLine();
                }

                Console.WriteLine("Data of the order:");
                Console.WriteLine(data);

                Console.WriteLine("Goods id:");
                correct = Int32.TryParse(Console.ReadLine(), out good_id);
                if (correct == false)
                {
                    Console.WriteLine("Goods id must be a number!");
                    Console.ReadLine();
                }

                correct = true;
            } while (correct == false);

            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("customer_id", customer_id);
            cmd.Parameters.AddWithValue("data", data);
            cmd.Parameters.AddWithValue("good_id", good_id);
            cmd.Prepare();

            try
            {
                cmd.ExecuteNonQuery();
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
        }
        public override void Delete()
        {
            base.Delete("Delete from orders where order_id = ");
        }
        public override void Update()
        {
            base.Update("Update orders ");
        }
        public override void Generate()
        {
            Console.WriteLine("How many records do you want?");
            bool correct = false;
            int recordsAmount;

            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);
            
            string sqlGenerate = "Insert into orders(customer_id, data, good_id) (select "
                + base.sqlRandomInteger
                + ", "
                + base.sqlRandomDate
                + ", "
                + base.sqlRandomInteger
                + " from generate_series(1, 1000000)  limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }   
    }
}
