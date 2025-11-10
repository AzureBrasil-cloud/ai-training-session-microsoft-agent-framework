declare module 'markdown-it' {
  export default class MarkdownIt {
    constructor(options?: any);
    render(markdown: string): string;
  }
}

