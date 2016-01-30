using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace lionsEventManagement.Models
{
    public class ViewMemberViewModel
    {
        public ViewMemberViewModel()
        {

        }

        public ViewMemberViewModel(Member member)
        {
            this.MemberId = member.MemberId;
            this.FirstName = member.FirstName;
            this.LastName = member.LastName;
            this.Email = member.Email;
        }

        [Key]
        public int MemberId { get; set; }

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

    public class EditMemberViewModel
    {
        public EditMemberViewModel()
        {

        }

        public EditMemberViewModel(Member member)
        {
            this.MemberId = member.MemberId;
            this.FirstName = member.FirstName;
            this.LastName = member.LastName;
            this.Email = member.Email;
        }

        [Key]
        public int MemberId { get; set; }

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
}