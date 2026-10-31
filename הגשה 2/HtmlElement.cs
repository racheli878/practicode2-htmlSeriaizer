using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace הגשה_2
{
    internal class HtmlElement
    {
        public string? Id { get; set; }

        public string? Name { get; set; }
        public List<string> Attributes { get; set; } = new List<string>();

        public List<string> Classes { get; set; } = new List<string>();

        public string? InnerHtml { get; set; }

        public HtmlElement? Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();
       public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> q=new Queue<HtmlElement>();
            q.Enqueue(this);
            while(q.Count>0)
            {
                var e=q.Dequeue();
                yield return e;
                foreach(var c in e.Children)
                    q.Enqueue(c);
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            var current = Parent;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }
    }
}
