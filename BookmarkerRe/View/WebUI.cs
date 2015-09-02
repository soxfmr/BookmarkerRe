using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace BookmarkerRe.View
{
    public partial class WebUI : UserControl
    {
        private static bool DOCUMENT_LOAD_COMPLETE = false;

        private Dictionary<int, string> viewList;

        public WebUI()
        {
            InitializeComponent();

            init();
        }

        private void init()
        {
            viewList = new Dictionary<int, string>();
        }

        public void addView(int index, string html)
        {
            viewList.Add(index, html);
        }

        public void LoadView(int index)
        {
            LoadView(index, false);
        }

        public void LoadView(int index, bool sync)
        {
            string html = "";
            if (! viewList.TryGetValue(index, out html))
                return;

            SetLoadStatus(false);

            myBrowser.Navigate(html);

            // Waiting until the page is loaded.
            if (sync)
            {
                while (! GetLoadStatus())
                {
                    Thread.Sleep(200);
                }
            }
        }

        public void Reload()
        {
            myBrowser.Refresh();
        }

        public HtmlDocument GetDocument()
        {
            return myBrowser.Document;
        }

        public void setCompletedListener(WebBrowserDocumentCompletedEventHandler handler)
        {
            myBrowser.DocumentCompleted += handler;
        }

        /// <summary>
        /// Trigger the status of the page
        /// </summary>
        /// <param name="finished"></param>
        public void SetLoadStatus(bool finished)
        {
            DOCUMENT_LOAD_COMPLETE = finished;
        }

        /// <summary>
        /// Give the status of the current page
        /// </summary>
        /// <returns></returns>
        public bool GetLoadStatus()
        {
            return DOCUMENT_LOAD_COMPLETE;
        }
    }
}
