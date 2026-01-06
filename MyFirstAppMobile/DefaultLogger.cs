using MyFirstAppMobile.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAppMobile
{
    public class DefaultLogger : IPlatformLogger
    {
        public void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
