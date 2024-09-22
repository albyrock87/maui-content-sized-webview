using Android.Webkit;
using Java.Interop;

namespace SizeWebViewToContent.Handlers;

public class ContentSizeObserverBridge : Java.Lang.Object
{
    private readonly WeakReference<ContentSizedWebViewHandler> _weakHandler;
    private double _contentHeight;
    private double _contentWidth;

    internal ContentSizeObserverBridge(ContentSizedWebViewHandler handler)
    {
        _weakHandler = new WeakReference<ContentSizedWebViewHandler>(handler);
    }

    public static string InitializerScript = """
                                             (function() {
                                                 var lastHeight = -1;
                                                 var lastWidth = -1;

                                                 function getContentSize() {
                                                     var body = document.body;
                                                     var contentSize = Array
                                                         .from(body.children)
                                                         .map(e => e.getBoundingClientRect())
                                                         .reduce((a, e) => ({
                                                             width: Math.max(a.width, e.width + e.x), 
                                                             height: Math.max(a.height, e.height + e.y)}), { width: 0, height: 0 });
                                                     var bodyStyle = getComputedStyle(body);
                                                     contentSize.width += parseInt(bodyStyle.marginRight);
                                                     contentSize.height += parseInt(bodyStyle.marginBottom);
                                                     if (bodyStyle.boxSizing !== 'border-box') {
                                                         contentSize.width += parseInt(bodyStyle.borderRightWidth) + parseInt(bodyStyle.paddingRight);
                                                         contentSize.height += parseInt(bodyStyle.borderBottomWidth) + parseInt(bodyStyle.paddingBottom);
                                                     }
                                                     return contentSize;
                                                 }

                                                 function contentSizedWebViewCheckSize() {
                                                     var contentSize = getContentSize();
                                                     var newHeight = contentSize.height;
                                                     var newWidth = contentSize.width;
                                                     if (newHeight !== lastHeight || newWidth !== lastWidth) {
                                                         lastHeight = newHeight;
                                                         lastWidth = newWidth;
                                                         window.ContentSizedWebViewHandler.contentSizeChanged(newWidth + '|' + newHeight);
                                                     }
                                                 }

                                                 var observer = new ResizeObserver((entries) => contentSizedWebViewCheckSize());
                                                 observer.observe(document.body, { box: "content-box" });
                                                 contentSizedWebViewCheckSize();
                                             })();
                                             """;

    [JavascriptInterface]
    [Export("contentSizeChanged")]
    public void InvokeAction(string? data)
    {
        if (data == null || !_weakHandler.TryGetTarget(out var handler)) return;
        var sizeParts = data.Split('|');
        if (sizeParts.Length != 2 ||
            !double.TryParse(sizeParts[0], out var width) ||
            !double.TryParse(sizeParts[1], out var height)) return;

        if (width == _contentWidth && height == _contentHeight)
        {
            return;
        }
        
        _contentWidth = width;
        _contentHeight = height;
        
        handler.VirtualView
            .Dispatcher
            .Dispatch(() =>
            {
                handler.OnContentSizeChanged(new Size(width, height));
            });
    }
}
