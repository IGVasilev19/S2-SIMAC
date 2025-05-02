using Microsoft.Data.SqlClient;
using BLL;

namespace DAL
{
    public class AccountRepository : IAccountRepository
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
                string query = "INSERT INTO Account (Name, Email, Password, RoleId) VALUES (@name, @email, @password, @roleId)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", account.Name);
                cmd.Parameters.AddWithValue("@email", account.Email);
                cmd.Parameters.AddWithValue("@password", account.Password);
                cmd.Parameters.AddWithValue("@roleId", account.AccountRole.RoleId); // Remove AccountRole for DB testing

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
            string roleName = "";
            List<Permission> permissions = new List<Permission>();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                // Get role name
                string roleQuery = "SELECT Name FROM [Role] WHERE RoleId = @id";
                SqlCommand roleCmd = new SqlCommand(roleQuery, conn);
                roleCmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = roleCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        roleName = reader.GetString(0);
                    }
                }

                // Get permissions for this role
                string permQuery = @"
                    SELECT p.Name
                    FROM RolePermission rp
                    JOIN Permission p ON rp.PermissionId = p.PermissionId
                    WHERE rp.RoleId = @id";

                SqlCommand permCmd = new SqlCommand(permQuery, conn);
                permCmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = permCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string permName = reader.GetString(0);
                        if (Enum.TryParse(permName, out Permission perm))
                        {
                            permissions.Add(perm);
                        }
                    }
                }
            }

            return new Role(id, roleName, permissions);
        }
    }
}