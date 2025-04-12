using MySql.Data.MySqlClient;

namespace pizza_ordering_app
{
    public static class DatabaseHelper
    {
        private static readonly string ConnectionString =
            "server=sql12.freesqldatabase.com;user id=sql12772758;password=ZfPWDQ2dFW;database=sql12772758;";
        //private static readonly string ConnectionString = 
        //    "server=localhost;user id=root;password=;database=pizza_app;";


        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public static MySqlCommand CreateCommand(string query, MySqlConnection connection)
        {
            return new MySqlCommand(query, connection);
        }
    }
}