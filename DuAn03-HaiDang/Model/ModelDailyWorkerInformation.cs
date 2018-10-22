using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuanLyNangSuat.Model
{
    public class ModelDailyWorkerInformation
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float ProductivityWorker { get; set; }
        public int CountWorker { get; set; }
        public int STTLineProduct { get; set; }
    }
}
