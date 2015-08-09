using System;
using System.Collections.Generic;
using System.Threading;

namespace BookmarkerRe
{
    public class BookmarkExecuter
    {
        private ExecuteListener mListener;

        private CatalogLoader   catalogLoader;
        private BookmarkLoader  bookmarkLoader;

        private RuleMatcher ruleMatcher;

        private List<CatalogEntity> mResult;

        public BookmarkExecuter(ExecuteListener listener)
        {
            mListener = listener;

            catalogLoader = new CatalogLoader();
            bookmarkLoader = new BookmarkLoader();

            ruleMatcher = new RuleMatcher();
        }

        public void Execute(String bookmarkFile, String RuleFile, bool Async = true)
        {
            if(Async)
            {
                Thread thread = new Thread(new ThreadStart(delegate
                {
                    _Execute(bookmarkFile, RuleFile);
                }));

                thread.IsBackground = true;
                thread.Start();

                return;
            }

            _Execute(bookmarkFile, RuleFile);
        }

        private void _Execute(String bookmarkFile, String RuleFile)
        {
            mListener.OnStart();

            try
            {
                bookmarkLoader.Load(bookmarkFile);
                catalogLoader.LoadFromXML(RuleFile);

                mListener.OnProcess("正在载入书签文件...");
                List<BookmarkInfo> bookmarkList = bookmarkLoader.GetBookmarkList();

                mListener.OnProcess("正在载入规则文件...");
                List<CatalogEntity> catalogList = catalogLoader.GetCatalogList();

                if (bookmarkList == null || bookmarkLoader.isEmpty())
                    throw new Exception("The content of the bookmark file is empty.");

                if (catalogList == null || catalogLoader.isEmpty())
                    throw new Exception("The content of the rule file is empty.");

                mListener.OnProcess("正在整理书签...");
                for (int i = 0, count = catalogList.Count; i < count; i++)
                {
                    catalogList[i].BookmarkList = ruleMatcher.Match(bookmarkList,
                        catalogList[i].Catalog.GetRuleList());
                }

                mResult = catalogList;

                mListener.OnFinish();
            }
            catch (Exception ex)
            {
                mListener.OnError(ex.Message);
            }
        }

        public List<CatalogEntity> GetResult()
        {
            return mResult;
        }
    }
}
