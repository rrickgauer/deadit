using Markdig;
using Microsoft.AspNetCore.Html;

namespace Deadit.Lib.Utility;

public static class MarkdownUtility
{
    private static readonly MarkdownPipeline _markdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

    public static string ToHtmlString(string markdown)
    {
        return Markdown.ToHtml(markdown, _markdownPipeline);
    }

    public static HtmlString ToHtmlContent(string markdown)
    {
        return new(ToHtmlString(markdown));
    }
}
