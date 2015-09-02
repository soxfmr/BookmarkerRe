using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BookmarkerRe
{
    public class RuleMatcher
    {
        private List<int> removeIndexList;

        public RuleMatcher()
        {
            removeIndexList = new List<int>();
        }

        public List<BookmarkInfo> Match(List<BookmarkInfo> bookmarkList, List<Rule> ruleList)
        {
            List<BookmarkInfo> matchList = null;
            List<BookmarkInfo> attachTempList = null;

            if (bookmarkList != null && ruleList != null 
                && bookmarkList.Count > 0 && ruleList.Count > 0)
            {
                matchList = new List<BookmarkInfo>();

                foreach (Rule rule in ruleList)
                {
                    if (rule is SimpleRule)
                    {
                        attachTempList = MatchSimple(bookmarkList, (SimpleRule) rule);
                    }else if(rule is UrlRule)
                    {
                        attachTempList = MatchUrl(bookmarkList, (UrlRule) rule);
                    }else
                    {
                        attachTempList = MatchBoth(bookmarkList, rule);
                    }

                    Purge(bookmarkList, removeIndexList);

                    matchList.AddRange(attachTempList);
                }
            }

            return matchList;
        }

        public List<BookmarkInfo> Purge(List<BookmarkInfo> bookmarkList, List<int> removeIndexList)
        {
            foreach(int index in removeIndexList)
            {
                bookmarkList.RemoveAt(index);
            }

            resetIndexList();

            return bookmarkList;
        }

        private List<BookmarkInfo> MatchUrl(List<BookmarkInfo> bookmarkList, UrlRule rule)
        {
            List<BookmarkInfo> matchList = new List<BookmarkInfo>();

            for (int i = bookmarkList.Count - 1; i >= 0; i--)
            {
                if (Regex.IsMatch(bookmarkList[i].Url, Regex.Escape(rule.Pattern), RegexOptions.IgnoreCase))
                {
                    matchList.Add(bookmarkList[i]);

                    addRemoveIndex(i);
                }
            }

            return matchList;
        }

        private List<BookmarkInfo> MatchSimple(List<BookmarkInfo> bookmarkList, SimpleRule rule)
        {
            List<BookmarkInfo> matchList = new List<BookmarkInfo>();

            for (int i = bookmarkList.Count - 1; i >= 0; i--)
            {
                if (Regex.IsMatch(bookmarkList[i].Title, Regex.Escape(rule.Pattern), RegexOptions.IgnoreCase))
                {
                    matchList.Add(bookmarkList[i]);

                    addRemoveIndex(i);
                }
            }

            return matchList;
        }

        private List<BookmarkInfo> MatchBoth(List<BookmarkInfo> bookmarkList, Rule rule)
        {
            List<BookmarkInfo> matchList = new List<BookmarkInfo>();

            for (int i = bookmarkList.Count - 1; i >= 0; i--)
            {
                if (Regex.IsMatch(bookmarkList[i].Url, Regex.Escape(rule.Pattern), RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(bookmarkList[i].Title, Regex.Escape(rule.Pattern), RegexOptions.IgnoreCase))
                {
                    matchList.Add(bookmarkList[i]);

                    addRemoveIndex(i);
                }
            }

            return matchList;
        }

        private void addRemoveIndex(int index)
        {
            removeIndexList.Add(index);
        }

        private void resetIndexList()
        {
            removeIndexList.Clear();
        }
    }
}
