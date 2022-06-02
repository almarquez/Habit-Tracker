using System.Data.SQLite;

string cs = @"Data Source=habit-tracker.db";

CreateDatabase();

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