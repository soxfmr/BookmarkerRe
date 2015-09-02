using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BookmarkerRe
{
    public partial class MainForm : Form
    {
        public const int VIEW_HOME = 1;
        public const int VIEW_PROCESS = 2;

        public const int CALLBACK_SUCCESS = 200;
        public const int CALLBACK_FAILED = 500;

        private string APP_ROOT = Environment.CurrentDirectory + "/main";

        private HtmlDocument mDocument;
        private HtmlElement edBookmarkFile;
        private HtmlElement edRuleFile;

        private HtmlElement txtPinText;

        private BookmarkExecuter bookmarkExecuter;
        private ExecuteListener executeListener;

        public MainForm()
        {
            InitializeComponent();

            init();
        }

        public void init()
        {
            webUI.addView(VIEW_HOME, APP_ROOT + "/index.html", "home");
            webUI.addView(VIEW_PROCESS, APP_ROOT + "/process.html", "process");
            webUI.setCompletedListener(WEBUI_DocumentCompleted);

            webUI.LoadView(VIEW_HOME);
            // Initial the listener
            executeListener = new ExecuteListener();
            executeListener.OnStart += OnStart;
            executeListener.OnFinish += OnFinish;
            executeListener.OnProcess += OnProcess;
            executeListener.OnError += OnError;
            // Executer to delegate the real action
            bookmarkExecuter = new BookmarkExecuter(executeListener);
        }

        private void WEBUI_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            mDocument = webUI.GetDocument();

            int index = webUI.GetViewIndex();

            switch(index)
            {
                case VIEW_HOME:

                    edRuleFile = mDocument.GetElementById("rule-file");
                    edBookmarkFile = mDocument.GetElementById("bookmark-file");

                    HtmlElement btnRuleFile = mDocument.GetElementById("btn-rule-file");
                    HtmlElement btnBookmarkFile = mDocument.GetElementById("btn-bookmark-file");
                    HtmlElement btnClassfiy = mDocument.GetElementById("btn-classfiy");

                    btnRuleFile.Click += new HtmlElementEventHandler(OnRuleFileClick);
                    btnBookmarkFile.Click += new HtmlElementEventHandler(OnBookmarkFileClick);
                    btnClassfiy.Click += new HtmlElementEventHandler(OnClassfiyClick);

                    break;
                case VIEW_PROCESS:

                    txtPinText = mDocument.GetElementById("pin-text");
                    HtmlElement btnExport = mDocument.GetElementById("btn-export");

                    btnExport.Click += new HtmlElementEventHandler(OnExportClick);

                    break;
            }

            webUI.SetLoadStatus(true);
        }

        public void OnRuleFileClick(object sender, HtmlElementEventArgs e)
        {
            String fn = ToolsUtil.GetSelectedFile("Rule File(*.xml)|*.xml|All Files(*.*)|*.*");
            if (fn != null && edBookmarkFile != null)
            {
                edRuleFile.SetAttribute("value", fn);
            }
        }

        public void OnBookmarkFileClick(object sender, HtmlElementEventArgs e)
        {
            String fn = ToolsUtil.GetSelectedFile("Bookmark File(*.html)|*.html|All Files(*.*)|*.*");
            if(fn != null && edRuleFile != null)
            {
                edBookmarkFile.SetAttribute("value", fn);
            }
        }

        public void OnClassfiyClick(object sender, HtmlElementEventArgs e)
        {
            if (edRuleFile == null || edBookmarkFile == null)
            {
                webUI.Reload();
                return;
            }

            String ruleFilePath     = edRuleFile.GetAttribute("value");
            String bookmarkFilePath = edBookmarkFile.GetAttribute("value");

            if(ruleFilePath == "" || bookmarkFilePath == "")
            {
                return;
            }

            bookmarkExecuter.Execute(bookmarkFilePath, ruleFilePath, true);
            
        }

        public void OnExportClick(object sender, HtmlElementEventArgs e)
        {
            String fn = ToolsUtil.GetSaveFileDialog("Bookmark File(*.html)|*.html|All Files(*.*)|*.*");
            if (fn != null)
            {
                bookmarkExecuter.Export(fn);
            }
        }

        private void ShowFeedback(String message)
        {
            if (txtPinText != null)
            {
                txtPinText.InnerText = message;
            }
        }

        /// <summary>
        /// Push the message on the process page when the action has done.
        /// function callback(int status) { success : 200, error : 500 }
        /// </summary>
        private void ProcessCallback(int status)
        {
            // Invoke the function in UI thread
            Invoke(new Action(delegate
            {
                object[] funcParams = new object[] { status };
                webUI.InvokeNativeFunc("callback", funcParams);
            }));
        }

        public void OnStart()
        {
            webUI.LoadView(VIEW_PROCESS, true);

            ShowFeedback("Initialize the environment...");
        }

        public void OnProcess(String message)
        {
            ShowFeedback(message);
        }

        public void OnFinish()
        {
            ShowFeedback("All Done！");

            ProcessCallback(CALLBACK_SUCCESS);
        }

        public void OnError(String message)
        {
            ShowFeedback(message);

            ProcessCallback(CALLBACK_FAILED);
        }
    }
}
