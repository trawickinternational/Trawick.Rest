using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Trawick.Data.Models;
using Trawick.Enrollment.Data;
using Trawick.Rest.Extensions;
using Trawick.Rest.Helpers;

namespace Trawick.Rest.Controllers
{
	public class CreditCardController : Controller
	{



		// GET: CreditCard
		public ActionResult Index()
		{
			return View();
		}



	}


	public class CreditCardInfo
	{
		public string cardNumber { get; set; }
		public string expirationDate { get; set; }
		public string cardCode { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string address { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public string zip { get; set; }
		public string TotalAmount { get; set; }
	}

	public class ProcessCreditCardResponse
	{
		public ProcessCreditCardResponse() : base() { }
		public ProcessCreditCardResponse(string responseText, string responseCode, string transactionId, string cvvResponse, string authcode, bool success)
				: this()
		{
			this.ResponseText = responseText;
			this.ResponseCode = responseCode;
			this.TransactionId = transactionId;
			this.CvvResponse = cvvResponse;
			this.AuthCode = authcode;
			this.Success = success;
		}

		public string ResponseText { get; set; }
		public string ResponseCode { get; set; }
		public string TransactionId { get; set; }
		public string CvvResponse { get; set; }
		public string AuthCode { get; set; }
		public bool Success { get; set; }
	}

}