using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Controllers
{
    public class CategoriesController : BaseController
    {
        public CategoriesController(string connectionString) : base(connectionString) { }
        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select category_id, name, department_id from categories";

            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Category Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Name: {0}", rdr.GetValue(1));
                    Console.WriteLine("Department Id: {0}", rdr.GetValue(2));
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
            string sqlInsert = "Insert into categories(name, department_id) VALUES(@name, @department_id)";

            string name = null;
            int department_id = 0;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter Categories properties:");
                Console.WriteLine("Name:");
                name = Console.ReadLine();
                if (name.Length > 40)
                {
                    correct = false;
                    Console.WriteLine("Length of name > 40. It is wrong.");
                    Console.ReadLine();
                    continue;
                }

                Console.WriteLine("Department Id:");
                correct = Int32.TryParse(Console.ReadLine(), out department_id);
                if(correct == false)
                {
                    Console.WriteLine("Department id must be a number!");
                    Console.ReadLine();
                }
               
                correct = true;
            } while (correct == false);

            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("department_id", department_id);
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
            base.Delete("Delete from categories where category_id = ");
        }
        public override void Update()
        {
            base.Update("Update categories ");
        }
        public override void Generate()
        {
            Console.WriteLine("How many records do you want?");
            bool correct = false;
            int recordsAmount;

            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);

            string sqlGenerate = "Insert into categories(name, department_id) (select "
                + base.sqlRandomString
                + ", "
                + base.sqlRandomInteger
                + " from generate_series(1, 1000000)  limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }
    }
}
