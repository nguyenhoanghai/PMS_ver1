using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuanLyNangSuat.Model
{
    public class ModelResult
    {
        public ModelResult()
        {
            this.ListError = new List<ModelError>();
        }
        public bool IsSuccsess { get; set; }
        public List<ModelError> ListError { get; set; }
    }

    public class ModelError
    {
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string ErrorContent { get; set; }
    }
}
