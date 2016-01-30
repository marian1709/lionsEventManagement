using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace lionsEventManagement.Models
{
    public class EventManagementModels
    {

    }

    public class Member
    {
        [Key]
        public int MemberId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateEdited { get; set; }

        [Required]
        [Display(Name = "Vorname")]
        public String FirstName { get; set; }
        
        [Required]
        [Display(Name = "Nachname")]
        public String LastName { get; set; }
        
        [Required]
        [Display(Name = "Emailadresse")]
        public String Email { get; set; }

    }

    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateEdited { get; set; }

        [Required]
        [Display(Name = "Titel")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Beschreibung")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Datum")]
        public DateTime Date { get; set; }
        [Required]
        [Display(Name = "Uhrzeit")]
        public DateTime Time { get; set; }
        public DateTime FirstReminderMail { get; set; }
        public DateTime SecondReminderMail { get; set; }
        [Required]
        public DateTime ReplyDeadline { get; set; }

        public Boolean AcceptanceEvent { get; set; }

        [Required]
        public virtual Address Address { get; set; }

        public virtual ICollection<Participant> Participants { get; set; }

    }

    public class Participant
    {
        [Key]
        public int ParticipantId { get; set; }

        public String SecurityCode { get; set; }

        public virtual Member Member { get; set; }

        // -1: No answer
        // 0: Not Participating
        // 1: Participating
        // 2: Participating with Family
        public int Participating { get; set; }
    }

    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
    }

    public class EventManagementDbContext : DbContext
    {
        public EventManagementDbContext() : base("DefaultConnection")
        {
            
        }

        public DbSet<Member> Members { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Participant> Participants { get; set; }

        public DbSet<ScheduledEmail> ScheduledEmails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}