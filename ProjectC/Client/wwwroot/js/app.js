var BlazorInterop = BlazorInterop || {};
BlazorInterop.LoadJsonEditor = function (container, body) {
    const options = {
        mode: 'view'
    };
    const editor = new JSONEditor(container, options);
    const json = JSON.parse(body);
    editor.set(json);
}