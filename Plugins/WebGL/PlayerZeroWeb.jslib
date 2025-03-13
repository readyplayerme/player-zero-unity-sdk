mergeInto(LibraryManager.library, {
    GetDeviceData: function() {
        async function generateHashedDeviceId(fingerprint) {
            const encoder = new TextEncoder();
            const data = encoder.encode(fingerprint);
            const hashBuffer = await crypto.subtle.digest("SHA-256", data);
            return Array.from(new Uint8Array(hashBuffer))
                .map(b => b.toString(16).padStart(2, "0"))
                .join("");
        }

        async function getOrCreateDeviceData() {
            let firstLoadTime = localStorage.getItem("timeOfFirstGameLoad");
            if (!firstLoadTime) {
                firstLoadTime = Date.now().toString();
                localStorage.setItem("timeOfFirstGameLoad", firstLoadTime);
            }

            const browser = navigator.userAgent;
            const os = navigator.platform;
            const screenResolution = `${screen.width}x${screen.height}`;
            const cpuCores = navigator.hardwareConcurrency || "Unknown";

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

            const fingerprint = `${firstLoadTime}-${browser}-${os}-${gpuModel}-${screenResolution}-${cpuCores}`;
            let deviceId = localStorage.getItem("device_id");
            if (!deviceId) {
                deviceId = await generateHashedDeviceId(fingerprint);
                localStorage.setItem("device_id", deviceId);
            }

            const deviceData = {
                deviceId: deviceId,
                timeOfFirstGameLoad: firstLoadTime,
                browser: browser,
                os: os,
                gpuModel: gpuModel,
                screenResolution: screenResolution,
                cpuCores: cpuCores
            };

            return allocate(intArrayFromString(JSON.stringify(deviceData)), "i8", ALLOC_NORMAL);
        }

        return getOrCreateDeviceData();
    }
});
