﻿using MVC.Models;
using System.Web.Mvc;

namespace MVC.Controllers
{
    [AuthorizeMember]
    public class FratorController : Controller
    {
        public ActionResult Index(int? id = null)
        {
            CompanyModel model = CompanyModel.Load(MemberSession.GetMemberId());
            if (id != null)
            {
                if (MemberSession.GetRole() == Data.Enumerations.Role.Administrator || MemberSession.GetRole() == Data.Enumerations.Role.FR)
                {
                    model.Show = model.AllCompany.Find(x => x.CompanyId == id);
                }
                else
                {
                    model.Show = model.MyCompany.Find(x => x.CompanyId == id);
                }
            }
                return View(model);
        }

        public ActionResult Profile(int id)
        {
            return RedirectToAction("Index", new { id = id });
        }

        public ActionResult Like(int idm, int idc)
        {
            Data.Entities.Comments.Like(idm, idc);
            return RedirectToAction("Index");
        }

        public ActionResult Unlike(int idm, int idc)
        {
            Data.Entities.Comments.Unlike(idm, idc);
            return RedirectToAction("Index");
        }

        public ActionResult Add()
        {
            return View(new CompanyAddModel());
        }
    }
}