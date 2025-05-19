using Microsoft.Data.SqlClient;
using BLL;
using DAL.Interfaces;

namespace DAL
{
    public class NotificationRepository : INotificationRepository
    {
        public IEnumerable<Notification> GetAll()
        {
            List<Notification> notifications = new List<Notification>();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT NotificationID, Title, Content, Important, OrganizationId, Date FROM Notification";
                SqlCommand cmd = new SqlCommand(query, conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Notification notification = new Notification(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetBoolean(3),
                            reader.GetInt32(4),
                            reader.GetDateTime(5)
                        );

                        notifications.Add(notification);
                    }
                }
            }
            return notifications;
        }

        public Notification GetById(int id) // This method doesn't do anything, don't work with it please
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT NotificationID, Title, Content, Important, OrganizationId, Date FROM Notification WHERE NotificationID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Notification notification = new Notification(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetBoolean(3),
                            reader.GetInt32(4),
                            reader.GetDateTime(5)
                        );

                        return notification;
                    }
                }
            }
            return null;
        }

        public void Add(Notification entity)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "INSERT INTO Notification (Title, Content, Important, OrganizationId, Date) VALUES (@title, @content, @important, @organizationId, @date)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", entity.Title);
                cmd.Parameters.AddWithValue("@content", entity.Content);
                cmd.Parameters.AddWithValue("@important", entity.Important);
                cmd.Parameters.AddWithValue("@organizationId", entity.OrganizationId);
                cmd.Parameters.AddWithValue("@date", entity.Date);

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Notification entity)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "UPDATE Notification SET Title = @title, Content = @content, Important = @important, Date = @date WHERE NotificationID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", entity.Title);
                cmd.Parameters.AddWithValue("@content", entity.Content);
                cmd.Parameters.AddWithValue("@important", entity.Important);
                cmd.Parameters.AddWithValue("@date", entity.Date);
                cmd.Parameters.AddWithValue("@id", entity.NotificationID);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "DELETE FROM Notification WHERE NotificationID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        public List<Notification> GetByPermission(int permissionId)
        {
            List<Notification> notifications = new List<Notification>();
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT NotificationID, Title, Content, Important, NotificationID, Date " +
                                "FROM Notification " +
                                "WHERE PermissionId = @permissionId OR PermissionId IS NULL";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@permissionId", permissionId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Notification notification = new Notification(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetBoolean(3),
                            reader.GetInt32(4),
                            reader.GetDateTime(5)
                        );
                        notifications.Add(notification);
                    }
                }
            }
            return notifications;
        }
    }
}