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
        public int AddRole(Role role)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "INSERT INTO Role (Name, OrganizationId) VALUES (@name, @organizationId); SELECT CAST(SCOPE_IDENTITY() AS INT);";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", role.Name);
                cmd.Parameters.AddWithValue("@organizationId", role.OrganizationId);

                int newId = (int)cmd.ExecuteScalar();
                return newId;
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
                string query = "SELECT RoleId, [Name], OrganizationId FROM Role WHERE RoleId = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Role role = new Role(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2)
                        );

                        return role;
                    }
                }
            }
            return null!;
        }

        public IEnumerable<Role> GetAll()
        {
            List<Role> roles = new List<Role>();
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT RoleId, [Name], OrganizationId FROM Role";
                SqlCommand cmd = new SqlCommand(query, conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Role organization = new Role(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetInt32(2)
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
                string query = "UPDATE Role SET [Name] = @name WHERE RoleId = @roleId";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@name", role.Name);
                cmd.Parameters.AddWithValue("@roleId", role.RoleId);

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
            return new List<Permission> { };
        }
        public IEnumerable<Role> GetAllRolesByOrganisationId(int organizationId)
        {
            List<Role> roles = new List<Role>();
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT RoleId, [Name], OrganizationId FROM Role WHERE OrganizationId = @organizationId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@organizationId", organizationId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Role role = new Role(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetInt32(2)
                        );

                        roles.Add(role);
                    }
                }
            }
            return roles;
        }

        public void AssignPermission(Role role, IEnumerable<Permission> permissions)
        {
            throw new NotImplementedException();
        }

        public void Add(Role entity)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfRoleOccupied(int roleId)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Account WHERE RoleId = @roleId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@roleId", roleId);

                var result = cmd.ExecuteScalar();

                if ((int)result == 0)
                {
                    return true;
                }

                return false;
            }
        }
    }
}