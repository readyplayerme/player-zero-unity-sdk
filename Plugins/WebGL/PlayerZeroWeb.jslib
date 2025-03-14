mergeInto(LibraryManager.library, {
    GetBrowser: function () {
        const browser = navigator.userAgent || "Unknown";
        console.log("JS: Returning Browser -> " + browser);

        var bufferSize = lengthBytesUTF8(browser) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(browser, buffer, bufferSize);
        return buffer;
    }
});