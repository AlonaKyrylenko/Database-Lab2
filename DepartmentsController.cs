using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Controllers
{
    public class DepartmentsController : BaseController
    {
        public DepartmentsController(string connectionString) : base(connectionString) { }
        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select department_id, name, good_id, availability from departments";

            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Department Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Name: {0}", rdr.GetValue(1));
                    Console.WriteLine("Good Id: {0}", rdr.GetValue(2));
                    Console.WriteLine("Availability: {0}", rdr.GetValue(3));
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
            string sqlInsert = "Insert into departments(name, good_id, availability) VALUES(@name, @good_id, @availability)";

            string name = null;
            int good_id = 0;
            int availability = 0;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter Departments properties:");
                Console.WriteLine("Name:");
                name = Console.ReadLine();
                if (name.Length > 40)
                {
                    correct = false;
                    Console.WriteLine("Length of name > 40. It is wrong.");
                    Console.ReadLine();
                    continue;
                }

                Console.WriteLine("Good id:");
                correct = Int32.TryParse(Console.ReadLine(), out good_id);
                if (correct == false)
                {
                    Console.WriteLine("Good id must be a number!");
                    Console.ReadLine();
                }

                Console.WriteLine("Availability:");
                correct = Int32.TryParse(Console.ReadLine(), out availability);
                if (correct == false)
                {
                    Console.WriteLine("Availability must be a number!");
                    Console.ReadLine();
                }

                correct = true;
            } while (correct == false);


            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("good_id", good_id);
            cmd.Parameters.AddWithValue("availability", availability);
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
            base.Delete("Delete from departments where department_id = ");
        }
        public override void Update()
        {
            base.Update("Update departments ");
        }
        public override void Find()
        {
            base.Find();
        }
        public override void Generate()
        {
            Console.WriteLine("How many records do you want?");
            bool correct = false;
            int recordsAmount;

            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);

            string sqlGenerate = "Insert into departments(name, good_id, availability) (select "
                + base.sqlRandomString 
                + ", "
                + base.sqlRandomInteger
                + ", "
                + base.sqlRandomInteger
                + " from generate_series(1, 1000000)  limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }
    }
}
