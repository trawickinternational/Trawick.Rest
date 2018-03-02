using System.Web.Mvc;

using Trawick.Data.Models;

namespace Trawick.Rest.Controllers
{
	public class ProviderController : Controller
	{

		public ActionResult GetProviders(string search, string zip, int distance, string type = null)
		{
			if (string.IsNullOrEmpty(search))
			{
				search = "ALL";
			}
			var providers = ProviderRepo.Search_Providers(search, zip, distance, type);
			if (providers != null)
			{
				return Json(providers, JsonRequestBehavior.AllowGet);
			}
			return Json("No providers found in the given area", JsonRequestBehavior.AllowGet);
		}



		public ActionResult GetProvidersIntl(string search, string country, string city)
		{
			if (string.IsNullOrEmpty(search))
			{
				search = "ALL";
			}
			var providers = ProviderRepo.Search_ProvidersIntl(search, country, city);
			if (providers != null)
			{
				return Json(providers, JsonRequestBehavior.AllowGet);
			}
			return Json("No providers found in the given area", JsonRequestBehavior.AllowGet);
		}



		public ActionResult GetDetails(int pKey)
		{
			var details = ProviderRepo.Provider_DetailsByKey(pKey);
			if (details != null)
			{
				return Json(details, JsonRequestBehavior.AllowGet);
			}
			return Json("No details found for that provider", JsonRequestBehavior.AllowGet);
		}



		//public ActionResult GetTypeList()
		//{
		//	DataSet ds = dg.GetDataSet("SELECT Type FROM GBGTypeXRef WHERE NOT Type IS NULL ORDER BY Type", "TypeList");
		//	this.Context.Response.ContentType = "application/json; charset=utf-8";
		//	this.Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
		//	this.Context.Response.Write(JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.Indented));
		//}



		public ActionResult GetCountryList()
		{
			var countries = ProviderRepo.Provider_GetCountryList();
			if (countries != null)
			{
				return Json(countries, JsonRequestBehavior.AllowGet);
			}
			return Json("No countries found", JsonRequestBehavior.AllowGet);
		}



		public ActionResult GetCityList(string country)
		{
			var cities = ProviderRepo.Provider_GetCityList(country);
			if (cities != null)
			{
				return Json(cities, JsonRequestBehavior.AllowGet);
			}
			return Json("No cities found", JsonRequestBehavior.AllowGet);
		}



		public ActionResult GetSpecList(string type)
		{
			if (string.IsNullOrEmpty(type))
			{
				type = "ALL";
			}
			var specs = ProviderRepo.Provider_GetSpecListByType(type);
			if (specs != null)
			{
				return Json(specs, JsonRequestBehavior.AllowGet);
			}
			return Json("No specs found", JsonRequestBehavior.AllowGet);
		}



		public ActionResult GetSpecListIntl(string country, string city)
		{
			var specs = ProviderRepo.Provider_GetSpecListByLocale(country, city);
			if (specs != null)
			{
				return Json(specs, JsonRequestBehavior.AllowGet);
			}
			return Json("No specs found", JsonRequestBehavior.AllowGet);
		}



		public string GetCountryListXML()
		{
			return ProviderRepo.Provider_GetCountryListXml().ToString();
		}


	}
}