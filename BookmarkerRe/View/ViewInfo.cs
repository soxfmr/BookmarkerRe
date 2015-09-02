using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkerRe.View
{
    public class ViewInfo
    {
        private string _tag;
        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        private string _path;
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public ViewInfo() { }

        public ViewInfo(string path, string tag)
        {
            _tag = tag;
            _path = path;
        }
    }
}
