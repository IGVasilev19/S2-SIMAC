using BLL;
using DAL.Interfaces;
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
                string query = "SELECT PermissionId, Name FROM Permission";
                SqlCommand cmd = new SqlCommand(query, conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Permission permission = new Permission(
                            reader.GetInt32(0),
                            reader.GetString(1)
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
                            reader.GetString(1)
                        );

                        return permission;
                    }
                }
            }
            return null;
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
