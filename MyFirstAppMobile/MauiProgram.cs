using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using MyFirstAppMobile.Interface;


namespace MyFirstAppMobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

            var builder = MauiApp.CreateBuilder();
#if ANDROID
            builder.Services.AddSingleton<IPlatformLogger, Platforms.Android.AndroidLogger>();
#else
            builder.Services.AddSingleton<IPlatformLogger, DefaultLogger>();
#endif
            builder.Services.AddSingleton<LifecycleLogger>();

            builder.ConfigureLifecycleEvents(events =>
            {
#if ANDROID
            events.AddAndroid(android =>
            {
                android.OnCreate((activity, bundle) =>
                    builder.Services.BuildServiceProvider()
                        .GetRequiredService<IPlatformLogger>()
                        .Log("Activity OnCreate"));
            });
#endif
            });

            return builder.Build();
        }
    }
}
