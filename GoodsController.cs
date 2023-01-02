using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Controllers
{
    public class GoodsController : BaseController
    {
        public GoodsController(string connectionString) : base(connectionString) { }

        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select good_id, name, price, category_id, department_id, order_id from goods";

            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Good Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Name: {0}", rdr.GetValue(1));
                    Console.WriteLine("Price: {0}", rdr.GetValue(2));
                    Console.WriteLine("Category Id: {0}", rdr.GetValue(3));
                    Console.WriteLine("Department Id: {0}", rdr.GetValue(4));
                    Console.WriteLine("Order Id: {0}", rdr.GetValue(5));
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
            string sqlInsert = "Insert into goods (name, price, category_id, department_id, order_id) VALUES(@name, @price, @category_id, @department_id, @order_id)";

            string name = null;
            int price = 0;
            int category_id = 0;
            int department_id = 0;
            int order_id = 0;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter Goods properties:");
                Console.WriteLine("Name:");
                name = Console.ReadLine();
                if (name.Length > 40)
                {
                    correct = false;
                    Console.WriteLine("Length of name > 40. It is wrong.");
                    Console.ReadLine();
                    continue;
                }

                Console.WriteLine("Price:");
                correct = Int32.TryParse(Console.ReadLine(), out price);
                if (correct == false)
                {
                    Console.WriteLine("Price must be a number!");
                    Console.ReadLine();
                }

                Console.WriteLine("Category id:");
                correct = Int32.TryParse(Console.ReadLine(), out category_id);
                if (correct == false)
                {
                    Console.WriteLine("Category id must be a number!");
                    Console.ReadLine();
                }

                Console.WriteLine("Department id:");
                correct = Int32.TryParse(Console.ReadLine(), out department_id);
                if (correct == false)
                {
                    Console.WriteLine("Department id must be a number!");
                    Console.ReadLine();
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
            cmd.Parameters.AddWithValue("price", price);
            cmd.Parameters.AddWithValue("category_id", category_id);
            cmd.Parameters.AddWithValue("department_id", department_id);
            cmd.Parameters.AddWithValue("order_id", order_id);
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
            base.Delete("Delete from goods where good_id = ");
        }
        public override void Update()
        {
            base.Update("Update goods ");
        }
        public override void Generate()
        {
            Console.WriteLine("How many records do you want?");
            bool correct = false;
            int recordsAmount;

            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);

            string sqlGenerate = "Insert into goods(name, price, category_id, department_id, order_id) (select " 
                + base.sqlRandomString
                + ", "
                + base.sqlRandomInteger
                + ", "
                + base.sqlRandomInteger
                + ", "
                + base.sqlRandomInteger
                + ", "
                + base.sqlRandomInteger
                + " from generate_series(1, 1000000)  limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }
    }
}
