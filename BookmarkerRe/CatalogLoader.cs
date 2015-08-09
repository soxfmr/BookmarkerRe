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

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                ParseCatalogs(doc);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 遍历所有分类节点
        /// </summary>
        /// <param name="doc">XML 文档对象</param>
        private void ParseCatalogs(XmlDocument doc)
        {
            // 载入根节点
            XmlNode rootNode = doc.SelectSingleNode("catalogs");
            
            if (rootNode.HasChildNodes)
            {
                // 新的分类信息列表
                catalogList = new List<CatalogEntity>();
                // 子节点信息
                XmlNode catalogNode = null;
                // 遍历节点
                for(int i = 0, count = rootNode.ChildNodes.Count; i < count; i++)
                {
                    catalogNode = rootNode.ChildNodes[i];
                    // 是否正常子节点
                    if(catalogNode.Name == "catalog" && catalogNode.HasChildNodes)
                    {
                        // 读取分类节点下的所有信息
                        ParseSingleCatalog(catalogNode);
                    }
                }
            }
        }

        /// <summary>
        /// 递归解析分类下所有信息并添加到全局分类树
        /// </summary>
        /// <param name="catalogNode">分类节点</param>
        /// <param name="parent">父节点索引</param>
        private void ParseSingleCatalog(XmlNode catalogNode, int parent = -1)
        {
            XmlNode node = null;
            CatalogEntity entity = null;

            // 新的分类
            BookmarkCatalog bookmarkCatalog = new BookmarkCatalog(((XmlElement)catalogNode).GetAttribute("name"));
            // 分类树分支
            entity = new CatalogEntity();
            entity.Parent = parent;
            entity.Catalog = bookmarkCatalog;
            // 添加到全局分支
            catalogList.Add(entity);

            // 遍历子节点，包括子目录和规则
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
        /// 遍历规则列表
        /// </summary>
        /// <param name="ruleNode">Rule 根节点</param>
        /// <returns></returns>
        private List<Rule> ParseRules(XmlNode ruleNode)
        {
            List<Rule> ruleList = new List<Rule>();

            XmlNode node = null;
            // 遍历规则节点
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
            // 根据规则父节点指定对应类型规则
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
