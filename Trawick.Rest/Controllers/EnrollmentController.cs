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
	public class EnrollmentController : Controller
	{

		public ActionResult GetEnrollmentCount(int userID, Guid authCode)
		{
			if (!AuthTicketRepo.AuthTicket_IsValid(authCode))
			{
				return Json("invalid auth code", JsonRequestBehavior.AllowGet);
			}
			var enrollments = EnrollmentRepo.App_GetByUserId(userID);
			return Json(enrollments.Count(), JsonRequestBehavior.AllowGet);
		}



		public ActionResult GetEnrollments(int userID, Guid authCode)
		{
			if (!AuthTicketRepo.AuthTicket_IsValid(authCode))
			{
				return Json("invalid auth code", JsonRequestBehavior.AllowGet);
			}
			var enrollments = EnrollmentRepo.App_GetByUserId(userID);
			if (enrollments != null)
			{
				return Json(enrollments, JsonRequestBehavior.AllowGet);
			}
			return Json("no enrollments found", JsonRequestBehavior.AllowGet);
		}



		public ActionResult GetAgentContact(int userID)
		{
			var member = MemberRepo.Member_GetByUserId(userID);
			if (member == null)
			{
				return Json("invalid member id", JsonRequestBehavior.AllowGet);
			}
			var agent = AgentRepo.Agent_GetByUserId(userID);
			if (agent != null)
			{
				return Json(agent, JsonRequestBehavior.AllowGet);
			}
			return Json("no agents found", JsonRequestBehavior.AllowGet);
		}



		public ActionResult GetCertificate(int userID, Guid authCode)
		{
			return GetDocument(userID, authCode, "Certificate");
		}



		public ActionResult GetIDCard(int userID, Guid authCode)
		{
			return GetDocument(userID, authCode, "Id Card");
		}



		public ActionResult GetVisaLetter(int userID, Guid authCode)
		{
			return GetDocument(userID, authCode, "Visa Letter");
		}



		private ActionResult GetDocument(int userID, Guid authCode, string docName)
		{
			if (!AuthTicketRepo.AuthTicket_IsValid(authCode))
			{
				return Json("invalid auth code", JsonRequestBehavior.AllowGet);
			}
			var enrollments = EnrollmentRepo.App_GetByUserId(userID);
			if (enrollments == null)
			{
				return Json("no enrollments found", JsonRequestBehavior.AllowGet);
			}
			if (enrollments.Count() > 1)
			{
				return Json(enrollments, JsonRequestBehavior.AllowGet);
			}
			var enroll = enrollments.First();
			WebClient wc = new WebClient();
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
			string service = string.Format("https://orders.trawickinternational.com/Retrieve{0}.ashx", docName.Replace(" ", ""));
			byte[] fileBytes = wc.DownloadData(service + enroll.DocumentQuery);
			if (fileBytes?.Length > 0)
			{
				string fileName = string.Format("{0}_{1}.pdf", enroll.PolicyName.Replace(" ", "-"), docName.Replace(" ", "-"));
				return File(fileBytes, "application/pdf", fileName);
			}
			return Json("problem retrieving document", JsonRequestBehavior.AllowGet);
		}

	}
}