using Microsoft.Data.SqlClient;
using BLL;

namespace DAL
{
    public class AccountRepository : IRepository<Account>
    {
        public List<Account> GetAll()
        {
            List<Account> accounts = new List<Account>();

            using (SqlConnection conn = DBConnection.GetConnection())       {
     
                string query = "SELECT AccountId, Name, Email, Password, RoleId FROM Accounts";
                SqlCommand cmd = new SqlCommand(query, conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int roleId = reader.GetInt32(4);
                        Role role = GetRoleById(roleId); 

                        Account account = new Account(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3), 
                            role
                        );

                        accounts.Add(account);
                    }
                }
            }

            return accounts;
        }

        public Account GetById(int id)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT AccountId, Name, Email, Password, RoleId FROM Accounts WHERE AccountId = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int roleId = reader.GetInt32(4);
                        Role role = GetRoleById(roleId);

                        Account account = new Account(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            role
                        );

                        return account;
                    }
                }
            }

            return null;
        }

        public void Add(Account account)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "INSERT INTO Accounts (Name, Email, Password, RoleId) VALUES (@name, @email, @password, @roleId)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", account.Name);
                cmd.Parameters.AddWithValue("@email", account.Email);
                cmd.Parameters.AddWithValue("@password", account.Password);
                cmd.Parameters.AddWithValue("@roleId", account.AccountRole.RoleId);

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Account account)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "UPDATE Accounts SET Name = @name, Email = @email, Password = @password, RoleId = @roleId WHERE AccountId = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", account.Name);
                cmd.Parameters.AddWithValue("@email", account.Email);
                cmd.Parameters.AddWithValue("@password", account.Password);
                cmd.Parameters.AddWithValue("@roleId", account.AccountRole.RoleId);
                cmd.Parameters.AddWithValue("@id", account.AccountId);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "DELETE FROM Accounts WHERE AccountId = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        public Role GetRoleById(int id)
        {
            return new Role(id, "DefaultRole", new List<Permission> { Permission.ViewReport });
        }

        public Account GetByEmail(string email)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT AccountId, Name, Email, Password, RoleId FROM Accounts WHERE Email = @email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@email", email);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int roleId = reader.GetInt32(4);
                        Role role = GetRoleById(roleId);

                        Account account = new Account(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            role
                        );

                        return account;
                    }
                }
            }

            return null;
        }
    }
}