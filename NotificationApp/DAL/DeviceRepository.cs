using Microsoft.Data.SqlClient;
using BLL;
using DAL.Interfaces;

namespace DAL
{
    public class DeviceRepository : IDeviceRepository
    {
        public IEnumerable<Device> GetAll()
        {
            List<Device> devices = new List<Device>();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT DeviceID, Name, Location FROM Devices";
                SqlCommand cmd = new SqlCommand(query, conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Device device = new Device(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2)
                        );

                        devices.Add(device);
                    }
                }
            }
            return devices;
        }

        public Device GetById(int id)
        {
            using(SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT DeviceID, Name, Location FROM Devices WHERE DeviceID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Device device = new Device(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2)
                        );

                        return device;
                    }
                }
            }
            return null;
        }

        public void Add(Device entity)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "INSERT INTO Devices (Name, Location) VALUES (@name, @location)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", entity.Name);
                cmd.Parameters.AddWithValue("@location", entity.Location);

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Device entity)
        {
            using(SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "UPDATE Devices SET Name = @name, Location = @location WHERE DeviceID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", entity.Name);
                cmd.Parameters.AddWithValue("@location", entity.Location);
                cmd.Parameters.AddWithValue("@id", entity.DeviceID);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "DELETE FROM Devices WHERE DeviceID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}