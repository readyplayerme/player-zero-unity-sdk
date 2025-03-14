mergeInto(LibraryManager.library, {
    GetDeviceData: function () {
        const browser = navigator.userAgent || "Unknown";
        let gpuModel = "Unknown";

        try {
            let canvas = document.createElement("canvas");
            let gl = canvas.getContext("webgl") || canvas.getContext("experimental-webgl");
            if (gl) {
                gpuModel = gl.getParameter(gl.RENDERER) || "Unknown";
            }
        } catch (e) {
            console.warn("WebGL not supported or blocked");
        }

        // ✅ Ensure JSON string is valid
        const jsonData = JSON.stringify({
            browser: browser,
            gpuModel: gpuModel
        });

        console.log("JS: Returning Device Data -> " + jsonData);

        // ✅ Allocate memory and return pointer
        var bufferSize = lengthBytesUTF8(jsonData) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(jsonData, buffer, bufferSize);
        return buffer;
    }
});
