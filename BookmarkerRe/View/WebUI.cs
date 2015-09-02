using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace BookmarkerRe.View
{
    public partial class WebUI : UserControl
    {
        private static bool DOCUMENT_LOAD_COMPLETE = false;

        private Dictionary<int, ViewInfo> viewList;

        public WebUI()
        {
            InitializeComponent();

            init();
        }

        private void init()
        {
            viewList = new Dictionary<int, ViewInfo>();
        }

        public void addView(int index, string html, string tag = null)
        {
            viewList.Add(index, new ViewInfo(html, tag));
        }

        public void LoadView(int index)
        {
            LoadView(index, false);
        }

        public void LoadView(int index, bool sync)
        {
            ViewInfo vi = GetViewInfo(index);
            if (vi == null) return;

            SetLoadStatus(false);

            myBrowser.Navigate(vi.Path);

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

        public void InvokeNativeFunc(string funcName, object[] args)
        {
            HtmlDocument doc = GetDocument();
            if (doc != null)
            {
                doc.InvokeScript(funcName, args);
            }
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

        public int GetViewIndex()
        {
            int index = 0;

            HtmlDocument doc = GetDocument();

            string attr = null;

            HtmlElementCollection elementList = doc.GetElementsByTagName("meta");
            foreach (HtmlElement element in elementList)
            {
                if ((attr = element.GetAttribute("name")) != null && attr == "view")
                {
                    foreach(KeyValuePair<int, ViewInfo> pair in viewList)
                    {
                        if (pair.Value.Tag == null || pair.Value.Tag.Length == 0) continue;

                        if( pair.Value.Tag == element.GetAttribute("content") )
                        {
                            index = pair.Key;
                            break;
                        }
                    }

                    break;
                }
            }

            return index;
        }

        private ViewInfo GetViewInfo(int index)
        {
            ViewInfo vi = null;
            viewList.TryGetValue(index, out vi);

            return vi;              
        }
    }
}
