using MyFirstAppMobile.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAppMobile;

public class LifecycleLogger
{
    private readonly IPlatformLogger _logger;

    public LifecycleLogger(IPlatformLogger logger)
    {
        _logger = logger;
    }

    public void Log(string messsage) => _logger.Log(messsage);
}
