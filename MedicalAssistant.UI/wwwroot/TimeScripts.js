window.getTimezoneOffsetInMinutes = () => {
    return new Date().getTimezoneOffset();
};

window.getCurrentBrowserDate = function () {
    return new Date().toISOString();
}
