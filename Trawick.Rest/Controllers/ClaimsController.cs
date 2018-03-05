using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Trawick.Data.Models;
using Trawick.Enrollment.Data;
using Trawick.Rest.Extensions;
using Trawick.Rest.Helpers;

namespace Trawick.Rest.Controllers
{
	public class ClaimsController : Controller
	{

		public ActionResult GetClaimsHeaders(int userID, DateTime startDate, DateTime endDate, Guid authCode)
		{
			if (!AuthTicketRepo.AuthTicket_IsValid(authCode))
			{
				return Json("invalid auth code", JsonRequestBehavior.AllowGet);
			}

			var claims = ClaimsRepo.Claims_GetByUserIdAndRange(userID, startDate, endDate);
			if (claims != null)
			{
				return Json(claims, JsonRequestBehavior.AllowGet);
			}

			return Json("claims not found", JsonRequestBehavior.AllowGet);
		}



		public ActionResult GetClaimsDetails(int claimNumber, Guid authCode)
		{
			if (!AuthTicketRepo.AuthTicket_IsValid(authCode))
			{
				return Json("invalid auth code", JsonRequestBehavior.AllowGet);
			}

			var claims = ClaimsRepo.Claims_GetByClaimNumber(claimNumber);
			if (claims != null)
			{
				return Json(claims, JsonRequestBehavior.AllowGet);
			}

			return Json("claims not found", JsonRequestBehavior.AllowGet);
		}

	}
}
