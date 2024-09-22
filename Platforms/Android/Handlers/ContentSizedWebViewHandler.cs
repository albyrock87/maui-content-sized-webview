using Microsoft.Maui.Handlers;
using WebView = Android.Webkit.WebView;

namespace SizeWebViewToContent.Handlers;

public class ContentSizedWebViewHandler : WebViewHandler
{
    private Size _contentSize;

    public new static IPropertyMapper<IWebView, IWebViewHandler> Mapper = new PropertyMapper<IWebView, IWebViewHandler>(WebViewHandler.Mapper)
    {
        [nameof(WebView.WebViewClient)] = MapContentSizedWebViewClient,
        [nameof(WebView.WebChromeClient)] = MapContentSizedWebChromeClient
    };

    public static void MapContentSizedWebChromeClient(IWebViewHandler handler, IWebView webView)
    {
        if (handler is ContentSizedWebViewHandler platformHandler)
        {
            handler.PlatformView.SetWebChromeClient(new ContentSizedWebChromeClient(platformHandler));
        }
    }

    public static void MapContentSizedWebViewClient(IWebViewHandler handler, IWebView webView)
    {
        if (handler is ContentSizedWebViewHandler platformHandler)
        {
            handler.PlatformView.SetWebViewClient(new ContentSizedWebViewClient(platformHandler));
        }
    }
    
    public new Controls.ContentSizedWebView VirtualView => (Controls.ContentSizedWebView)base.VirtualView;

    public ContentSizedWebViewHandler() : base(Mapper)
    {
    }
    
    protected override WebView CreatePlatformView()
    {
        var platformView = base.CreatePlatformView();
        platformView.AddJavascriptInterface(new ContentSizeObserverBridge(this), nameof(ContentSizedWebViewHandler));
        
        return platformView;
    }

    public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
    {
        var scrollViewContentWidth = _contentSize.Width;
        var scrollViewContentHeight = _contentSize.Height;
        double width;
        double height;

        if (widthConstraint <= 0 || double.IsInfinity(widthConstraint))
        {
            width = scrollViewContentWidth;
        }
        else
        {
            width = Math.Min(widthConstraint, scrollViewContentWidth);
        }

        if (heightConstraint <= 0 || double.IsInfinity(heightConstraint))
        {
            height = scrollViewContentHeight;
        }
        else
        {
            height = Math.Min(heightConstraint, scrollViewContentHeight);
        }

        return new Size(width, height);
    }

    public void OnContentSizeChanged(Size size)
    {
        _contentSize = size;
        Invoke(nameof(IView.InvalidateMeasure), null);
    }
}