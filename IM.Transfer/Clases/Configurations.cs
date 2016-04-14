using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace IM.Transfer.Clases
{
    class Configurations
    {
        public static string getValueKey(string key)
        {
            string filetype = new AppSettingsReader().GetValue(key, typeof(System.String)).ToString();
            //string filetype = ConfigurationSettings.AppSettings["StartTime"];
            return filetype;
        }
        
    }
}
