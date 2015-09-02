using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkerRe
{
    public class ChromeBookmarkWrapper : BookmarkContract
    {
        const string TEMPLATE_DOCUMENT = "<!DOCTYPE NETSCAPE-Bookmark-file-1>\n" +
                                         "<head>" +
                                         "<META HTTP-EQUIV= \"Content-Type\" CONTENT=\"text/html; charset=UTF-8\">" +
                                         "<TITLE>Bookmarks</TITLE>" +
                                         "</head>" +
                                         "<body>" +
                                         "<H1>Bookmarks</H1>" +
                                         "<DL>" +
                                         "<DT><H3 PERSONAL_TOOLBAR_FOLDER=\"true\">Bookmarks</H3>" +
                                         "<DL>{0}</DL>" +
                                         "</DT></DL>" +
                                         "</body>";

        const string TEMPLATE_CATALOG = "<DT><h3>{0}</h3><DL>{1}</DL></DT>";

        const string TEMPLATE_BOOKMARK = "<DT><a ADD_DATE=\"{0}\" ICON=\"{1}\" href=\"{2}\">{3}</a></DT>";

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
            return String.Format(TEMPLATE_BOOKMARK, info.AddDate, info.Icon, info.Url, info.Title);
        }
    }
}
