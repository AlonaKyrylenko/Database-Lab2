using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Controllers
{
    public abstract class BaseController
    {
        public string connectionString;
        protected NpgsqlConnection sqlConnection;

        string fieldToFind = null;
        string valueToFind = null;
        string fieldToSet = null;
        string valueToSet = null;

        string[] fieldsToFind = new string[10];
        string[] valuesToFind = new string[10];

        public readonly string sqlUpdate = "Update @table set @field_to_update = @new_value where @field_to_find = @old_value";
        public readonly string sqlRandomString = "chr(trunc(65 + random() * 50)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int)";
        public readonly string sqlRandomInteger = "trunc(random()*1000)::int";
        public readonly string sqlRandomDate = "timestamp '2022-12-01 20:00:00' + random() * (timestamp '2022-12-31 20:00:00' - timestamp '2022-12-01 10:00:00')";
        public readonly string sqlRandomBoolean = "trunc(random()*2)::int::boolean";

        public BaseController(string connectionString)
        {
            this.connectionString = connectionString;
            this.sqlConnection = new NpgsqlConnection(connectionString);
        }

        public void Read()
        {
            Read("");
        }
        public virtual void Create()
        {
            throw new NotImplementedException();
        }
        public virtual void Update()
        {
            throw new NotImplementedException();
        }
        public virtual void Delete()
        {
            throw new NotImplementedException();
        }
        virtual public void Generate()
        {
            throw new NotImplementedException();
        }
        public virtual void Find()
        {
            Console.Clear();
            int actualSize = 0;
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Enter field you want to find with");
                fieldsToFind[i] = Console.ReadLine();
                Console.WriteLine("Enter value you want find");
                valuesToFind[i] = Console.ReadLine();
                Console.WriteLine("Enter 1 to continue");
                actualSize++;

                int choose = 0;
                bool correct = Int32.TryParse(Console.ReadLine(), out choose);
                if (correct = false || choose != 1)
                {
                    break;
                }
            }

            string whereCondition = " where ";

            int parseInt;
            if (Int32.TryParse(valuesToFind[0], out parseInt) == false)
            {
                valuesToFind[0] = "'" + valuesToFind[0] + "'";
            }
            whereCondition +=  fieldsToFind[0] + " = " + valuesToFind[0];

            for (int i = 1; i < actualSize; i++)
            {
                if (Int32.TryParse(valuesToFind[i], out parseInt) == false)
                {
                    valuesToFind[i] = "'" + valuesToFind[i] + "'";
                }
                whereCondition += " and " + fieldsToFind[i] + " = " + valuesToFind[i];
            }

            Read(whereCondition);
        }
        virtual public void Read(string whereCondition)
        {

        }
        protected void Delete(string sqlDelete)
        {
            bool correct = false;
            int id = 0;
            do
            {
                Console.WriteLine("Enter number of record you want to delete (or 0 to step back):");
                correct = Int32.TryParse(Console.ReadLine(), out id);
                if (correct == false)
                {
                    Console.WriteLine("Id must be a number...");
                    Console.ReadLine();
                    continue;
                }
            } while (correct == false || id < 0);

            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlDelete + id, sqlConnection);

            try
            {
                cmd.Prepare();
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
        private void Update(string table, string field_to_update, string new_value, string field_to_find, string old_value)
        {
            sqlConnection.Open();

            StringBuilder updateString = new StringBuilder("Update", 200);

            int new_int;
            if (!Int32.TryParse(new_value, out new_int))
            {
                new_value = "'" + new_value + "'";
            }
            if (!Int32.TryParse(old_value, out new_int))
            {
                old_value = "'" + old_value + "'";
            }

            updateString.AppendFormat(" {0} set {1} = {2} where {3} = {4}", table, field_to_update, new_value, field_to_find, old_value);

            using var cmd = new NpgsqlCommand(updateString.ToString(), sqlConnection);

            try
            {
                cmd.Prepare();
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
        protected void Update(string sqlUpdate)
        {

            Console.Clear();
            Console.WriteLine("Enter name of field you want to find:");
            fieldToFind = Console.ReadLine();
            Console.WriteLine("Enter value in this field you want to find:");
            valueToFind = Console.ReadLine();

            Console.WriteLine("Enter name of field you want to change:");
            fieldToSet = Console.ReadLine();
            Console.WriteLine("Enter new value in this field");
            valueToSet = Console.ReadLine();

            int ParseInt = 0;
            if (Int32.TryParse(valueToFind, out ParseInt) == false)
            {
                valueToFind = "'" + valueToFind + "'";
            }
            if (Int32.TryParse(valueToSet, out ParseInt) == false)
            {
                valueToSet = "'" + valueToSet + "'";
            }

            string sqlQuery = sqlUpdate + "set " + fieldToSet + " = " + valueToSet + " where " + fieldToFind + " = " + valueToFind;

            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlQuery, sqlConnection);

            try
            {
                cmd.Prepare();
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
        protected void Generate(string sqlGenerate)
        {
            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlGenerate, sqlConnection);

            try
            {
                cmd.Prepare();
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
    }
}
