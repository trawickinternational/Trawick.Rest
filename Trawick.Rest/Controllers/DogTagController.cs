using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Trawick.Rest.Controllers
{
    public class DogTagController : Controller
    {
        // GET: DogTag
        public ActionResult AltitudeFactors()
        {
            var model = Trawick.Data.Models.QuoteRates.DogTagRatesRepo.DogTag_GetAltitudeFactors();
            if (model != null)
            {

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult TourOperators()
        {
            var model = Trawick.Data.Models.DogTagRepo.TourOperator_GetAll().OrderBy(m=>m.Name);
            if (model != null)
            {

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
}