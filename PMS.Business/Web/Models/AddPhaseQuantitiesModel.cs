using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Web.Models
{
   public class AddPhaseQuantitiesModel
    {
        public int Id { get; set; }
        public int NangSuatId { get; set; }
        public int PhaseId { get; set; }
        public int Quantity { get; set; }
        public int CommandTypeId { get; set; }
        public string strCommandType { get; set; }
        public string strDate { get; set; }
        public DateTime Date { get; set; }
        public string LineName { get; set; }
        public int AssignId { get; set; }
    }
}
