using Microsoft.Extensions.Logging;
using SizeWebViewToContent.Controls;
using SizeWebViewToContent.Handlers;

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
#if IOS
                handlersCollection.AddHandler<ContentSizedWebView, ContentSizedWebViewHandler>();
#endif
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}