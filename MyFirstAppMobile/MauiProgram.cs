using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using MyFirstAppMobile.Data;
using MyFirstAppMobile.Interface;
using MyFirstAppMobile.ViewModels;


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
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "fitness.db3");
            builder.Services.AddSingleton<IEntriesRepository>(_ => new SqliteEntriesRepository(dbPath));

            builder.Services.AddSingleton<EntriesViewModel>();
            builder.Services.AddTransient<EntryFormViewModel>();
            builder.Services.AddTransient<EntryDetailsViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<EntryPage>();
            builder.Services.AddTransient<DetailsEntryPage>();

            builder.UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
