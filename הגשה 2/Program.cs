
using System.Text.RegularExpressions;
using הגשה_2;

static async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

var html = await Load("https://meetus-client.onrender.com");

var cleanHtml = new Regex("\\s+").Replace(html, " ");

var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

static HtmlElement buildTree(List<string> htmlLines)
{

    HtmlElement? root = null;
    var current = root;
    var attrRegex = new Regex("([^\\s=]+)(?:\\s*=\\s*(?:\"([^\"]*)\"|'([^']*)'|([^\\s\"'>]+)))?");
    foreach (var line in htmlLines)
    {
        var firstWord = line.Split(' ')[0];
        var tagName = firstWord.TrimEnd('/');

        if (firstWord == "/html")
        {
            return root;
        }
        else if (line[0]=='!')
        {
            continue;
        }
        else if (HtmlHelper.Instance.tags.Contains(tagName))
        {
            HtmlElement newNode = new HtmlElement();
            newNode.Name = tagName;
            newNode.Parent = current;
            var rest = line.Substring(firstWord.Length).Trim().TrimEnd('/');
            foreach (Match m in attrRegex.Matches(rest))
            {
                newNode.Attributes.Add(m.Value);
                var attrName = m.Groups[1].Value;
                var attrValue = m.Groups[2].Success ? m.Groups[2].Value : m.Groups[3].Success ? m.Groups[3].Value : m.Groups[4].Value;
                if (attrName == "id")
                    newNode.Id = attrValue;
                else if (attrName == "class")
                    newNode.Classes.AddRange(attrValue.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            }
                if(root==null)
                    root = newNode;
                else
                    current!.Children.Add(newNode);
                var selfClosing=line.EndsWith('/')||HtmlHelper.Instance.voidTags.Contains(tagName);
            if (!selfClosing)
                current = newNode;
        }
        else if (line[0] == '/')
        {
            current = current?.Parent;
        }
        else
        {
            current!.InnerHtml = line;
        }
    }
    return root!;
}


static void Print(HtmlElement element, int depth = 0)
{
    var indent = new string(' ', depth * 2);
    var id = !string.IsNullOrEmpty(element.Id) ? $" #{element.Id}" : "";
    var classes = element.Classes.Count > 0 ? " ." + string.Join(".", element.Classes) : "";
    var text = !string.IsNullOrEmpty(element.InnerHtml) ? $"  \"{element.InnerHtml}\"" : "";

    Console.WriteLine($"{indent}{element.Name}{id}{classes}{text}");

    foreach (var child in element.Children)
        Print(child, depth + 1);
}

var root = buildTree(htmlLines);
Print(root);

var sel = Selector.Create("body div");
foreach (var r in root.findElement(sel))
    Console.WriteLine(r.Name);