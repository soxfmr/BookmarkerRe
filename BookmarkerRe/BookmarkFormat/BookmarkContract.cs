using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkerRe
{
    public interface BookmarkContract
    {
        string Wrapper(string catalogs);
        string CatalogWrapper(string name, string bookmarks);
        string BookmarkWrapper(BookmarkInfo info);
    }
}
