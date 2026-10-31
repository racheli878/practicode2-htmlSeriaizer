
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace הגשה_2
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance { get { return _instance; } }
        public string[] tags { get; set; }
        public string[] voidTags { get; set; }
        private HtmlHelper() 
        {
            tags =JsonSerializer.Deserialize<string[]>(File.ReadAllText("HtmlTags.json"))!;
            voidTags =JsonSerializer.Deserialize<string[]>(File.ReadAllText("HtmlVoidTags.json"))!;
        }

       

    }
}
