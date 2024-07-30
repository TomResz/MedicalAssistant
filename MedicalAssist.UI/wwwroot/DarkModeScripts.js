function setRadzenComponentsTheme(isDarkMode) {
    let normal = document.getElementById('normalTheme');
    let dark = document.getElementById('darkModeTheme');

    if (isDarkMode === true) {
        normal.disabled = true;
        dark.disabled = false;
    } else {
        normal.disabled = false;
        dark.disabled = true;
    }
}
window.onload = function () {
    const loadingTheme = document.querySelector('.loading-logo-screen');
    if (loadingTheme) {
        const isDarkMode = localStorage.getItem("isDarkMode");
        loadingTheme.style.backgroundColor = isDarkMode == "true" ? 'rgba(50,51,61,1)' : 'white';
    }
}
