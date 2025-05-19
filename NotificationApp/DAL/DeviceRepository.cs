using Microsoft.Data.SqlClient;
using BLL;
using DAL.Interfaces;

namespace DAL
{
    public class DeviceRepository : IDeviceRepository
    {
        public Device GetById(int id)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT * FROM Device WHERE DeviceID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Device device = new Device(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetInt32(4),
                            reader.GetInt32(3)
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
                string query = "INSERT INTO Devices (Name, Location,StatusId, OrganizationId) VALUES (@name, @location, @StatusId, @OrganizationId)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", entity.Name);
                cmd.Parameters.AddWithValue("@location", entity.Location);
                cmd.Parameters.AddWithValue("@StatusId", (int)entity.DeviceStatus);
                cmd.Parameters.AddWithValue("@OrganizationId", entity.OrganizationID);

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Device entity)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "UPDATE Device SET Name = @name, Location = @location, StatusId = @StatusID, OrganizationId = @OrganizationID WHERE DeviceID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", entity.Name);
                cmd.Parameters.AddWithValue("@location", entity.Location);
                cmd.Parameters.AddWithValue("@id", entity.DeviceID);
                cmd.Parameters.AddWithValue("@OrganizationID", entity.OrganizationID);
                cmd.Parameters.AddWithValue("@StatusID", (int)entity.DeviceStatus);

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
    
        public IEnumerable<Device> GetAll()
        {
            List<Device> devices = new List<Device>();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT * from Device";

                SqlCommand cmd = new SqlCommand(query, conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Device device = new Device(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetInt32(4),
                            reader.GetInt32(3)
                        );

                        devices.Add(device);
                    }

                }
                return devices;
            }
        }
        
        
        public Status GetStatus(Device device)
        {
            Status status = new Status();
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT StatusId from Device Where DeviceID = @DeviceID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DeviceID", device.DeviceID);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int statusId = reader.GetInt32(0);
                        return (Status)statusId; // cast int to Status enum
                        
                    }
                    else
                    {
                        throw new Exception("Device not found.");
                    }
                }
            }
        }

    }
}