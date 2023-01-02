using lab2.Controllers;
using System;

namespace lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Host=localhost; Username=postgres; Password=1234; Database=KPI";


            int table = 0;
            int action = 0;
            do
            {
                table = FirstMenu();
                if (table == 0)
                {
                    return;
                }

                BaseController controller = null;

                switch (table)
                {
                    case 1:
                        action = SecondMenu("Customer");
                        controller = new CustomerController(connectionString);
                        break;
                    case 2:
                        action = SecondMenu("Orders");
                        controller = new OrdersController(connectionString);
                        break;
                    case 3:
                        action = SecondMenu("Departments");
                        controller = new DepartmentsController(connectionString);
                        break;
                    case 4:
                        action = SecondMenu("categories");
                        controller = new CategoriesController(connectionString);
                        break;
                    case 5:
                        action = SecondMenu("Goods");
                        controller = new GoodsController(connectionString);
                        break;
                }

                switch (action)
                {
                    case 1:
                        controller.Read();
                        break;
                    case 2:
                        controller.Create();
                        break;
                    case 3:
                        controller.Update();
                        break;
                    case 4:
                        controller.Delete();
                        break;
                    case 5:
                        controller.Generate();
                        break;
                    case 6:
                        controller.Find();
                        break;
                }

            } while (true);
        }

        public static int FirstMenu()
        {
            var choice = 0;
            var correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Choose the table you want to manipulate with:");
                Console.WriteLine("Enter table number in range 1-5 or 0 to exit:");
                Console.WriteLine("1.Customer");
                Console.WriteLine("2.Orders");
                Console.WriteLine("3.Departments");
                Console.WriteLine("4.Categories");
                Console.WriteLine("5.Goods");
                correct = Int32.TryParse(Console.ReadLine(), out choice);
            } while (choice < 0 || choice > 5 || correct == false);

            return choice;
        }
        public static int SecondMenu(string tableToChange)
        {
            var choice = 0;
            var correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Choose what you want to do with '" + tableToChange + "' table:");
                Console.WriteLine("Enter number in range 1-6 or 0 to exit:");
                Console.WriteLine("1.Read");
                Console.WriteLine("2.Create");
                Console.WriteLine("3.Update");
                Console.WriteLine("4.Delete");
                Console.WriteLine("5.Generate");
                Console.WriteLine("6.Find");
                correct = Int32.TryParse(Console.ReadLine(), out choice);
            } while (choice < 0 || choice > 6 || correct == false);

            return choice;
        }
    }
}
