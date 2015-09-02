using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkerRe
{
    public class BookmarkCatalog
    {
        private List<UrlRule> urlRuleList;
        private List<SimpleRule> simpleRuleList;
        private List<Rule> commonRuleList;

        public String Name;

        public BookmarkCatalog(String name)
        {
            this.Name = name;

            urlRuleList     = new List<UrlRule>();
            simpleRuleList  = new List<SimpleRule>();
            commonRuleList  = new List<Rule>();
        }

        public void addRule(Rule rule)
        {
            if(rule is SimpleRule)
            {
                simpleRuleList.Add((SimpleRule) rule);
            }else if(rule is UrlRule)
            {
                urlRuleList.Add((UrlRule) rule);
            }else
            {
                commonRuleList.Add(rule);
            }
        }

        public void addRules(List<Rule> ruleList)
        {
            foreach(Rule rule in ruleList)
            {
                addRule(rule);
            }
        }

        public List<Rule> GetRuleList()
        {
            List<Rule> ruleList = null;

            int count = urlRuleList.Count + simpleRuleList.Count +
                        commonRuleList.Count;
            if(count > 0)
            {
                ruleList = new List<Rule>(count);
                ruleList.AddRange(urlRuleList);
                ruleList.AddRange(simpleRuleList);
                ruleList.AddRange(commonRuleList);
            }

            return ruleList;
        }
    }
}
