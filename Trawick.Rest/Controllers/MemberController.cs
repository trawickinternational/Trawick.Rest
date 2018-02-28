using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Trawick.Data.Models;
using Trawick.Rest.Helpers;

namespace Trawick.Rest.Controllers
{
	public class MemberController : Controller
	{


		public ActionResult InitRegistration(int userID, DateTime dob)
		{
			var member = MemberRepo.Member_GetByUserId(userID);
			if (member == null || member.dob != dob)
			{
				return Json("invalid member id", JsonRequestBehavior.AllowGet);
			}
			Guid authCode = AuthTicketRepo.AuthTicket_GetCode(member.member_id, member.membership_id);
			return Json(new AppMember(member, authCode), JsonRequestBehavior.AllowGet);
		}


		public ActionResult GetMemberInfo(int userID, Guid authCode)
		{
			if (!AuthTicketRepo.AuthTicket_IsValid(authCode))
			{
				return Json("invalid auth code", JsonRequestBehavior.AllowGet);
			}
			var member = MemberRepo.Member_GetByUserId(userID);
			if (member != null)
			{
				return Json(new AppMember(member, authCode), JsonRequestBehavior.AllowGet);
			}
			return Json("member not found", JsonRequestBehavior.AllowGet);
		}


		public ActionResult Register(int userID, string emailAddr, string password, Guid authCode)
		{
			if (!AuthTicketRepo.AuthTicket_IsValid(authCode))
			{
				return Json("invalid auth code", JsonRequestBehavior.AllowGet);
			}

			RegexUtilities util = new RegexUtilities();
			Regex rx = new Regex(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,11})");


			if (!rx.IsMatch(password))
			{
				return Json("invalid password", JsonRequestBehavior.AllowGet);
			}

			if (!util.IsValidEmail(emailAddr))
			{
				return Json("invalid email/username", JsonRequestBehavior.AllowGet);
			}

			var member = MemberRepo.Member_GetByUserId(userID);
			if (member == null)
			{
				return Json("member not found", JsonRequestBehavior.AllowGet);
			}

			member.username = emailAddr;
			member.email2 = emailAddr;
			member.password = password;
			member.isClaimsReg = true;

			try
			{
				MemberRepo.Member_UpdateMember(member);
				//SendEmailConfirm(userID);
				return Json("register successful", JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json("register error - " + ex.Message, JsonRequestBehavior.AllowGet);
			}
		}


		public ActionResult LoginGetAuth(string username, string password)
		{
			var member = MemberRepo.Member_Login(username, password);
			if (member == null)
			{
				return Json("member not found", JsonRequestBehavior.AllowGet);
			}
			Guid authCode = AuthTicketRepo.AuthTicket_GetCode(member.member_id, member.membership_id);
			if (authCode != null)
			{
				return Json(new { MemberID = member.userid, AuthCode = authCode}, JsonRequestBehavior.AllowGet);
			}
			return Json("bad login", JsonRequestBehavior.AllowGet);
		}

































	}


	class AppMember
	{
		public int member_id { get; set; }
		public int? userid { get; set; }
		public Guid membership_id { get; set; }
		public string firstname { get; set; }
		public string middlename { get; set; }
		public string lastname { get; set; }
		public DateTime? dob { get; set; }
		public string address1 { get; set; }
		public string address2 { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public string zip { get; set; }
		public string country { get; set; }
		public string email1 { get; set; }
		public string phone1 { get; set; }
		public string passport { get; set; }
		public Guid auth_ticket_id { get; set; }

		public AppMember(Member member, Guid authCode)
		{
			member_id = member.member_id;
			userid = member.userid;
			membership_id = member.membership_id;
			firstname = member.firstname;
			middlename = member.middlename;
			lastname = member.lastname;
			dob = member.dob;
			address1 = member.address1;
			address2 = member.address2;
			city = member.city;
			state = member.state;
			zip = member.zip;
			country = CountryRepo.Country_GetNameById((int)member.country_id);
			email1 = member.email1;
			phone1 = member.phone1;
			passport = member.passport;
			auth_ticket_id = authCode;
		}

	}
}