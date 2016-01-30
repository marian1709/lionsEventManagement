using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using lionsEventManagement.Models;
using Microsoft.Ajax.Utilities;

namespace lionsEventManagement.Controllers
{
    [Authorize(Roles = "Administrator, Moderator")]
    public class EventManagementController : Controller
    {
        private EventManagementDbContext db = new EventManagementDbContext();

        // GET: EventManagement
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: EventManagement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Event eventModel = db.Events.Find(id);
            if (eventModel == null)
                return HttpNotFound();

            var eventViewModel = ViewEvent(eventModel);

            return View(eventViewModel);
        }

        // GET: EventManagement/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View(new CreateEventViewModel());
        }

        // POST: EventManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateEventViewModel eventViewModel)
        {
            if (ModelState.IsValid)
            {
                var createEvent = CreateEvent(eventViewModel);
                db.Events.Add(createEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eventViewModel);
        }

        // GET: EventManagement/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Event eventModel = db.Events.Find(id);
            if (eventModel == null)
                return HttpNotFound();

            return View(new CreateEventViewModel(eventModel));
        }

        // POST: EventManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateEventViewModel eventViewModel)
        {
            if (ModelState.IsValid)
            {
                var dbEvent = db.Events.Find(eventViewModel.EventId);
                var updateEvent = UpdateEvent(dbEvent, eventViewModel);

                db.Entry(updateEvent).State = EntityState.Modified;
                db.Entry(updateEvent.Address).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(eventViewModel);
        }

        // GET: EventManagement/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Event eventModel = db.Events.Find(id);
            if (eventModel == null)
                return HttpNotFound();

            var eventViewModel = ViewEvent(eventModel);
            return View(eventViewModel);
        }

        // POST: EventManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event dbEvent = db.Events.Find(id);
            db.Events.Remove(dbEvent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: EventManagement/Invited/5
        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult Invited(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Event eventModel = db.Events.Find(id);
            if (eventModel == null)
                return HttpNotFound();

            ViewBag.EventId = eventModel.EventId;

            var participantList = eventModel.Participants;

            return View(participantList);
        }

        // GET: EventManagement/Invite/5
        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult Invite(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.EventId = id;

            var members = db.Members.ToList();
            return View(members);
        }

        // POST: EventManagement/Invite/5
        [HttpPost]
        public ActionResult Invite(int eventId, String idList)
        {
            if (String.IsNullOrEmpty(idList))
                return RedirectToAction("Index");

            List<String> memberIdListString = idList.Split(',').ToList();
            List<int> memberIdList = memberIdListString.Select(int.Parse).ToList();

            // Add Participants to Event
            Event eventModel = db.Events.Find(eventId);
            if(eventModel == null) 
                return HttpNotFound();
            
            foreach (int memberId in memberIdList)
            {
                var participant = new Participant();
                participant.SecurityCode = Guid.NewGuid().ToString();
                participant.Member = db.Members.Find(memberId);

                if (eventModel.AcceptanceEvent)
                    participant.Participating = -1;
                else
                    participant.Participating = 1;

                bool duplicate = false;
                foreach(Participant p in eventModel.Participants)
                    if (p.Member.MemberId == memberId)
                        duplicate = true;

                if(!duplicate)
                    eventModel.Participants.Add(participant);
            }
            
            db.SaveChanges();

            // Generate Scheduled E-Mails
            CreateEmailsToBeSent(eventModel);

            return RedirectToAction("Invited", "EventManagement", new { id = eventId });
        }

        public ActionResult RemoveInvite(int eventId, int participantId)
        {
            // Remove participant from events
            var eventModel = db.Events.Find(eventId);
            var participant = eventModel.Participants.FirstOrDefault(p => p.ParticipantId == participantId);
            if(participant != null)
                eventModel.Participants.Remove(participant);

            db.SaveChanges();

            // Remove participant
            var participantList = db.Participants.Where(p => p.ParticipantId == participantId);
            db.Participants.RemoveRange(participantList);
            db.SaveChanges();

            // Remove scheduled emails which are not sent
            var emails = db.ScheduledEmails.Where(e => e.ParticipantId == participantId && e.EmailSent == false).ToList();
            if (emails.Count > 0)
            {
                db.ScheduledEmails.RemoveRange(emails);
            }

            db.SaveChanges();

            return RedirectToAction("Invited", "EventManagement", new {id = eventId});
        }

        public ActionResult Accept(int? id)
        {
            var pid = int.Parse(Request.QueryString["pid"]);
            var ac = Request.QueryString["ac"];
            var a = int.Parse(Request.QueryString["a"]);
            // Get the event
            var eventModel = db.Events.Find(id);
            
            // Get the participant
            var participant = eventModel.Participants.FirstOrDefault(p => p.ParticipantId == pid);
            if (participant == null)
                return HttpNotFound();

            var participantDb = db.Participants.Find(pid);
            if (participantDb == null)
                return HttpNotFound();

            if (participantDb.SecurityCode != ac)
                return HttpNotFound();

            // Check if already accepted or declined

            if (eventModel.AcceptanceEvent == true)
            {
                if (participantDb.Participating == -1)
                {
                    participant.Participating = (int) a;
                    participantDb.Participating = (int) a;
                }
            }
            else
            {
                if (participantDb.Participating == 1)
                {
                    participant.Participating = (int)a;
                    participantDb.Participating = (int)a;
                }
            }

            db.SaveChanges();
            return View();
        }

        private ViewEventViewModel ViewEvent(Event eventModel)
        {
            var addressViewModel = new AddressViewViewModel
            {
                City = eventModel.Address.City,
                HouseNumber = eventModel.Address.HouseNumber,
                StreetName = eventModel.Address.StreetName,
                Zip = eventModel.Address.Zip
            };

            var eventViewModel = new ViewEventViewModel
            {
                EventId = eventModel.EventId,
                Title = eventModel.Title,
                Description = eventModel.Description,
                Date = eventModel.Date,
                Time = eventModel.Time,
                ReplyDeadline = eventModel.ReplyDeadline,
                FirstReminderMail = eventModel.FirstReminderMail,
                SecondReminderMail = eventModel.SecondReminderMail,
                AcceptanceEvent = eventModel.AcceptanceEvent,
                Address = addressViewModel
            };

            return eventViewModel;
        }

        private Event CreateEvent(CreateEventViewModel eventViewModel)
        {
            var address = new Address
            {
                City = eventViewModel.Address.City,
                HouseNumber = eventViewModel.Address.HouseNumber,
                StreetName = eventViewModel.Address.StreetName,
                Zip = eventViewModel.Address.Zip
            };

            var createEvent = new Event
            {
                DateCreated = DateTime.Now,
                Title = eventViewModel.Title,
                Description = eventViewModel.Description,
                DateEdited = DateTime.Now,
                Date = eventViewModel.Date,
                Time = eventViewModel.Time,
                ReplyDeadline = eventViewModel.ReplyDeadline,
                FirstReminderMail = eventViewModel.FirstReminderMail,
                SecondReminderMail = eventViewModel.SecondReminderMail,
                AcceptanceEvent = eventViewModel.AcceptanceEvent,
                Address = address,
                Participants = new List<Participant>()
            };

            return createEvent;
        }

        private Event UpdateEvent(Event eventModel, CreateEventViewModel eventViewModel)
        {
            eventModel.Address.City = eventViewModel.Address.City;
            eventModel.Address.HouseNumber = eventViewModel.Address.HouseNumber;
            eventModel.Address.StreetName = eventViewModel.Address.StreetName;
            eventModel.Address.Zip = eventViewModel.Address.Zip;

            eventModel.Date = eventViewModel.Date;
            eventModel.Time = eventViewModel.Time;
            eventModel.Description = eventViewModel.Description;
            eventModel.ReplyDeadline = eventViewModel.ReplyDeadline;
            eventModel.FirstReminderMail = eventViewModel.FirstReminderMail;
            eventModel.SecondReminderMail = eventViewModel.SecondReminderMail;
            eventModel.Title = eventViewModel.Title;
            eventModel.AcceptanceEvent = eventViewModel.AcceptanceEvent;

            eventModel.DateEdited = DateTime.Now;

            return eventModel;
        }

        private void CreateEmailsToBeSent(Event eventModel)
        {
            // Replace wildcards in Email Text
            var description = eventModel.Description;

            description = description.Replace("%%Datum%%", eventModel.Date.ToShortDateString());
            description = description.Replace("%%Uhrzeit%%", eventModel.Date.ToShortTimeString());
            
            foreach (var participant in eventModel.Participants)
            {
                // Check if participant already got an email
                var sentmail = db.ScheduledEmails.FirstOrDefault(m => m.ParticipantId == participant.ParticipantId);
                if(sentmail != null) continue;

                var email = new ScheduledEmail();
                email.EmailSent = false;
                email.EmailText = description;
                email.EventId = eventModel.EventId;
                email.MemberId = participant.Member.MemberId;
                email.ParticipantId = participant.ParticipantId;
                email.AnswerCode = participant.SecurityCode;
                email.ScheduledDate = DateTime.Now;
                db.ScheduledEmails.Add(email);

                var reminderMail = new ScheduledEmail();
                reminderMail.EmailSent = false;
                reminderMail.EmailText = description;
                reminderMail.EventId = eventModel.EventId;
                reminderMail.MemberId = participant.Member.MemberId;
                reminderMail.ParticipantId = participant.ParticipantId;
                reminderMail.AnswerCode = participant.SecurityCode;
                reminderMail.ScheduledDate = eventModel.FirstReminderMail;
                
                db.ScheduledEmails.Add(reminderMail);
                db.SaveChanges();
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
