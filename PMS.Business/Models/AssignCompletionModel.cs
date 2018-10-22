using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
    public class AssignCompletionModel : P_AssignCompletion
    {
        public string IsFinishStr { get; set; }
        public string CommoName { get; set; }
    }
}
