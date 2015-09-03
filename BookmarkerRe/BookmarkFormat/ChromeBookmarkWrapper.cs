using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkerRe
{
    public class ChromeBookmarkWrapper : BookmarkContract
    {
        const string TEMPLATE_DOCUMENT = "<!DOCTYPE NETSCAPE-Bookmark-file-1>\r\n" +
                                         "<META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; charset=UTF-8\">\r\n" +
                                         "<TITLE>Bookmarks</TITLE>\r\n" +
                                         "<H1>Bookmarks</H1>\r\n" +
                                         "<DL><p>\r\n" +
                                         "<DT><H3 PERSONAL_TOOLBAR_FOLDER=\"true\">书签栏</H3>\r\n" +
                                         "<DL><p>\r\n" +
                                         "{0}\r\n" + 
                                         "</DL><p>\r\n" + // End with bookmarks bar
                                         "</DL><p>"; //  End with bookmarks

        const string TEMPLATE_CATALOG = "<DT><H3 ADD_DATE=\"1441282141\" LAST_MODIFIED=\"1441282284\">{0}</H3>\r\n<DL><p>\r\n{1}</DL><p>\r\n";

        const string TEMPLATE_BOOKMARK = "<DT><A HREF=\"{0}\" ADD_DATE=\"{1}\" ICON=\"{2}\" >{3}</A>";

        public string Wrapper(string catalogs)
        {
            return String.Format(TEMPLATE_DOCUMENT, catalogs);
        }

        public string CatalogWrapper(string name, string bookmarks)
        {
            return String.Format(TEMPLATE_CATALOG, name, bookmarks);
        }

        public string BookmarkWrapper(BookmarkInfo info)
        {
            return String.Format(TEMPLATE_BOOKMARK, info.Url, info.AddDate, info.Icon, info.Title);
        }
    }
}
