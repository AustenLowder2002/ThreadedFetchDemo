using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

public class DatabaseConnector
{

    public static void Database() {
        MySqlCommand myCommand = new();
        myCommand.CommandTimeout = 60;
        MySqlConnection myConnection;
        var sb = new MySqlConnectionStringBuilder()
        {
            Server = "localhost",
            Port = 3306,
            UserID = "root",
            Password = "12385",
            Database = "test"
        };
        myConnection = new MySqlConnection(sb.ConnectionString);

        try {
            myConnection.Open();
            DataTable table = myConnection.GetSchema("Tables");
            DisplayData(table);
            // Based on documentaion for dotnet mysql connector
            myCommand.Connection = myConnection;
            myCommand.CommandText = "DROP PROCEDURE IF EXISTS add_emp";
            myCommand.ExecuteNonQuery();
            myCommand.CommandText = "DROP TABLE IF EXISTS emp";
            myCommand.ExecuteNonQuery();
            myCommand.CommandText = "CREATE TABLE emp (" +
            "empno INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY," +
            "first_name VARCHAR(20), last_name VARCHAR(20), birthdate DATE)";
            myCommand.ExecuteNonQuery();
        }
        catch (MySqlException ex) {
            switch (ex.Number) {
                case 0:
                Console.WriteLine("Cannot connect to server. Contact administrator");
                break;
                case 1045:
                Console.WriteLine("Invalid username/password, please try again");
                break;
            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
        }
        finally {
            myConnection.Close();
        }
    }

    private static void DisplayData(DataTable table) {
        foreach (DataRow row in table.Rows) {
            foreach (DataColumn col in table.Columns) {
                Console.WriteLine("{0} {1}", col.ColumnName, row[col]);
            }
            Console.WriteLine("==============================");
        }
    }
}