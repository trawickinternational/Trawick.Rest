using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;



namespace Trawick.Rest.Controllers
{
    public class AgentController : Controller
    {

        public ActionResult Agent_GetAgentInfo(int AgentId)
        {
            var model = Trawick.Data.Models.ContactRepo.Agent_GetAgentInfo(AgentId);
            if (model != null)
            {
                var jS = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                return Json(jS, JsonRequestBehavior.AllowGet);
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Agent_GetAgentInfoJson(int AgentId)
        {
            var model = Trawick.Data.Models.ContactRepo.Agent_GetAgentInfo(AgentId);
            if (model != null)
            {
             
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Agent_GetAllJson()
        {
            var model = Trawick.Data.Models.AgentRepo.GetAllActive();

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}