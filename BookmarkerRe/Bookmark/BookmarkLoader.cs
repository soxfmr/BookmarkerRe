using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace BookmarkerRe
{
    public class BookmarkLoader
    {

        private List<BookmarkInfo> bookmarkList;

        public BookmarkLoader() { }

        public void Load(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("The bookmark file dosen't exists.");

            bookmarkList = new List<BookmarkInfo>();

            BookmarkInfo bookmarkInfo = null;
            String line = null;
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(path, Encoding.UTF8);
                while((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (!Regex.IsMatch(line, BookmarkInfo.BOOKMARK_PATTNER))
                    {
                        continue;
                    }

                    bookmarkInfo = new BookmarkInfo();
                    if(bookmarkInfo.Parse(line))
                    {
                        bookmarkList.Add(bookmarkInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if(reader != null) reader.Close();
            }
        }

        public bool isEmpty()
        {
            if (bookmarkList == null) return false;

            return bookmarkList.Count == 0;
        }

        public List<BookmarkInfo> GetBookmarkList()
        {
            return bookmarkList;
        }
    }
}
