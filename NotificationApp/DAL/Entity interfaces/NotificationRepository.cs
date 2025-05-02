using Microsoft.Data.SqlClient;
using BLL;

namespace DAL
{
    public class NotificationRepository : INotificationRepository
    {
        public List<Notification> GetAll()
        {
            List<Notification> notifications = new List<Notification>();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT NotificationID, Title, Content, Important, Read, Date FROM Notifications";
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
                            reader.GetBoolean(4),
                            reader.GetDateTime(5)
                        );

                        notifications.Add(notification);
                    }
                }
            }
            return notifications;
        }

        public Notification GetById(int id)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT NotificationID, Title, Content, Important, Read, Date FROM Notifications WHERE NotificationID = @id";
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
                            reader.GetBoolean(4),
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
                string query = "INSERT INTO Notifications (Title, Content, Important, Read, Date) VALUES (@title, @content, @important, @read, @date)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", entity.Title);
                cmd.Parameters.AddWithValue("@content", entity.Content);
                cmd.Parameters.AddWithValue("@important", entity.Important);
                cmd.Parameters.AddWithValue("@read", entity.Read);
                cmd.Parameters.AddWithValue("@date", entity.Date);

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Notification entity)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "UPDATE Notifications SET Title = @title, Content = @content, Important = @important, Read = @read, Date = @date WHERE NotificationID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", entity.Title);
                cmd.Parameters.AddWithValue("@content", entity.Content);
                cmd.Parameters.AddWithValue("@important", entity.Important);
                cmd.Parameters.AddWithValue("@read", entity.Read);
                cmd.Parameters.AddWithValue("@date", entity.Date);
                cmd.Parameters.AddWithValue("@id", entity.NotificationID);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "DELETE FROM Notifications WHERE NotificationID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}