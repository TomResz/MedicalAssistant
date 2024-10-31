const languages = {
    "pl": {
        "UnhandledError": "Wystąpił nieznany błąd.",
        "Reload": "Przeładuj stronę"
    },
    "en": {
        "UnhandledError": "An unhandled error has occurred.",
        "Reload": "Reload"
    }
}
function changeLanguage(language) {
    const texts = languages[language] || languages['pl'];
    document.getElementById('error-message').textContent = texts.UnhandledError;
    document.getElementById('reload-link').textContent = texts.Reload;
}

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
function loadErrorLabels() {
    let language = localStorage.getItem("Culture");
    language = language?.startsWith('pl') ? 'pl' : 'en';
    changeLanguage(language);
}
window.onload = function () {
    const loadingTheme = document.querySelector('.loading-logo-screen');
    if (loadingTheme) {
        const isDarkMode = localStorage.getItem("isDarkMode");
        loadingTheme.style.backgroundColor = isDarkMode == "true" ? 'rgba(50,51,61,1)' : 'white';
    }
    loadErrorLabels();
}