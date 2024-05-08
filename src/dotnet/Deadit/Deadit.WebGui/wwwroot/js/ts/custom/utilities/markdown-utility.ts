
// see: https://github.com/markedjs/marked

import { marked } from 'marked';

export class MarkdownUtility
{
    public static toHtml = (text: string) =>
    {
        return marked.parse(text);
    }
}