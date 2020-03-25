using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trawick.Data.Models;

namespace Trawick.Rest.Controllers
{
    public class TravelRosterController : Controller
    {
        // GET: TravelRoster
        public ActionResult ApproveTravelRoster(int id)
        {
            string error = "";
            var Roster = TravelRosterRepo.Roster_GetById(id);

            if (Roster != null)
            {
                try
                {
                    Roster.ApproveRoster();

                    var Invoice = Trawick.Data.Models.TravelRosterInvoiceRepo.Invoice_GetByRosterId(id).FirstOrDefault();

                    if (Invoice != null)
                    {
                        var args = TravelRosterInvoice.GetInvoiceOnApprovalEmailArgs(Invoice.ID);
                        if (!string.IsNullOrEmpty(args.EmailTo))
                            Trawick.Email.EmailHelpers.PortalContact.SendPortalContact(args);

                        return Json( new { id = Invoice.ID });
                    }

                    error = "Error Approving Roster.";
                }
                catch (Exception e)
                {
                    // do something
                    error = "Error: " + (e.InnerException != null ? e.InnerException.Message : e.Message);
                }
            }

            return Json( new { id = 0, errMessage = error });
        }
    }
}