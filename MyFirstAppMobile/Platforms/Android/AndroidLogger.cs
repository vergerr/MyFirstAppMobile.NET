using MyFirstAppMobile.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alog = Android.Util;

namespace MyFirstAppMobile.Platforms.Android
{
    public class AndroidLogger : IPlatformLogger
    {
        public void Log(string message)
        {
            Alog.Log.Debug("ANDROIDLOG", message);
        }
    }
}
