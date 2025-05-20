using BLL;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PermissionRepository : IPermissionRepository
    {
        public IEnumerable<Permission> GetAll()
        {
            List<Permission> permissions = new List<Permission>();
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT PermissionId, Name, ParentId FROM Permission";
                SqlCommand cmd = new SqlCommand(query, conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Permission permission = new Permission(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.IsDBNull(2) ? null : reader.GetInt32(2)
                        );

                        permissions.Add(permission);
                    }
                }
            }
            return permissions;
        }

        public Permission GetById(int id)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT * FROM Permission WHERE PermissionId = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Permission permission = new Permission(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.IsDBNull(2) ? null : reader.GetInt32(2)
                        );

                        return permission;
                    }
                }
            }
            return null;
        }

        public List<Permission> GetPermissionsByRoleId(int roleId)
        {
            List<Permission> permissions = new List<Permission>();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    SELECT p.PermissionId, p.Name, p.ParentId
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
                        int? parentId = reader.IsDBNull(2) ? null : reader.GetInt32(2);
                        permissions.Add(new Permission(id, name, parentId));
                    }
                }
            }
            return permissions;
        }

        public void Add(Permission permission)
        {
            throw new NotImplementedException();
        }

        public void Update(Permission entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
