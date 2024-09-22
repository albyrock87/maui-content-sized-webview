using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using WebView = Android.Webkit.WebView;

namespace SizeWebViewToContent.Handlers;

public class ContentSizedWebChromeClient : MauiWebChromeClient
{
    public ContentSizedWebChromeClient(IWebViewHandler handler) : base(handler)
    {
    }

    public override void OnProgressChanged(WebView? view, int newProgress)
    {
        base.OnProgressChanged(view, newProgress);
        if (newProgress == 100)
        {
            view?.EvaluateJavascript(ContentSizeObserverBridge.InitializerScript, null);
        }
    }
}