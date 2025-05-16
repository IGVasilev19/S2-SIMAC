using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class RoleRepository : IRoleRepository
    {
        public void Add(Role role)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "INSERT INTO Role (Name) VALUES (@name)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", role.Name);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    DELETE FROM RolePermission WHERE RoleId = @id;
                    DELETE FROM Role WHERE RoleId = @id;";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public Role GetById(int id)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT RoleId, Name FROM Role WHERE RoleId = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Role role = new Role(
                            reader.GetInt32(0),
                            reader.GetString(1)
                        );

                        return role;
                    }
                }
            }
            return null;
        }

        public IEnumerable<Role> GetAll()
        {
            List<Role> roles = new List<Role>();
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT RoleId, Name FROM Role";
                SqlCommand cmd = new SqlCommand(query, conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Role organization = new Role(
                            reader.GetInt32(0),
                            reader.GetString(1)
                        );

                        roles.Add(organization);
                    }
                }
            }
            return roles;
        }

        public void Update(Role role)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "UPDATE Role SET Name = @name";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", role.Name);

                cmd.ExecuteNonQuery();
            }
        }

        public void AssignPermission(int roleId, IEnumerable<Permission> permissions)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "DELETE FROM RolePermission " +
                                "WHERE RoleId = @roleId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@roleId", roleId);
                cmd.ExecuteNonQuery();

                foreach (Permission permission in permissions)
                {
                    query = "INSERT INTO RolePermission (RoleId, PermissionId) VALUES (@roleId, @permissionId)";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@roleId", roleId);
                    cmd.Parameters.AddWithValue("@permissionId", permission.PermissionId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Permission> GetPermissionsByRoleId(int roleId)
        {
            List<Permission> permissions = new List<Permission>();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    SELECT p.PermissionId, p.Name
                    FROM RolePermission rp
                    JOIN Permission p ON rp.PermissionId = p.PermissionId
                    WHERE rp.RoleId = @roleId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@roleId", roleId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        permissions.Add(new Permission(id, name));
                    }
                }
            }
            return permissions;
        }

        public void AssignPermission(Role role, IEnumerable<Permission> permissions)
        {
            throw new NotImplementedException();
        }
    }
}