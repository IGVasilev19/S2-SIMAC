using BLL;
using DAL.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class OrganizationRepository : IOrganizationRepository
    {
        public void Add(Organization organization)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "INSERT INTO Organization (Name) VALUES (@name)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", organization.Name);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "DELETE FROM Organization WHERE OrganizationID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        public List<Organization> GetAll()
        {
            List<Organization> organizations = new List<Organization>();
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT OrganizationId, Name FROM Organization";
                SqlCommand cmd = new SqlCommand(query, conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Organization organization = new Organization(
                            reader.GetInt32(0),
                            reader.GetString(1)
                        );

                        organizations.Add(organization);
                    }
                }
            }
            return organizations;
        }

        public Organization GetById(int id)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT * FROM Role WHERE OrganizationId = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Organization organization = new Organization(
                            reader.GetInt32(0),
                            reader.GetString(1)
                        );

                        return organization;
                    }
                }
            }
            return null;

        }

        public void Update(Organization entity)
        {
            throw new NotImplementedException();
        }
    }
}
