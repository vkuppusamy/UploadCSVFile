using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UploadCSVFile.Models
{
    public class UploadCSVModel
    {
        public int DealNumber { get; set; }
        public string CustomerName { get; set; }
        public string DealershipName { get; set; }
        public string Vehicle { get; set; }
        //public double Price { get; set; }
        public string Price { get; set; }
        //public DateTime DealDate { get; set; }
        public String DealDate { get; set; }
    }
}