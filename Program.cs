using System.Data.SQLite;

string cs = @"Data Source=habit-tracker.db";

CreateDatabase();
GetUserInput();

void CreateDatabase()
{
    using (var connection = new SQLiteConnection(cs))
    {
        using (var tableCmd = connection.CreateCommand())
        {
            connection.Open();

            tableCmd.CommandText = 
                @"CREATE TABLE IF NOT EXISTS habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTEGER
                    )";

            tableCmd.ExecuteNonQuery();
        }
    }
}

void GetUserInput()
{
    Console.Clear();
    bool closeApp = false;
    while(!closeApp)
    {
        Console.WriteLine("\n\nMain Menu");
        Console.WriteLine("What would you like to do?");
        Console.WriteLine("0 = Close Application");
        Console.WriteLine("1 = Get all habits");
        Console.WriteLine("2 = Insert habit");
        Console.WriteLine("3 = Delete habit");
        Console.WriteLine("4 = Update habit");
        Console.WriteLine("--------------------------\n");

        string command = Console.ReadLine();

        switch (command)
        {
            case "0": closeApp = true; break;
            case "1": GetAllHabits(); break;
            case "2": Insert(); break;
            case "3": Delete(); break;
            case "4": Update(); break;
            default: Console.WriteLine("Invalid command. Please type a number between 0-4."); break;
        }

        void GetAllHabits()
        {
            using var connection = new SQLiteConnection(cs);
            connection.Open();

            string stm = "SELECT * FROM habits";
            using var cmd = new SQLiteCommand(stm, connection);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Console.WriteLine($"ID: {rdr.GetInt32(0)} Date: {rdr.GetString(1)} Quantity: {rdr.GetInt32(2)}");
            }
        }

        void Insert()
        {
            Console.WriteLine("Please enter the date mm-dd-yyyy");
            string date = Console.ReadLine();

            Console.WriteLine("Please enter quantity: ");
            int quantity = Convert.ToInt32(Console.ReadLine());

            using (var con = new SQLiteConnection(cs))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();

                    cmd.CommandText = 
                        $"INSERT INTO habits(Date, Quantity) VALUES('{date}', '{quantity}')";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        void Update()
        {
            Console.WriteLine("\nPlease type in ID you wish to update.");
            int habitId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Type in new date. Format mm-dd-yyyy");
            string newDate = Console.ReadLine();

            Console.WriteLine("Type in new quantity.");
            int newQuantity = Convert.ToInt32(Console.ReadLine());

            using (var con = new SQLiteConnection(cs))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();

                    cmd.CommandText = 
                        $"UPDATE habits SET Date = {newDate}, Quantity = {newQuantity} WHERE Id = {habitId}";
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("*****Habit has been updated.*****");
            }
        }

        void Delete()
        {
            Console.WriteLine("\nPlease type in the ID you wish to delete.\n");
            int habitId = Convert.ToInt32(Console.ReadLine());

            using (var con = new SQLiteConnection(cs))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();

                    cmd.CommandText = 
                        $"DELETE FROM habits WHERE Id = {habitId}";
                    cmd.ExecuteNonQuery();
                }
            }

            Console.WriteLine("*****Your habit was deleted.*****");
        }
    }
}