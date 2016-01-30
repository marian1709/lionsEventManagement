using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;

namespace lionsEventManagement.Models
{
    public class EmailModels
    {
    }

    public class ScheduledEmail
    {
        [Key]
        public int EmailId { get; set; }
        public int EventId { get; set; }
        public int MemberId { get; set; }
        public int ParticipantId { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:t}", ApplyFormatInEditMode = true)]
        public DateTime? ScheduledDate { get; set; }
        public Boolean EmailSent { get; set; }

        public String EmailText { get; set; }

        public String AnswerCode { get; set; }
    }
}