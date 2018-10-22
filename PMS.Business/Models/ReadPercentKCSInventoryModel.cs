using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class ReadPercentKCSInventoryModel: P_ReadPercent_KCSInventory
    {
        public List<P_ReadPercent_KCSInventory_De> Childs { get; set; }

        public ReadPercentKCSInventoryModel()
        {
            Childs = new List<P_ReadPercent_KCSInventory_De>();
        }
    }
}
