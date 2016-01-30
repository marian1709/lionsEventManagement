using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lionsEmailService
{
    public class Program
    {
        private static List<DbScheduledEmail> scheduledMailList;
        private static List<DbEvent> eventList;  

        public static void Main(string[] args)
        {
            

            Console.WriteLine("Emailservice has been started. Press any key to stop the service.");
            while (!Console.KeyAvailable)
            {
                // Empty the List for Mails to send
                scheduledMailList = new List<DbScheduledEmail>();
                eventList = new List<DbEvent>();

                Console.WriteLine("Looping...");
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString =
                        "Server=MSCHIESERVER\\SQLEXPRESS;Database = lionsEventManagement;User Id = lionsEventManagement;Password = ******;Trusted_Connection = True;Persist Security Info = True;Integrated Security = False";

                    connection.Open();

                    // Get the scheduled email
                    SqlCommand commandMails = new SqlCommand("SELECT mail.EmailId, mail.EventId, mail.MemberId, mail.EmailText, mail.AnswerCode, mail.ParticipantId, mail.EmailSent, mail.ScheduledDate, member.MemberId, member.Email " +
                                                             "FROM dbo.ScheduledEmail AS mail " +
                                                             "INNER JOIN member " +
                                                             "ON (mail.MemberId = member.MemberId) " +
                                                             "WHERE EmailSent = 'false' " +
                                                             "AND (ScheduledDate < GETDATE())", connection);

                    SqlCommand commandMember = new SqlCommand("SELECT FirstName, LastName, Email, " +
                                                              "FROM dbo.Member", connection);

                    
                    // Get all scheduled mails which are not sent already and where the ScheduledDate is before now
                    using (SqlDataReader reader = commandMails.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DbScheduledEmail dbMail = new DbScheduledEmail();
                            dbMail.EmailId = (int) reader[0];
                            dbMail.EventId = (int) reader[1];
                            dbMail.MemberId = (int) reader[2];
                            dbMail.Emailtext = (String) reader[3];
                            dbMail.Answercode = (String) reader[4];
                            dbMail.ParticipantId = (int) reader[5];
                            dbMail.EmailAddress = (String) reader[9];
                            
                            scheduledMailList.Add(dbMail);
                        }
                        
                        reader.Close();
                    }

                    // Get all event data for the events in the scheduled mails
                    foreach (var scheduledMail in scheduledMailList)
                    {
                        SqlCommand commandEvent = new SqlCommand("SELECT EventId, Title, Description, Date " +
                                                                 "FROM dbo.Event " +
                                                                 "WHERE EventId = "+scheduledMail.EventId, connection);

                        using (SqlDataReader reader = commandEvent.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var e = new DbEvent();
                                e.EventId = (int) reader[0];
                                e.Title = (String) reader[1];
                                e.Description = (String) reader[2];
                                e.Date = (DateTime) reader[3];

                                // Do not add duplicate Events to the event list
                                var duplicate = false;
                                foreach (var dbEvent in eventList)
                                {
                                    if (dbEvent.EventId == e.EventId) duplicate = true;
                                }
                                if(!duplicate) eventList.Add(e);
                            }

                            reader.Close();
                        }

                        // Send the scheduledMail -----------------------------------------------------------------------------

                        // Get the Event
                        var emailEvent = eventList.SingleOrDefault(ev => ev.EventId == scheduledMail.EventId);
                        if(emailEvent == null) continue;

                        // Create Mail Links
                        var acceptLink = "<a href=\"http://localhost:1155/EventManagement/Accept/" + scheduledMail.EventId + "?pid=" + scheduledMail.ParticipantId + "&ac=" + scheduledMail.Answercode + "&a=1\" >Zusagen</a>";
                        var acceptWithFamilyLink = "<a href=\"http://localhost:1155/EventManagement/Accept/" + scheduledMail.EventId + "?pid=" + scheduledMail.ParticipantId + "&ac=" + scheduledMail.Answercode + "&a=2\" >Zusagen mit Anhang</a>";
                        var declineLink = "<a href=\"http://localhost:1155/EventManagement/Accept/" + scheduledMail.EventId + "?pid=" + scheduledMail.ParticipantId + "&ac=" + scheduledMail.Answercode + "&a=0\" >Absagen</a>";

                        MailMessage mail = new MailMessage("marian.schiemann@googlemail.com", scheduledMail.EmailAddress);
                        SmtpClient client = new SmtpClient();
                        client.Port = 25;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Host = "localhost";
                        mail.IsBodyHtml = true;
                        mail.Subject = "lionsEventManagement - " + emailEvent.Title;
                        mail.Body = emailEvent.Description + "<br /> <br />" + acceptLink + "<br />" + acceptWithFamilyLink + "<br />" + declineLink + "<br />";

                        client.Send(mail);

                        // Update Database -> Mail sent
                        SqlCommand command = new SqlCommand("UPDATE ScheduledEmail SET EmailSent = 'true' " +
                                                            "WHERE EmailId = "+scheduledMail.EmailId, connection);

                        command.ExecuteReader();
                    }
                    
                }
                Thread.Sleep(1 * 60 * 1000);
            }
        }

        public static void SendMail(DbScheduledEmail dbMail)
        {
            
        }

    public class DbScheduledEmail
        {
            public int EmailId { get; set; }
            public int EventId { get; set; }
            public int MemberId { get; set; }
            public String Emailtext { get; set; }
            public String Answercode { get; set; }
            public int ParticipantId { get; set; }
            public String EmailAddress { get; set; }
        }

        public class DbEvent
        {
            public int EventId { get; set; }
            public String Title { get; set; }
            public String Description { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
