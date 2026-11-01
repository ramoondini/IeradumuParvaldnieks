using Microsoft.Extensions.Logging;
using IeradumuParvaldnieks.Services;

namespace IeradumuParvaldnieks
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<HttpClient>(_ => new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7239/")
            });

            builder.Services.AddTransient<IeradumuApiService>();

            return builder.Build();
        }
    }
}
