using Microsoft.Extensions.Logging;

using TronDuel.View;
using TronDuel.ViewModel;

namespace TronDuel
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            _ = builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .RegisterAppServices()
                .RegisterViewModels()
                .RegisterViews()
                .RegisterModels();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
        {
            //_ = mauiAppBuilder.Services.AddTransient<Item>();
            return mauiAppBuilder;
        }

        private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            _ = mauiAppBuilder.Services.AddTransient<GameSetupViewModel>();
            _ = mauiAppBuilder.Services.AddTransient<GameViewModel>();

            return mauiAppBuilder;
        }

        private static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
        {
            //_ = mauiAppBuilder.Services.AddTransient<Item>();

            _ = mauiAppBuilder.Services.AddSingleton<AppShell>();
            _ = mauiAppBuilder.Services.AddTransient<Greeter>();
            _ = mauiAppBuilder.Services.AddTransient<Game>();
            return mauiAppBuilder;
        }

        private static MauiAppBuilder RegisterModels(this MauiAppBuilder mauiAppBuilder)
        {
            //_ = mauiAppBuilder.Services.AddTransient<Item>();

            return mauiAppBuilder;
        }

    }
}
