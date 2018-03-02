﻿using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

using Trawick.Data.Models;
using Trawick.Rest.Extensions;
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



		public ActionResult UpdateMemberInfo(int userID, Guid authCode, string address1, string address2, string city, string state, string zip, string phone, string email, string passport)
		{
			if (!AuthTicketRepo.AuthTicket_IsValid(authCode))
			{
				return Json("invalid auth code", JsonRequestBehavior.AllowGet);
			}

			RegexUtilities util = new RegexUtilities();
			if (!util.IsValidEmail(email))
			{
				return Json("invalid email/username", JsonRequestBehavior.AllowGet);
			}

			var member = MemberRepo.Member_GetByUserId(userID);
			if (member == null)
			{
				return Json("member not found", JsonRequestBehavior.AllowGet);
			}

			member.TryUpdateProperty(x => x.address1, address1);
			member.TryUpdateProperty(x => x.address2, address2);
			member.TryUpdateProperty(x => x.city, city);
			member.TryUpdateProperty(x => x.state, state);
			member.TryUpdateProperty(x => x.zip, zip);
			member.TryUpdateProperty(x => x.phone1, phone);
			member.TryUpdateProperty(x => x.email1, email);
			member.TryUpdateProperty(x => x.passport, passport);

			try
			{
				MemberRepo.Member_UpdateMember(member);
				return Json("update successful", JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json("update error - " + ex.Message, JsonRequestBehavior.AllowGet);
			}
		}



		public ActionResult Register(int userID, Guid authCode, string email, string password)
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

			if (!util.IsValidEmail(email))
			{
				return Json("invalid email/username", JsonRequestBehavior.AllowGet);
			}

			var member = MemberRepo.Member_GetByUserId(userID);
			if (member == null)
			{
				return Json("member not found", JsonRequestBehavior.AllowGet);
			}

			member.username = email;
			member.email2 = email;
			member.password = password;
			member.isClaimsReg = true;

			try
			{
				MemberRepo.Member_UpdateMember(member);
				SendEmailConfirm(userID);
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
				return Json(new { userID = member.userid, authCode = authCode }, JsonRequestBehavior.AllowGet);
			}

			return Json("bad login", JsonRequestBehavior.AllowGet);
		}



		private static void SendEmailConfirm(int userID)
		{
			// Invisible code. You just have to trust me.
		}


	}
}