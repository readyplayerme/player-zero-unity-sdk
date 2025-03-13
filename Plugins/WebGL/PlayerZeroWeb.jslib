mergeInto(LibraryManager.library, {
    GetDeviceData: function() {
        async function getDeviceData() {
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

            // Construct WebGL-specific data object
            const deviceData = {
                browser: browser,
                gpuModel: gpuModel
            };

            return allocate(intArrayFromString(JSON.stringify(deviceData)), "i8", ALLOC_NORMAL);
        }

        return getDeviceData();
    }
});
