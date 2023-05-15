using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WpfTemplate.Models
{
    public class PersonModel : IDataErrorInfo
    {
        private string _name;
        public string Name 
        { 
            get { return _name; } 
            set { _name = value; } 
        }

        public int Age { get; set; }

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                        result = "Name must be filled out";
                }
                return result;
            }
        }
    }
}
