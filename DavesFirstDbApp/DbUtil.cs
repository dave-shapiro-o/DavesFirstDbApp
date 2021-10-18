using System;
using System.Data.SqlClient;

namespace DavesFirstDbApp
{
    class DbUtil
    {
        private static SqlConnection conn;
        private static SqlCommand cmd;
        private static SqlDataReader reader;

        private static string connectionString;
        private static string query;

        internal static void DeleteRow(int id)
        {
            using (conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    cmd = new SqlCommand ($"DELETE FROM Dave_Db_Test.dbo.HeartRates WHERE ID = {id}", conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Controller.ThingsWentWrong(e); 
                }
            }
        }

        internal static void SelectQuery(string input)
        {
            switch (input)
            {
                case "1":
                    query = $@"SELECT title_id
                                     ,title
                                     ,type
                                     ,pub_id
                                     ,price
                                     ,advance
                                     ,royalty
                                     ,ytd_sales
                                     ,notes
                                     ,pubdate
                              FROM pubs.dbo.titles";
                    break;
                case "2":
                    query = @"SELECT ID
                                    ,FirstName
                                    ,LastName
                                    ,YearOfBirth
                              FROM Dave_Db_Test.dbo.HeartRates ";
                    break;
            }
        }
        internal static void SetConnectionString(string dataSource, string dbName)
        {
            connectionString = $@"Data Source={dataSource};Initial Catalog={dbName};Integrated Security=True";
        }

        internal static int ReturnTable(string input)
        {
            using (conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    cmd = new SqlCommand(query, conn);
                    if (input.Equals("1")) { return ReturnBooksTable(); }
                    else { return ReturnPeopleTable(); }
                }

                catch (Exception e)
                {
                    Controller.ThingsWentWrong(e);
                    return -1;
                }
            }                   
        }

        private static int ReturnPeopleTable()
        {
            using (reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        string firstName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        string lastName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        int yearOfBirth = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);

                        Person person = new Person(id, firstName, lastName, yearOfBirth);
                        Controller.PersonRetrieved(person);
                    }
                    return 0;
                }
                return -1;
            }
        }

        private static int ReturnBooksTable()
        {
            using (reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string titleId = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                        string title = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        string type = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        string pubId = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);

                        decimal price = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4);
                        decimal advance = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5);

                        Book book = new Book(titleId, title, type, pubId, price, advance);
                        Controller.BookRetrieved(book);
                    }
                    return 0;
                }
                return -1;
            }
        }

        internal static void AddRow(string first, string last, int year)
        {
            using (conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    cmd = new SqlCommand("INSERT INTO Dave_Db_Test.dbo.HeartRates (FirstName, LastName, YearOfBirth) " +
                                            "VALUES (@FirstName, @LastName, @YearOfBirth)", conn);

                    cmd.Parameters.AddWithValue("@FirstName", first);
                    cmd.Parameters.AddWithValue("@LastName", last);
                    cmd.Parameters.AddWithValue("@YearOfBirth", year);
                    cmd.ExecuteNonQuery();
                }

                catch (Exception e)
                {
                    Controller.ThingsWentWrong(e);
                }
            }          
        }

        internal static void ModifyEntry(int id, string dataOption, string newData)
        {
            using (conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    if (dataOption.Equals("YearOfBirth") && int.TryParse(newData, out _))
                    {
                        cmd = new SqlCommand(
                            $"UPDATE Dave_Db_Test.dbo.HeartRates SET {dataOption} = '{int.Parse(newData)}' WHERE ID = {id}", conn);
                    }

                    else if (newData.Length > 0)
                    {
                        cmd = new SqlCommand(
                            $"UPDATE Dave_Db_Test.dbo.HeartRates SET {dataOption} = '{newData}' WHERE ID = {id}", conn);
                    }
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Controller.ThingsWentWrong(e);
                }
            }          
        }
    }
}
