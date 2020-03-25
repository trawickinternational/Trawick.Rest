using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trawick.Data.Models;

namespace Trawick.Rest.Controllers
{
    public class GBGClaimsController : Controller
    {
        // GET: GBGClaims
        public ActionResult ClaimsByMember(int memberId)
        {


            var claims = CBPClaimsRepo.GetClaimsByMemberId(memberId);
            return Json(claims, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ClaimsByMemberDate(int memberId, DateTime startDate, DateTime endDate)
        {
            var claims = CBPClaimsRepo.GetClaimsByMemberId(memberId);
            var ret = claims.Where(m => m.ClaimDetails.Any(t => t.DateOfLoss >= startDate && t.DateOfLoss <= endDate)).ToList();
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
    }
}