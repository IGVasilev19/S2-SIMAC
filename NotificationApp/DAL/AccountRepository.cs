using Microsoft.Data.SqlClient;
using BLL;
using DAL.Interfaces;
using System.Reflection.PortableExecutable;

namespace DAL
{
    public class AccountRepository : IAccountRepository
    {
        public IEnumerable<Account> GetAll()
        {
            List<Account> accounts = new List<Account>();

            using (SqlConnection conn = DBConnection.GetConnection())       {
     
                string query = "SELECT AccountId, [Name], Email, [Password], OrganizationId, RoleId FROM Account";
                SqlCommand cmd = new SqlCommand(query, conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int roleId = reader.GetInt32(4);

                        Account account = new Account(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetInt32(4),
                            reader.GetInt32(5)
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
                string query = "SELECT AccountId, Name, Email, Password, OrganizationId, RoleId FROM Account WHERE AccountId = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int roleId = reader.GetInt32(4);

                        Account account = new Account(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetInt32(4),
                            reader.GetInt32(5)
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
                string query = "INSERT INTO Account (Name, Email, Password, OrganizationId, RoleId) VALUES (@name, @email, @password, @organizationId, @roleId)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", account.Name);
                cmd.Parameters.AddWithValue("@email", account.Email);
                cmd.Parameters.AddWithValue("@password", account.Password);
                cmd.Parameters.AddWithValue("@organizationId", account.OrganizationId);
                cmd.Parameters.AddWithValue("@roleId", account.RoleId);

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Account account)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "UPDATE Account SET Name = @name, Email = @email, Password = @password, OrganizationId = @organizationId, RoleId = @roleId WHERE AccountId = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", account.Name);
                cmd.Parameters.AddWithValue("@email", account.Email);
                cmd.Parameters.AddWithValue("@password", account.Password);
                cmd.Parameters.AddWithValue("@organizationId", account.OrganizationId);
                cmd.Parameters.AddWithValue("@roleId", account.RoleId);
                cmd.Parameters.AddWithValue("@id", account.AccountId);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "DELETE FROM Account WHERE AccountId = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        public Account GetByEmail(string email)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {

                string query = "SELECT AccountId, Name, Email, Password, OrganizationId, RoleId FROM Account WHERE Email = @email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@email", email);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Account user = new Account(
                            reader.GetInt32(reader.GetOrdinal("AccountId")),
                            reader.GetString(reader.GetOrdinal("Name")),
                            reader.GetString(reader.GetOrdinal("Email")),
                            reader.GetString(reader.GetOrdinal("Password")),
                            reader.GetInt32(reader.GetOrdinal("OrganizationId")),
                            reader.GetInt32(reader.GetOrdinal("RoleId"))
                        );
                        return user;
                    }
                }
            }
            return null;
        }

        public IEnumerable<Account> GetByOrganization(int organizationId)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT AccountId, Name, Email, Password, OrganizationId, RoleId FROM Account WHERE OrganizationId = @organizationId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@organizationId", organizationId);

                List<Account> accounts = new List<Account>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Account user = new Account(
                            reader.GetInt32(reader.GetOrdinal("AccountId")),
                            reader.GetString(reader.GetOrdinal("Name")),
                            reader.GetString(reader.GetOrdinal("Email")),
                            reader.GetString(reader.GetOrdinal("Password")),
                            reader.GetInt32(reader.GetOrdinal("OrganizationId")),
                            reader.GetInt32(reader.GetOrdinal("RoleId"))
                        );
                        accounts.Add(user);
                    }
                    return accounts;
                }
            }
        }
        public Account GetManagerByOrganization(int organizationId)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {

                string query = "SELECT AccountId, [Name], Email, [Password], OrganizationId, RoleId FROM Account " +
                    "WHERE RoleId = '2' AND OrganzationId = @organizationId"; //Manager is hard coded to roleId 2
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@organizationId", organizationId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int roleId = reader.GetInt32(4);

                        Account account = new Account(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetInt32(4),
                            reader.GetInt32(5)
                        );
                        return account;
                    }
                }
            }
            return null;
        }
    }
}