using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkerRe
{
    public class CatalogEntity
    {
        public int Parent;
        public BookmarkCatalog Catalog;
        public List<BookmarkInfo> BookmarkList;
    }
}
