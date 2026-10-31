using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace הגשה_2
{
    internal static class HtmlElementExtension
    {
        public static HashSet<HtmlElement> findElement(this HtmlElement element,Selector selector) 
        {
            HashSet<HtmlElement>list= new HashSet<HtmlElement>();
            help(element,selector,list);
            return list;
        }
        public static void help(HtmlElement element,Selector selector,HashSet<HtmlElement> res)
        {
            var filtered=element.Descendants().Skip(1).Where(d=>(selector.TagName==null||d.Name==selector.TagName)&&(selector.Id==null||d.Id==selector.Id)
            &&selector.Classes.All(c=>d.Classes.Contains(c)));
            if (selector.Child == null)
                res.UnionWith(filtered);
            else foreach(var f in filtered)
                help(f, selector.Child,res);
        }
    }
}
