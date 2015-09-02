using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace BookmarkerRe
{
    /*
    * XML Format Of The Rule File
    * 
    * <?xml version="1.0" encoding="UTF-8">
    * <catalogs>
    *       <catalog name="Programming">
    *           <catalog name="Android">
    *               <rules>
    *                   <url>
    *                       <item>Android</item>
    *                   </url>
    *                   <simple>
    *                       <item>Google Developer</item>
    *                   </simple>
    *               </rules>
    *           </catalog>
    *           <catalog name="WindowsPhone">
    *               <rules>
    *                   <item>WindowsPhone</item>
    *               </rules>
    *           </catalog>
    *           <rules>
    *               <url>
    *                   <item>Development</item>
    *               </url>
    *               <item>For Loop</item>
    *           </rules>
    *     </catalog>
    * <catalogs>
    */

    public class CatalogLoader
    {
        private List<CatalogEntity> catalogList;

        public List<CatalogEntity> GetCatalogList()
        {
            return catalogList;
        }

        public bool isEmpty()
        {
            if (catalogList == null) return false;

            return catalogList.Count == 0;
        }

        public CatalogLoader() {}

        public void LoadFromXML(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("The rule file dosen't exists.");
            // Catching the exception in outside
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            ParseCatalogs(doc);
        }

        /// <summary>
        /// Resolve all of node
        /// </summary>
        /// <param name="doc">XML Doucment Object</param>
        private void ParseCatalogs(XmlDocument doc)
        {
            // Load the root node
            XmlNode rootNode = doc.SelectSingleNode("catalogs");
            
            if (rootNode.HasChildNodes)
            {
                // Creating a new entity for this node
                catalogList = new List<CatalogEntity>();
                // An instance to store the child node temporary
                XmlNode catalogNode = null;
                // Resolve all of child node
                for(int i = 0, count = rootNode.ChildNodes.Count; i < count; i++)
                {
                    catalogNode = rootNode.ChildNodes[i];
                    // Is that a normal node
                    if(catalogNode.Name == "catalog" && catalogNode.HasChildNodes)
                    {
                        // Resolve all of information of child node
                        ParseSingleCatalog(catalogNode);
                    }
                }
            }
        }

        /// <summary>
        /// Resolve all of information of child node and append to the global tree of catalogs
        /// </summary>
        /// <param name="catalogNode">parent node</param>
        /// <param name="parent">the index of parent node in global catalogs tree</param>
        private void ParseSingleCatalog(XmlNode catalogNode, int parent = -1)
        {
            XmlNode node = null;
            CatalogEntity entity = null;

            // Creating a new catalog
            BookmarkCatalog bookmarkCatalog = new BookmarkCatalog(((XmlElement)catalogNode).GetAttribute("name"));
            // A new catalog entity
            entity = new CatalogEntity();
            entity.Parent = parent;
            entity.Catalog = bookmarkCatalog;
            // Append to the global tree
            catalogList.Add(entity);

            // Resolve all of child node and rules
            for (int i = 0, count = catalogNode.ChildNodes.Count; i < count; i++)
            {
                node = catalogNode.ChildNodes[i];

                if (!node.HasChildNodes)
                {
                    continue;
                }

                switch(node.Name)
                {
                    case "catalog":
                        ParseSingleCatalog(node, catalogList.IndexOf(entity));
                        break;
                    case "rule":
                        bookmarkCatalog.addRules( ParseRules(node) );
                        break;
                    default: break;
                }
            }
        }

        /// <summary>
        /// Resolve all of rule
        /// </summary>
        /// <param name="ruleNode">Parent Rule</param>
        /// <returns></returns>
        private List<Rule> ParseRules(XmlNode ruleNode)
        {
            List<Rule> ruleList = new List<Rule>();

            XmlNode node = null;

            for (int i = 0, count = ruleNode.ChildNodes.Count; i < count; i++)
            {
                node = ruleNode.ChildNodes[i];
                if (node.Name != "item" && !node.HasChildNodes)
                {
                    continue;
                }

                switch (node.Name)
                {
                    case "url":
                        ruleList.AddRange(ParseRules(node));
                        break;
                    case "simple":
                        ruleList.AddRange(ParseRules(node));
                        break;
                    case "item":
                        ruleList.Add(ParseSingleRule(node));
                        break;
                    default: break;
                }
            }

            return ruleList;
        }
        
        private Rule ParseSingleRule(XmlNode item)
        {
            Rule rule = null;
            // Creating a new object for the rule with different type
            switch (item.ParentNode.Name)
            {
                case "url":
                    rule = new UrlRule(item.InnerText);
                    break;
                case "simple":
                    rule = new SimpleRule(item.InnerText);
                    break;
                case "rule":
                    rule = new Rule(item.InnerText);
                    break;
                default: break;
            }

            return rule;
        }
    }
}
