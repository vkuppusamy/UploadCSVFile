using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using UploadCSVFile.Models;
using System.Globalization;
using System.Text;

namespace UploadCSVFile.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase FileUpload)
        {
            List<UploadCSVModel> dealerRecords = new List<UploadCSVModel>();
            if (ModelState.IsValid)
            {
                string uploadPath = string.Empty;
                if (FileUpload != null && FileUpload.ContentLength > 0)
                {

                    string folderPath = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    uploadPath = folderPath + Path.GetFileName(FileUpload.FileName);
                    string extension = Path.GetExtension(FileUpload.FileName);
                    FileUpload.SaveAs(uploadPath);

                    //Read the contents of CSV file.
                    string[] csvRows = System.IO.File.ReadAllLines(uploadPath, Encoding.Default);
                    Boolean rowHeader = false;
                    //Execute a loop over the rows.
                    foreach (string row in csvRows)
                    {
                        if (rowHeader)
                        {
                            if (!string.IsNullOrEmpty(row))
                            {
                                
                                string[] columns = ReadColumns(row);
                                dealerRecords.Add(new UploadCSVModel
                                {
                                    DealNumber = Convert.ToInt32(columns[0]),
                                    CustomerName = columns[1],
                                    DealershipName = columns[2],
                                    Vehicle = columns[3],
                                    //Price = Convert.ToDouble(columns[4]),
                                    Price = columns[4],
                                    DealDate = columns[5]
                                    //DealDate = Convert.ToDateTime(columns[5], CultureInfo.InvariantCulture)
                                    //DealDate = DateTime.ParseExact(columns[5], "M/d/yyyy", CultureInfo.InvariantCulture)
                                });
                            }
                        }
                        rowHeader = true;
                    }
                }
            }
            else
            {
                ModelState.AddModelError("File", "Please Upload Your file");
            }
            return View(dealerRecords);
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

        private string[] ReadColumns(string row)
        {
            string[] rowColumns = new string[6];

            if (String.IsNullOrEmpty(row))
                return null;

            int pos = 0;
            int index = 0;
            string value;

            while (pos < row.Length)
            {
                // Special handling for quoted field
                if (row[pos] == '"')
                {
                    pos++;

                    // Parse quoted value
                    int start = pos;
                    while (pos < row.Length)
                    {
                        // Test for quote character
                        if (row[pos] == '"')
                        {
                            // Found one
                            pos++;

                            // If two quotes together, keep one
                            // Otherwise, indicates end of value
                            if (pos >= row.Length || row[pos] != '"')
                            {
                                pos--;
                                break;
                            }
                        }
                        pos++;
                    }
                    value = row.Substring(start, pos - start);
                    value = value.Replace("\"\"", "\"");
                }
                else
                {
                    // Parse unquoted value
                    int start = pos;
                    while (pos < row.Length && row[pos] != ',')
                        pos++;
                    value = row.Substring(start, pos - start);
                }

                // Add field to list
                if (index <= 5)
                {
                    rowColumns[index] = value;
                    index++;
                }

                // Eat up to and including next comma
                while (pos < row.Length && row[pos] != ',')
                    pos++;
                if (pos < row.Length)
                    pos++;
            }

            // Return true if any columns read
            return rowColumns;
        }
    }
}