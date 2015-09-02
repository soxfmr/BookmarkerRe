using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

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

                mListener.OnProcess("Loading bookmark file...");
                List<BookmarkInfo> bookmarkList = bookmarkLoader.GetBookmarkList();

                mListener.OnProcess("Loading rule file...");
                List<CatalogEntity> catalogList = catalogLoader.GetCatalogList();

                if (bookmarkList == null || bookmarkLoader.isEmpty())
                    throw new Exception("The content of the bookmark file is empty.");

                if (catalogList == null || catalogLoader.isEmpty())
                    throw new Exception("The content of the rule file is empty.");

                mListener.OnProcess("Processing...");
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

        public void Export(string fileName)
        {
            BookmarkFormat bookmarkFormat = new BookmarkFormat( mResult, new ChromeBookmarkWrapper() );
            string content = bookmarkFormat.Format();

            Thread thread = new Thread(new ThreadStart(delegate
            {
                StreamWriter writer = null;
                try
                {
                    writer = new StreamWriter(fileName);
                    writer.Write(content);
                }
                catch (IOException ex)
                {
                    Console.Write(ex.Message);
                }
                finally
                {
                    writer.Close();
                }
            }));

            thread.IsBackground = true;
            thread.Start();
        }
    }
}
