namespace SizeWebViewToContent;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        MyWebView.Source = new HtmlWebViewSource
        {
            Html = """
                   <html>
                       <head>
                            <meta name='viewport' content='width=device-width; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/>
                            <script type="text/javascript">
                                function addText() {
                                    var p = document.createElement('p');
                                    p.innerHTML = 'This is a paragraph of text.';
                                    document.body.appendChild(p);
                                }
                                function removeText() {
                                    var p = document.querySelector('p');
                                    if (p) {
                                        document.body.removeChild(p);
                                    }
                                }
                            </script>
                       </head>
                       <body style="background:lightblue">
                           <h3>WebView content</h3>
                           <button onclick="addText()">Add paragraph</button>
                           <button onclick="removeText()">Remove paragraph</button>
                           </div>
                       </body>
                   </html>
                   """
        };
    }
}