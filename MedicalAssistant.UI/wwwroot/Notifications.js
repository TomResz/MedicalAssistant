function isMobileDevice() {
    return /Android|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
}

function showNotification(title, body, url) {
    if ('Notification' in window && Notification.permission === 'granted') {
        new Notification(title, {
            body: body,
            icon: "MainIcon.png",
            data: { url: url || '/currents' }
        });
        console.log("Notification title " + title);
        return;
    }else{
        requestNotificationPermission();
        if(Notification.permission === 'granted') {
            new Notification(title, {
                body: body,
                icon: "MainIcon.png",
                data: { url: url || '/currents' }
            });
            console.log("Notification title " + title);
        }
    }
    console.log("Notification failed");
}

function requestNotificationPermission() {
    if (Notification.permission === 'default') {
        return Notification.requestPermission()
            .then(permission => permission === 'granted');
    }
    return Promise.resolve(Notification.permission === 'granted');
}

window.requestNotificationPermission = requestNotificationPermission;
window.showNotification = showNotification;