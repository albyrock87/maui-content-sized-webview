using Foundation;
using Microsoft.Maui.Handlers;
using WebKit;

namespace SizeWebViewToContent.Handlers;

public class ContentSizedWebViewHandler : WebViewHandler
{
    private IDisposable? _contentSizeObserver;

    protected override void ConnectHandler(WKWebView platformView)
    {
        base.ConnectHandler(platformView);

        var weakHandler = new WeakReference<ContentSizedWebViewHandler>(this);
        _contentSizeObserver = platformView.ScrollView.AddObserver(
            "contentSize",
            NSKeyValueObservingOptions.New,
            _ =>
            {
                if (weakHandler.TryGetTarget(out var handler))
                {
                    handler.Invoke(nameof(IView.InvalidateMeasure), null);
                }
            }
        );
    }

    protected override void DisconnectHandler(WKWebView platformView)
    {
        _contentSizeObserver?.Dispose();
        _contentSizeObserver = null;

        base.DisconnectHandler(platformView);
    }

    public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
    {
        var scrollViewContentSize = PlatformView.ScrollView.ContentSize;
        double width;
        double height;

        if (widthConstraint <= 0 || double.IsInfinity(widthConstraint))
        {
            width = scrollViewContentSize.Width;
        }
        else
        {
            width = Math.Min(widthConstraint, scrollViewContentSize.Width);
        }

        if (heightConstraint <= 0 || double.IsInfinity(heightConstraint))
        {
            height = scrollViewContentSize.Height;
        }
        else
        {
            height = Math.Min(heightConstraint, scrollViewContentSize.Height);
        }

        return new Size(width, height);
    }
}