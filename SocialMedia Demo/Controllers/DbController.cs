using System.Data;
using MySql.Data.MySqlClient;
using SocialMedia_Demo.Models;

namespace SocialMedia_Demo.Controllers;

public static class  DbController
{
    private static readonly MySqlConnection Conn = new MySqlConnection("Server=localhost;Database=csharp;uid=root;password=admin1234");
    
    private static string HashPass(string input)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string result = BCrypt.Net.BCrypt.HashPassword(input,salt);
        return result;
    }   
    //CRUD operations
    public static List<Person> GetUsers(int id)
    {
        List<Person> friends = GetFriends(id);
        List<Person> people = new List<Person>();
        try
        {
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            string query = $"SELECT UserName,Id FROM Users Where Id !={id}";

            MySqlCommand command = new MySqlCommand(query, Conn);
            MySqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                string name = (string)dataReader[0];
                int personId = (int)dataReader[1];
                PersonStatus status;
                try
                {
                    status = friends.First(x => x.PersonId == (int)dataReader[1]).Status;
                }
                catch (InvalidOperationException)
                {
                    status = PersonStatus.None;
                }
                people.Add(new Person()
                {
                    Name = name,
                    PersonId = personId,
                    Profile_Photo = "img.png",
                    Status = status
                });
            }
            
            
        }
        catch (MySqlException e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            if (Conn.State != ConnectionState.Closed)
            {
                Conn.Close();
            }
        }

        return people;
    }

    public static List<Person> GetFriends(int id)
    {
        List<Person> friends = new List<Person>(); 
        try
        {
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }

            string query = $"SELECT Users.Id,Users.UserName,Friendship.Status From Users " +
                           $"INNER JOIN Friendship ON Friendship.FriendId=Users.Id WHERE Friendship.UserId={id} " +
                           $"UNION Select Users.Id,Users.UserName,Friendship.Status From Users " +
                           $"INNER JOIN Friendship ON Friendship.UserId=Users.Id WHERE Friendship.FriendId={id}";
            //                                                                                      Friendship ON Friendship.FriendId=Users.Id WHERE Friendship.UserId=3 OR  Friendship.FriendId=3;

            MySqlCommand command = new MySqlCommand(query, Conn);
            MySqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                friends.Add(new Person()
                {
                    PersonId = (int)dataReader[0],
                    Name = (string)dataReader[1],
                    Profile_Photo = "img.png",
                    Status = (PersonStatus)dataReader[2]
                });
            }
        }
        catch (MySqlException e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            if (Conn.State != ConnectionState.Closed)
            {
                Conn.Close();
            }
        }

        return friends;
    }

    public static List<Person> GetFriendRequests(int id)
    {
        List<Person> friends = new List<Person>(); 
        try
        {
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }

            string query = $"SELECT Users.Id,Users.UserName,Friendship.Status From Users INNER JOIN Friendship ON Friendship.UserId=Users.Id WHERE Friendship.FriendId={id};";

            MySqlCommand command = new MySqlCommand(query, Conn);
            MySqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                friends.Add(new Person()
                {
                    PersonId = (int)dataReader[0],
                    Name = (string)dataReader[1],
                    Profile_Photo = "img.png",
                    Status = (PersonStatus)dataReader[2]
                });
            }
        }
        catch (MySqlException e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            if (Conn.State != ConnectionState.Closed)
            {
                Conn.Close();
            }
        }

        return friends;
    }
    
    public static bool Add(User user)
    {
        int result = 0;
        if (user.Password != null)
        {
            string hashed = HashPass(user.Password);
            try
            {
                if (Conn.State != ConnectionState.Open)
                {
                    Conn.Open();
                }

                string query =
                    $"INSERT INTO Users (UserName,Email,Password) VALUES('{user.UserName}','{user.Email}','{hashed}')";

                MySqlCommand command = new MySqlCommand(query, Conn);

                result = command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                if (Conn.State != ConnectionState.Closed)
                {
                    Conn.Close();
                }
            }
        }

        return result > 0;
    }
    
    public static bool AddFriend(int id, Person person)
    {
        int result;
        List<Person> friends = GetFriends(id);
        int count = friends.Count(x => x.PersonId == person.PersonId);
        if (count > 0) 
        {
            return false;
        }
        try
        {
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            
            string query = $"INSERT INTO Friendship(UserId,FriendId,Status,DateAdded,DateModified) VALUES({id},{person.PersonId},{(int)person.Status},NOW(),NOW())";
            MySqlCommand updateCommand = new MySqlCommand(query, Conn);
            result = updateCommand.ExecuteNonQuery();

        }
        catch (MySqlException e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            if(Conn.State != ConnectionState.Closed)
                    Conn.Close();
        }
        return result > 0;
    }

    public static bool UpdateFriend(int id, Person person)
    {
        int result;
        if (Conn.State != ConnectionState.Open)
        {
            Conn.Open();
        }
        try
        {
            string query =
                $"UPDATE Friendship SET Status={(int)person.Status} WHERE UserId ={id} AND FriendId={person.PersonId}";

            MySqlCommand command = new MySqlCommand(query,Conn);
            result = command.ExecuteNonQuery();
        }
        catch (MySqlException e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            if (Conn.State != ConnectionState.Closed)
            {
                Conn.Close();
            }
        }

        return result > 0;
    }
    // public static bool Modify(params string[] moreColumns){}
    
    //Login 
    public static bool Authenticate(User user)
    {
        bool result = false;
        if (user.Password != null)
        {
            try
            {
                if (Conn.State != ConnectionState.Open)
                {
                    Conn.Open();
                }

                string query = $"SELECT Password FROM Users WHERE UserName = '{user.UserName}'";
                MySqlCommand command = new MySqlCommand(query, Conn);
                MySqlDataReader reader = command.ExecuteReader();
                List<string> passes = new List<string>();
                while (reader.Read())
                {
                    string? read = reader[0].ToString();
                    if (read != null)
                    {
                        passes.Add(read);
                    }
                }

                foreach (var pass in passes)
                {
                    result = BCrypt.Net.BCrypt.Verify(user.Password , pass);
                }
                
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                Conn.Close();
            }
        }

        return result;
    }
}