using Microsoft.Extensions.Logging;
using SizeWebViewToContent.Controls;

namespace SizeWebViewToContent;

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
            })
            .ConfigureMauiHandlers(handlersCollection =>
            {
#if IOS || ANDROID
                handlersCollection.AddHandler<ContentSizedWebView, Handlers.ContentSizedWebViewHandler>();
#endif
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}