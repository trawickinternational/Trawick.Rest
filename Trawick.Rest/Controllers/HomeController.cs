using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trawick.Rest.Helpers;

namespace Trawick.Rest.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index(string message)
		{
			if (!string.IsNullOrEmpty(message))
			{
				ViewBag.Message = message;
			}

			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}




		[HttpPost]
		public ActionResult TestPassword(FormData data)
		{
			string message = string.Empty;
			var pass = Valid.Password(data.Password, out message);


			return RedirectToAction("Index", new { message = message });
		}

	}



	public class FormData
	{
		public string Password { get; set; }
		//public int TextBoxIntData { get; set; }
		//public bool CheckboxData { get; set; }
	}

}