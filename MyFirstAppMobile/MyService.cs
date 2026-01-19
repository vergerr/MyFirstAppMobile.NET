using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAppMobile
{
    public sealed class MyService(ILogger<MyService> logger)
    {
        public void Invoke()
        {
            logger.LogInformation("A simple log message");
            logger.LogError("A {Parameter} log message", "formatted");
            logger.LogWarning(new EventId(1, nameof(Invoke)), "Message with EventId");
        }
    }
}
