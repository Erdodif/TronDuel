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
            _ = mauiAppBuilder.Services.AddSingleton<GVMService>();
            return mauiAppBuilder;
        }

        private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            _ = mauiAppBuilder.Services.AddTransient<GameSetupViewModel>();
            _ = mauiAppBuilder.Services.AddTransient<GameViewModel>();
            //_ = mauiAppBuilder.Services.AddTransient<MapViewModel>();
            //_ = mauiAppBuilder.Services.AddTransient<TileViewModel>();
            return mauiAppBuilder;
        }

        private static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
        {
            _ = mauiAppBuilder.Services.AddSingleton<AppShell>();
            _ = mauiAppBuilder.Services.AddSingleton<Greeter>();
            _ = mauiAppBuilder.Services.AddSingleton<Game>();
            return mauiAppBuilder;
        }

        private static MauiAppBuilder RegisterModels(this MauiAppBuilder mauiAppBuilder)
        {
            //_ = mauiAppBuilder.Services.AddTransient<Item>();
            return mauiAppBuilder;
        }

    }
}
