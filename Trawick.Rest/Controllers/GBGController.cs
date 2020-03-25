using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Trawick.Rest.Controllers
{
    public class GBGController : Controller
    {
        // GET: GBG
        public ActionResult MissingTransactions()
        {
            var cont = new Trawick.Enrollment.Data.siadminEntities();
            string filename = "c:\\Temp\\missing.txt";
            var model = cont.Dobbins_GBGNotSent.Select(m => new { ID = m.ID, SubmitJson = m.SubmitXml });

            FileInfo info = new FileInfo(filename);
            if (!info.Exists)
            {
                MemoryStream fs = new MemoryStream();
               using (StreamWriter writer = new StreamWriter(fs))
                {
                    foreach (var item in model)
                    {
                        writer.WriteLine(item.SubmitJson);
                    }
                    fs.Flush();

                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, (int)fs.Length);
                    string contentType = "text/plain";

                    var cd = new System.Net.Mime.ContentDisposition
                    {
                        FileName = filename,
                        Inline = true,
                    };

                    Response.AppendHeader("Content-Disposition", cd.ToString());

                    return File(bytes, contentType);
                }
            }
            return null;
        }
    }
}