using System.Collections.Generic;
using System.Text;

namespace BookmarkerRe
{
    public class BookmarkFormat
    {
        private List<CatalogEntity> catalogList;
        private BookmarkContract bookmarkWrapper;

        public BookmarkFormat(List<CatalogEntity> catalogList, BookmarkContract bookmarkWrapper)
        {
            this.catalogList = catalogList;
            this.bookmarkWrapper = bookmarkWrapper;
        }

        public string Format()
        {
            if (catalogList == null || catalogList.Count == 0)
                return null;

            string catalogForamtted = FindIndependentTree(catalogList, -1, 0);

            return bookmarkWrapper.Wrapper(catalogForamtted);
        }

        private string FindIndependentTree(List<CatalogEntity> catalogList, int parent, int offset)
        {
            string Ret = "";

            StringBuilder sb = new StringBuilder();

            for (int i = offset, count = catalogList.Count; i < count; i++)
            {
                if(catalogList[i].Parent == parent) 
                {
                    // Resolve the sub-tree of this ordered catalog beginning with sepcial offset in the list
                    string child = FindIndependentTree(catalogList, i, i + 1);
                    // Give the bookmark of this catalog
                    string bookmarks = GetBookmarks(catalogList[i].BookmarkList);

                    sb.Clear();

                    if (child != null && child.Length > 0)
                    {
                        sb.AppendLine(child);
                    }

                    if(bookmarks != null)
                    {
                        sb.AppendLine(bookmarks);
                    }

                    Ret += bookmarkWrapper.CatalogWrapper(catalogList[i].Catalog.Name, sb.ToString());
                }
            }

            return Ret;
        }

        private string GetBookmarks(List<BookmarkInfo> bookmarkList)
        {
            if (bookmarkList == null || bookmarkList.Count == 0)
                return null;

            StringBuilder sb = new StringBuilder();
            foreach (BookmarkInfo info in bookmarkList)
            {
                sb.AppendLine(bookmarkWrapper.BookmarkWrapper(info));
            }

            return sb.ToString();
        }
    }
}
