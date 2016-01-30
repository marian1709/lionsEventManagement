using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace lionsEventManagement.Models
{
    public class ViewEventViewModel
    {
        public ViewEventViewModel()
        {
            this.Address = new AddressViewViewModel();
            this.Date = DateTime.Now;
            this.Time = DateTime.Now;
            this.FirstReminderMail = DateTime.Now;
            this.SecondReminderMail = DateTime.Now;
            this.ReplyDeadline = DateTime.Now;
        }

        public ViewEventViewModel(Event eventModel)
        {
            this.EventId = eventModel.EventId;
            this.Title = eventModel.Title;
            this.Description = eventModel.Description;
            this.Date = eventModel.Date;
            this.Time = eventModel.Time;
            this.FirstReminderMail = eventModel.FirstReminderMail;
            this.SecondReminderMail = eventModel.SecondReminderMail;
            this.ReplyDeadline = eventModel.ReplyDeadline;
            this.AcceptanceEvent = eventModel.AcceptanceEvent;
            this.Address = new AddressViewViewModel(eventModel.Address);
        }

        public int EventId { get; set; }

        [Required]
        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Datum")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:t}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Uhrzeit")]
        [DataType(DataType.Time), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }

        [Required]
        [Display(Name = "Erste Erinnerung")]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime FirstReminderMail { get; set; }

        [Required]
        [Display(Name = "Zweite Erinnerung")]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime SecondReminderMail { get; set; }

        [Required]
        [Display(Name = "Deadline Rückmeldung")]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime ReplyDeadline { get; set; }

        [Required]
        [Display(Name = "Zusage benötigt?")]
        public Boolean AcceptanceEvent { get; set; }

        [Required]
        public AddressViewViewModel Address { get; set; }
    }

    public class CreateEventViewModel
    {
        public CreateEventViewModel()
        {
            this.Address = new AddressEditorViewModel();
            this.Date = DateTime.Now;
            this.Time = DateTime.Now;
            this.FirstReminderMail = DateTime.Now;
            this.SecondReminderMail = DateTime.Now;
            this.ReplyDeadline = DateTime.Now;
        }

        public CreateEventViewModel(Event eventModel)
        {
            this.EventId = eventModel.EventId;
            this.Title = eventModel.Title;
            this.Description = eventModel.Description;
            this.Date = eventModel.Date;
            this.Time = eventModel.Time;
            this.FirstReminderMail = eventModel.FirstReminderMail;
            this.SecondReminderMail = eventModel.SecondReminderMail;
            this.ReplyDeadline = eventModel.ReplyDeadline;
            this.AcceptanceEvent = eventModel.AcceptanceEvent;
            this.Address = new AddressEditorViewModel(eventModel.Address);
        }

        public int EventId { get; set; }

        [Required]
        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Datum")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:t}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Uhrzeit")]
        [DataType(DataType.Time), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }

        [Required]
        [Display(Name = "Erste Erinnerung")]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime FirstReminderMail { get; set; }

        [Required]
        [Display(Name = "Zweite Erinnerung")]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime SecondReminderMail { get; set; }

        [Required]
        [Display(Name = "Deadline Rückmeldung")]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime ReplyDeadline { get; set; }

        [Required]
        [Display(Name = "Zusage benötigt?")]
        public Boolean AcceptanceEvent { get; set; }

        [Required]
        public AddressEditorViewModel Address { get; set; }
    }

    public class AddressViewViewModel
    {
        public AddressViewViewModel() { }
        public AddressViewViewModel(Address addressModel)
        {
            this.AddressId = addressModel.AddressId;
            this.City = addressModel.City;
            this.Zip = addressModel.Zip;
            this.StreetName = addressModel.StreetName;
            this.HouseNumber = addressModel.HouseNumber;
        }

        public int AddressId { get; set; }
        [Required]
        [Display(Name = "Stadt")]
        public string City { get; set; }
        [Required]
        [Display(Name = "PLZ")]
        public string Zip { get; set; }
        [Required]
        [Display(Name = "Straße")]
        public string StreetName { get; set; }
        [Required]
        [Display(Name = "Hausnummer")]
        public string HouseNumber { get; set; }
    }

    public class AddressEditorViewModel
    {
        public AddressEditorViewModel() { }
        public AddressEditorViewModel(Address addressModel)
        {
            this.AddressId = addressModel.AddressId;
            this.City = addressModel.City;
            this.Zip = addressModel.Zip;
            this.StreetName = addressModel.StreetName;
            this.HouseNumber = addressModel.HouseNumber;
        }

        public int AddressId { get; set; }
        [Required]
        [Display(Name = "Stadt")]
        public string City { get; set; }
        [Required]
        [Display(Name = "PLZ")]
        public string Zip { get; set; }
        [Required]
        [Display(Name = "Straße")]
        public string StreetName { get; set; }
        [Required]
        [Display(Name = "Hausnummer")]
        public string HouseNumber { get; set; }
    }

    public class ParticipantListViewModel
    {
        public ParticipantListViewModel()
        {
            this.Participants = new List<ParticipantViewModel>();
        }

        public ViewEventViewModel Event { get; set; }
        public IEnumerable<ParticipantViewModel> Participants { get; set; }
    }

    public class ParticipantViewModel
    {
        [Display(Name = "Vorname")]
        public String FirstName { get; set; }
        [Display(Name = "Nachname")]
        public String LastName { get; set; }
        [Display(Name = "E-Mailadresse")]
        public String Email { get; set; }

        public int Participating { get; set; }
    }

    public class InviteViewModel
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
    }
}