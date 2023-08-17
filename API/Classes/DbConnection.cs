using MySqlConnector;
using System.Reflection.PortableExecutable;

namespace API.Classes
{
    public class DbConnection
    {
        string host = "localhost";
        string dbname = "apinexum";
        string user = "root";
        string password = "";

        MySqlConnection conn;

        public void SqlConnect()
        { 
            conn = new MySqlConnection($"Server={host};User ID={user};Password={password};Database={dbname}");
        }
        public async Task<bool> SqlQueryAsync(string _sqlCommand)
        {
            try
            {
                SqlConnect();
                await conn.OpenAsync();

                MySqlCommand commandSql = new MySqlCommand(_sqlCommand, conn);
                MySqlDataReader commReader = await commandSql.ExecuteReaderAsync();
                await conn.CloseAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<Person>> SqlSelectAsync(string _sqlSelect)
        {
            SqlConnect();
            await conn.OpenAsync();

            MySqlCommand commandSql = new MySqlCommand(_sqlSelect, conn);
            MySqlDataReader commReader = await commandSql.ExecuteReaderAsync();

            List< Person > persons = new List< Person >();
            while (await commReader.ReadAsync())
            {
                Object[] values = new Object[commReader.FieldCount];
                commReader.GetValues(values);
                try
                {
                    Person selectedPerson = new Person()
                    {
                        Id = (int)values[0],
                        Name = (string)values[1],
                        Email = (string)values[2],
                        Entity = (EntityType)values[3],
                        IdNumber = (string)values[4],
                        ZIPCode = (string)values[5],
                        Address = (string)values[6],
                        PublicArea = (string)values[7],
                        District = (string)values[8],
                        City = (string)values[9],
                        State = (string)values[10]
                    };
                    persons.Add(selectedPerson);
                }
                catch
                {
                    continue;
                }
            }
            await conn.CloseAsync();
            return persons;
        }
    }
}
