using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using lionsEventManagement.Models;

namespace lionsEventManagement.Controllers
{
    public class MemberManagementController : Controller
    {
        private EventManagementDbContext db = new EventManagementDbContext();

        // GET: MemberManagement
        public ActionResult Index()
        {
            return View(db.Members.ToList());
        }

        // GET: MemberManagement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
                return HttpNotFound();

            var memberViewModel = ViewMember(member);
            return View(memberViewModel);
        }

        // GET: MemberManagement/Create
        public ActionResult Create()
        {
            return View(new EditMemberViewModel());
        }

        // POST: MemberManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditMemberViewModel memberViewModel)
        {
            if (ModelState.IsValid)
            {
                var createMember = CreateMember(memberViewModel);

                db.Members.Add(createMember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(memberViewModel);
        }

        // GET: MemberManagement/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(new EditMemberViewModel(member));
        }

        // POST: MemberManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditMemberViewModel memberViewModel)
        {
            if (ModelState.IsValid)
            {
                var dbMember = db.Members.Find(memberViewModel.MemberId);
                var updateMember = UpdateMember(dbMember, memberViewModel);

                db.Entry(updateMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(memberViewModel);
        }

        // GET: MemberManagement/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Member member = db.Members.Find(id);
            if (member == null)
                return HttpNotFound();

            var memberViewModel = ViewMember(member);
            return View(memberViewModel);
        }

        // POST: MemberManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member dbMember = db.Members.Find(id);

            // Remove Scheduled Emails
            var scheduledEmails = db.ScheduledEmails.Where(s => s.MemberId == dbMember.MemberId);
            db.ScheduledEmails.RemoveRange(scheduledEmails);
            db.SaveChanges();

            // Remove Invites from Events
            var allEvents = db.Events.ToList();
            foreach (Event dbEvent in allEvents)
            {
                var participants = dbEvent.Participants.Where(p => p.Member.MemberId == dbMember.MemberId);
                foreach (var participant in participants)
                {
                    dbEvent.Participants.Remove(participant);
                }
                db.SaveChanges();
            }

            // Remove invites
            var participantList = db.Participants.Where(p => p.Member.MemberId == dbMember.MemberId);
            db.Participants.RemoveRange(participantList);
            db.SaveChanges();

            db.Members.Remove(dbMember);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private ViewMemberViewModel ViewMember(Member memberModel)
        {
            var memberViewModel = new ViewMemberViewModel
            {
                MemberId = memberModel.MemberId,
                FirstName = memberModel.FirstName,
                LastName = memberModel.LastName,
                Email = memberModel.Email
            };

            return memberViewModel;
        }

        private Member CreateMember(EditMemberViewModel memberViewModel)
        {
            var member = new Member
            {
                MemberId = memberViewModel.MemberId,
                DateCreated = DateTime.Now,
                DateEdited = DateTime.Now,
                FirstName = memberViewModel.FirstName,
                LastName = memberViewModel.LastName,
                Email = memberViewModel.Email
            };

            return member;
        }

        private Member UpdateMember(Member memberModel, EditMemberViewModel memberViewModel)
        {
            memberModel.FirstName = memberViewModel.FirstName;
            memberModel.LastName = memberViewModel.LastName;
            memberModel.Email = memberViewModel.Email;
            memberModel.DateEdited = DateTime.Now;
            
            return memberModel;
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
