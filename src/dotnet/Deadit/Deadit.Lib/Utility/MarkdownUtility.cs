using Markdig;

namespace Deadit.Lib.Utility;

public static class MarkdownUtility
{
    private static readonly MarkdownPipeline _markdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

    public static string ToHtml(string markdown)
    {
        return Markdown.ToHtml(markdown, _markdownPipeline);
    }
}
