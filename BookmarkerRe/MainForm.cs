using System;
using System.Threading;
using System.Windows.Forms;

namespace BookmarkerRe
{
    public partial class MainForm : Form
    {
        public const int VIEW_HOME = 1;
        public const int VIEW_PROCESS = 2;

        public const int CALLBACK_SUCCESS = 200;
        public const int CALLBACK_FAILED = 500;

        private static bool DOCUMENT_LOAD_COMPLETE = false;

        private string APP_ROOT = Environment.CurrentDirectory + "/main";

        private HtmlDocument        mDocument;
        private HtmlElement         edBookmarkFile;
        private HtmlElement         edRuleFile;

        private HtmlElement         txtPinText;

        private BookmarkExecuter    bookmarkExecuter;
        private ExecuteListener     executeListener;

        public MainForm()
        {
            InitializeComponent();

            init();
        }

        public void init()
        {
            LoadView(VIEW_HOME);
            // 初始化监听事件
            executeListener = new ExecuteListener();
            executeListener.OnStart += OnStart;
            executeListener.OnFinish += OnFinish;
            executeListener.OnProcess += OnProcess;
            executeListener.OnError += OnError;
            // 执行器
            bookmarkExecuter = new BookmarkExecuter(executeListener);
        }

        /******************************* Main User Interface Operation ************************************/

        /// <summary>
        /// 载入视图
        /// </summary>
        /// <param name="index">视图索引</param>
        /// <param name="wait">同步视图加载</param>
        private void LoadView(int index, bool wait = false)
        {
            switch(index)
            {
                case VIEW_HOME:
                    LoadView(APP_ROOT + "/index.html", wait);
                    break;
                case VIEW_PROCESS:
                    LoadView(APP_ROOT + "/process.html", wait);
                    break;
                default: break;
            }
        }

        /// <summary>
        /// 载入视图
        /// </summary>
        /// <param name="fn">视图路径</param>
        /// <param name="wait">同步视图加载</param>
        private void LoadView(String fn, bool wait)
        {
            SetLoadStatus(false);

            MainUserInterface.Navigate(fn);

            // 等待页面加载完成
            if(wait)
            {
                while(! GetLoadStatus())
                {
                    Thread.Sleep(200);
                }
            }
        }

        /// <summary>
        /// 在过程中显示信息
        /// </summary>
        /// <param name="message"></param>
        private void ShowFeedback(String message)
        {
            // 处理过程信息
            if (txtPinText != null)
            {
                txtPinText.InnerText = message;
            }
        }

        /// <summary>
        /// 设置视图加载状态
        /// </summary>
        /// <param name="finished"></param>
        private void SetLoadStatus(bool finished)
        {
            // 置位加载状态
            DOCUMENT_LOAD_COMPLETE = finished;
        }

        /// <summary>
        /// 获取视图加载状态
        /// </summary>
        /// <returns></returns>
        private bool GetLoadStatus()
        {
            return DOCUMENT_LOAD_COMPLETE;
        }

        /// <summary>
        /// 停止过程视图
        /// function callback(int status) { success : 200, error : 500 }
        /// </summary>
        private void ProcessCallback(int status)
        {
            if(mDocument != null)
            {
                object[] funcParams = new object[] { status };

                this.Invoke(new Action(delegate
                {
                    mDocument.InvokeScript("callback", funcParams);
                }));
            }
        }

        /******************************* Main User Interface Operation ************************************/

        private void MainUserInterface_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            mDocument = MainUserInterface.Document;

            int index = 0;

            String attr = null;

            HtmlElementCollection elementList = mDocument.GetElementsByTagName("meta");
            foreach(HtmlElement element in elementList)
            {
                if((attr = element.GetAttribute("name")) != null && attr == "view")
                {
                    switch(element.GetAttribute("content"))
                    {
                        case "home":
                            index = VIEW_HOME;
                            break;
                        case "process":
                            index = VIEW_PROCESS;
                            break;
                        default: break;
                    }

                    break;
                }
            }

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

                    break;
            }

            SetLoadStatus(true);
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
                MainUserInterface.Refresh();
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

        public void OnStart()
        {
            // 非异步加载视图
            LoadView(VIEW_PROCESS, true);

            ShowFeedback("载入环境...");
        }

        public void OnFinish()
        {
            ShowFeedback("整理完成！");

            ProcessCallback(CALLBACK_SUCCESS);
        }

        public void OnProcess(String message)
        {
            ShowFeedback(message);
        }

        public void OnError(String message)
        {
            ShowFeedback(message);

            ProcessCallback(CALLBACK_FAILED);
        }
    }
}
