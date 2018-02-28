using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trawick.Data.Models;
namespace Trawick.Rest.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member
        public ActionResult GetMemberInfo(int userID, string authCode)
        {

            return Json(MemberRepo.Member_GetByUserId(userID), JsonRequestBehavior.AllowGet);
        }
    }
}