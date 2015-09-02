using System;

namespace BookmarkerRe
{
    public class BookmarkInfo
    {
        public const string BOOKMARK_PATTNER = "<A[\\s\\S]+</A>";
        
        private const string BOOKMARK_TAG_URL = "HREF";
        private const string BOOKMARK_TAG_ICON = "ICON";
        private const string BOOKMARK_TAG_ADD_DATE = "ADD_DATE";
        // The redundancy content that will be remove
        private const string BOOKMARK_PURGES = "<DT>";
        // The separator of echo redundancy words
        private const char BOOKMARK_PURGE_SEPARATOR = '|';

        public string Url;
        public string Title;
        public string Icon;
        public string AddDate;

        public BookmarkInfo() { }

        public BookmarkInfo(string url, string title, string icon, string addDate)
        {
            this.Url = url;
            this.Title = title;
            this.Icon = icon;
            this.AddDate = addDate;
        }

        public bool Parse(string bookmark)
        {
            bool bRet = false;
            try
            {
                bookmark = Purge(bookmark);

                this.Title      = GetTitle(bookmark);
                this.Url        = Find(bookmark, BOOKMARK_TAG_URL);
                this.Icon       = Find(bookmark, BOOKMARK_TAG_ICON);
                this.AddDate    = Find(bookmark, BOOKMARK_TAG_ADD_DATE);

                bRet = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return bRet;
        }

        private string Purge(string origin)
        {
            string Ret = origin;

            string[] purges = BOOKMARK_PURGES.Split( BOOKMARK_PURGE_SEPARATOR );
            foreach(string purge in purges)
            {
                Ret = Ret.Replace(purge, "");
            }

            return Ret;
        }

        public string GetTitle(string subject)
        {
            string tagStart = ">";
            string tagEnd = "</A>";

            return _Find(subject, tagStart, tagEnd);
        }

        private string Find(string subject, string tag)
        {
            string tagStart = tag + "=\"";
            string tagEnd = "\"";

            return _Find(subject, tagStart, tagEnd);
        }

        private string _Find(string subject, string tagStart, string tagEnd)
        {
            string Ret = "";

            int index = 0, offset = 0;

            if ((index = subject.IndexOf(tagStart)) != -1)
            {
                offset = index + tagStart.Length;
                Ret = subject.Substring(offset, subject.IndexOf(tagEnd, offset) - offset);
            }

            return Ret;
        }
    }
}
