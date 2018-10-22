using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMS.Data;

namespace PMS.Business.Models
{
   public class ConfigModel
    {
        public List<ShowLCD_Config> Configs { get; set; }
        public List<ShowLCD_Panel> Panels { get; set; }
        public List<ShowLCD_LabelForPanelContent> LabelNames { get; set; }
        public List<ShowLCD_TableLayoutPanel> ColumnConfigs { get; set; }
        public List<ShowLCD_LabelArea> LabelConfigs { get; set; }
        public ConfigModel()
        {
            Configs = new List<ShowLCD_Config>();
            Panels = new List<ShowLCD_Panel>();
            LabelNames = new List<ShowLCD_LabelForPanelContent>();
            ColumnConfigs = new List<ShowLCD_TableLayoutPanel>();
            LabelConfigs = new List<ShowLCD_LabelArea>();
        }
    }
}
