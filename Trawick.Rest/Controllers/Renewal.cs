using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;



namespace Trawick.Rest.Controllers
{
    public class RenewalController : Controller
    {

        public ActionResult DateInfo(int Id)
        {
            var model = Trawick.Data.Models.EnrollmentRepo.Renewal_GetDateInfo(Id);
            if (model != null)
            {

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
}