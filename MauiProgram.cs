namespace NextDesk.App
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Components.Authorization;
    using NextDesk.Classes.Client;
    using NextDesk.Classes.Client.App;
    using NextDesk.Classes.Client.BackgroundLoader;
    using NextDesk.Classes.Toast;
    using Serilog;
    using Serilog.Events;

    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            SetupSerilog();

            builder
                .UseMauiApp<App>()
                .Logging.AddSerilog();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();

            var baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7035" : "https://localhost:7035";
            var handler = GetPlatformMessageHandler();
            builder.Services.AddScoped(_ => new HttpClient(handler) { BaseAddress = new Uri(baseAddress) });
#else
            builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri("http://nextdesk.site/") });
#endif

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddScoped<MyHttpClient>();
            builder.Services.AddScoped<Crud>();
            builder.Services.AddScoped<ILocalStorage, AppLocalStorage>();
            builder.Services.AddScoped<ApplicationState>();
            builder.Services.AddScoped<ToastService>();
            builder.Services.AddScoped<BackgroundLoaderService>();

            builder.Services.AddAuthorizationCore(opt =>
                opt.FallbackPolicy = new AuthorizationPolicyBuilder()
                                            .RequireAuthenticatedUser()
                                            .Build()
            );
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

            return builder.Build();
        }

        public static HttpMessageHandler GetPlatformMessageHandler()
        {
            if (DeviceInfo.Platform != DevicePlatform.Android && DeviceInfo.Platform != DevicePlatform.iOS) return new HttpClientHandler();

            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    if (cert is not null && cert.Issuer.Equals("CN=localhost")) return true;
                    return errors == System.Net.Security.SslPolicyErrors.None;
                }
            };
        }

        private static void SetupSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Debug()
                .CreateLogger();
        }
    }
}