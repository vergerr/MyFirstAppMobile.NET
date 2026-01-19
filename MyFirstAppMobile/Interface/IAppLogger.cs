using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAppMobile.Interface
{
    internal interface IAppLogger
    {
        void Info(string message);
        void Warn(Exception ex, string message);
        void Crit(Exception? ex,string message);
    }
}
