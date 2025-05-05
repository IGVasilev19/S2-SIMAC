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
                string query = "SELECT NotificationID, Title, Content, Important, Date FROM Notification";
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
                            reader.GetDateTime(4)
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
                string query = "SELECT NotificationID, Title, Content, Important, Date FROM Notification WHERE NotificationID = @id";
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
                            reader.GetDateTime(4)
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
                string query = "INSERT INTO Notification (Title, Content, Important, Date) VALUES (@title, @content, @important, @date)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", entity.Title);
                cmd.Parameters.AddWithValue("@content", entity.Content);
                cmd.Parameters.AddWithValue("@important", entity.Important);
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

        
    }
}