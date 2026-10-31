using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace הגשה_2
{
    internal class Selector
    {
        public string? TagName { get; set; }
        public string? Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector? Parent { get; set; }
        public Selector? Child { get; set; }

        public static Selector Create(string query)
        {
            var selectors = query.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            Selector selector = new Selector();
            Selector current = selector;
            for(int i=0;i<selectors.Count;i++)
            {
                var parts = new Regex("(?=[#.])").Split(selectors[i]).Where(p => p.Length > 0).ToList();
                foreach (var p in parts)
                {
                    if (p.StartsWith('#'))
                        current.Id = p.Substring(1);
                    else if (p.StartsWith('.'))
                        current.Classes.Add(p.Substring(1));
                    else if (HtmlHelper.Instance.tags.Contains(p))
                        current.TagName = p;
                }
                if (i < selectors.Count - 1)
                {
                    var next=new Selector();
                    next.Parent=current;
                    current.Child = next;
                    current=next;
                }
            }
            return selector;
        }
    }
}
