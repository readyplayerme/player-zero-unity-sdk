mergeInto(LibraryManager.library, {
    GetDeviceData: function() {
        const browser = navigator.userAgent;
        let gpuModel = "Unknown";

        try {
            let canvas = document.createElement("canvas");
            let gl = canvas.getContext("webgl") || canvas.getContext("experimental-webgl");
            if (gl) {
                gpuModel = gl.getParameter(gl.RENDERER);
            }
        } catch (e) {
            console.warn("WebGL not supported or blocked");
        }

        const deviceData = {
            browser: browser,
            gpuModel: gpuModel
        };

        return JSON.stringify(deviceData);
    }
});
