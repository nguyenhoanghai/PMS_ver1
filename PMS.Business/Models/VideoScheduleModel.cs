using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class VideoScheduleModel : P_PlayVideoShedule
    {
        public List<P_PlayVideoSheduleDetail> Detail { get; set; }
        public VideoScheduleModel()
        {
            Detail = new List<P_PlayVideoSheduleDetail>();
        }
    }
}
