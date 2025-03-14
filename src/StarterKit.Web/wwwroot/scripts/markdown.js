window.markdownToHtml = (markdown, elementId) => {
    console.info(markdown)
    console.info(elementId)
    const html = marked(markdown, {
        highlight: function (code, lang) {
            return hljs.highlightAuto(code, [lang]).value;
        }
    });
    document.getElementById(elementId).innerHTML = html;
    hljs.highlightAll();
};