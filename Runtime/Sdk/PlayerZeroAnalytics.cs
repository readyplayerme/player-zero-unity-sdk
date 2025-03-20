using System;
using System.Collections;
using UnityEngine;
using System.Globalization;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
#endif

namespace PlayerZero.Runtime.Sdk
{
    public class PlayerZeroAnalytics : MonoBehaviour
    {
        private const string PZ_SESSION_ID = "PZ_SESSION_ID";
        private const string PZ_AVATAR_SESSION_ID = "PZ_AVATAR_SESSION_ID";
        private const int HEARTBEAT_INTERVAL_IN_SECONDS = 60;

        private static PlayerZeroAnalytics _instance;
        private Coroutine _heartbeatTimer;
        public bool debugMode;

        private long lastPlayerActivityAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        private Vector3 lastMousePosition;

        private void Awake()
        {
            if (_instance == null || _instance == this)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);

#if ENABLE_INPUT_SYSTEM
                InputSystem.onEvent += OnNewInputEvent;
#endif

                _heartbeatTimer = StartCoroutine(Heartbeat());
            }
            else
            {
                Destroy(this);
            }
        }
#if ENABLE_LEGACY_INPUT_MANAGER
        private void Update()
        {

            if (DetectLegacyInput())
            {
                lastPlayerActivityAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }

        }
        
        private bool DetectLegacyInput()
        {
            if (Input.anyKeyDown) return true;

            if (Input.mousePosition != lastMousePosition)
            {
                lastMousePosition = Input.mousePosition;
                return true;
            }

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) return true;

            if (Input.touchCount > 0) return true;

            if (Input.GetJoystickNames().Length > 0)
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f)
                    return true;
            }

            return false;
        }
#endif

#if ENABLE_INPUT_SYSTEM
        private void OnNewInputEvent(InputEventPtr eventPtr, InputDevice device)
        {
            lastPlayerActivityAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
#endif

        private IEnumerator Heartbeat()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(HEARTBEAT_INTERVAL_IN_SECONDS);
                Debug.Log("Sending heartbeat event with last activity: " + lastPlayerActivityAt);
            }
        }

        private void OnDestroy()
        {
            if (_instance != this)
                return;

            _instance = null;

#if ENABLE_INPUT_SYSTEM
            InputSystem.onEvent -= OnNewInputEvent;
#endif

            StopAllCoroutines();
        }
    }
}
