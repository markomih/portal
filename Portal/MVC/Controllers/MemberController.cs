﻿using System.Web.Mvc;
using MVC.ViewModels.Member;
using MVC.ViewModels;

namespace MVC.Controllers
{
    [AuthorizeMember]
    public class MemberController : Controller
    {
        // GET: MemberProfile
        public ActionResult Index()
        {
            MemberIndexViewModel model = MemberIndexViewModel.Load(MemberSession.GetMemberId());
            return View(model);
        }
        public ActionResult AllMembers()
        {
            var model = Data.Entities.Members.GetMemberThumbnails();
            return View(model);
        }

        public ActionResult Profile(int id)
        {
            MemberProfileViewModel model = MemberProfileViewModel.Load(id);
            return View(model);
        }

        public ActionResult Edit()
        {
            EditProfileViewModel model = EditProfileViewModel.Load(MemberSession.GetMemberId());
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditProfileViewModel m)
        {
            int memberId = MemberSession.GetMemberId();
            Data.Entities.Members.EditProfile(memberId, m.Name, m.Surname, m.Nickname,
                                              m.Faculty, m.DateOfBirth, m.Status, m.Phone,
                                              m.Facebook, m.LinkedIn, m.Skype);
            return RedirectToAction("Profile", new { id = memberId });
        }
    }
}