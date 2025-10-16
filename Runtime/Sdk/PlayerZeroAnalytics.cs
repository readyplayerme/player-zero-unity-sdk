using System;
using System.Collections;
using System.Globalization;
using PlayerZero.Api.V1;
using PlayerZero.Data;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
#endif

namespace PlayerZero.Runtime.Sdk
{
    /// <summary>
    /// Handles Player Zero analytics session management, heartbeat, and activity tracking for avatars and games.
    /// </summary>
    public class PlayerZeroAnalytics : MonoBehaviour
    {
        /// <summary>
        /// PlayerPrefs key for the game session ID.
        /// </summary>
        private const string PZ_SESSION_ID = "PZ_SESSION_ID";
        /// <summary>
        /// PlayerPrefs key for the avatar session ID.
        /// </summary>
        private const string PZ_AVATAR_SESSION_ID = "PZ_AVATAR_SESSION_ID";
        /// <summary>
        /// Interval in seconds between heartbeat events.
        /// </summary>
        private const int HEARTBEAT_INTERVAL_IN_SECONDS = 60;

        private static PlayerZeroAnalytics _instance;
        private static Settings _settings;

        private Coroutine _heartbeatTimer;

        /// <summary>
        /// Enables debug logging for analytics events.
        /// </summary>
        public bool debugMode;
        private static DeviceContext _deviceContext;
        
        private long lastPlayerActivityAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        private Vector3 lastMousePosition;
        
        /// <summary>
        /// Initializes the analytics session, device context, and heartbeat timer.
        /// </summary>
        private void Awake()
        {
            _settings = Resources.Load<Settings>("PlayerZeroSettings");
            if (_settings == null || string.IsNullOrEmpty(_settings.GameId))
            {
                Debug.LogError("Player Zero Game ID is required. Please set it in tools -> Player Zero.");
                return;
            }
            
            if (_instance == null || _instance == this)
            {
#if ENABLE_INPUT_SYSTEM
        // Capture any input event (keyboard, mouse, gamepad, touch)
        InputSystem.onEvent += OnNewInputEvent;
#endif
                _instance = this;
                DontDestroyOnLoad(gameObject);
                _deviceContext = DeviceAnalytics.GetDeviceInfo();
                if (_heartbeatTimer != null)
                    StopCoroutine(_heartbeatTimer);

                if (!string.IsNullOrEmpty(PlayerZeroSdk.GetHotLoadedAvatarId()))
                {
                    if (debugMode)
                        Debug.Log("Sending game session started event to Player Zero.");

                    var sessionId =
                        PlayerZeroSdk.StartEventSession<GameSessionStartedEvent, GameSessionStartedProperties>(
                            new GameSessionStartedEvent()
                            {
                                Properties = new GameSessionStartedProperties()
                                {
                                    AvatarId = PlayerZeroSdk.GetHotLoadedAvatarId()
                                }
                            }
                        );

                    if (debugMode)
                        Debug.Log("Sending avatar session started event to Player Zero.");

                    var avatarSessionId =
                        PlayerZeroSdk.StartEventSession<AvatarSessionStartedEvent, AvatarSessionStartedProperties>(
                            new AvatarSessionStartedEvent()
                            {
                                Properties = new AvatarSessionStartedProperties()
                                {
                                    AvatarId = PlayerZeroSdk.GetHotLoadedAvatarId(),
                                    GameSessionId = sessionId,
                                    SdkVersion = _settings.Version,
                                    SdkPlatform = "Unity",
                                    DeviceContext = _deviceContext
                                }
                            }
                        );

                    PlayerPrefs.SetString(PZ_SESSION_ID, sessionId);
                    PlayerPrefs.SetString(PZ_AVATAR_SESSION_ID, avatarSessionId);
                    PlayerPrefs.Save();
                }

                _heartbeatTimer = StartCoroutine(Heartbeat());
            }
            else
            {
                Debug.LogWarning("Removing duplicate Player Zero analytics component.");
                Destroy(this);
            }
        }
        
#if ENABLE_LEGACY_INPUT_MANAGER  
        /// <summary>
        /// Updates player activity timestamp if legacy input is detected.
        /// </summary>
        private void Update()
        {
            if (DetectLegacyInput())
            {
                lastPlayerActivityAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
        }
        
        /// <summary>
        /// Detects player activity using the legacy input system.
        /// </summary>
        /// <returns>True if activity is detected; otherwise, false.</returns>
        private bool DetectLegacyInput()
        {
            // Legacy Input System (No setup needed)
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
        /// <summary>
        /// Updates player activity timestamp when a new input event is received.
        /// </summary>
        /// <param name="eventPtr">The input event pointer.</param>
        /// <param name="device">The input device.</param>
        private void OnNewInputEvent(InputEventPtr eventPtr, InputDevice device)
        {
            // Any input event from any device
            lastPlayerActivityAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
#endif

        /// <summary>
        /// Coroutine that sends heartbeat events at regular intervals.
        /// </summary>
        private IEnumerator Heartbeat()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(HEARTBEAT_INTERVAL_IN_SECONDS);

                if (!string.IsNullOrEmpty(PlayerZeroSdk.GetHotLoadedAvatarId()) &&
                    PlayerPrefs.HasKey(PZ_AVATAR_SESSION_ID))
                {
                    if (debugMode)
                        Debug.Log("Sending avatar heartbeat event to Player Zero.");

                    PlayerZeroSdk.SendEvent<AvatarSessionHeartbeatEvent, AvatarSessionHeartbeatProperties>(
                        new AvatarSessionHeartbeatEvent()
                        {
                            Properties = new AvatarSessionHeartbeatProperties()
                            {
                                SessionId = PlayerPrefs.GetString(PZ_AVATAR_SESSION_ID),
                                LastAvatarActivityAt = lastPlayerActivityAt
                            }
                        });
                }
            }
        }

        /// <summary>
        /// Cleans up analytics sessions and sends session ended events on destruction.
        /// </summary>
        private void OnDestroy()
        {
            if (_instance != this)
                return;

            _instance = null;
#if ENABLE_INPUT_SYSTEM
        InputSystem.onEvent -= OnNewInputEvent; // Cleanup to avoid memory leaks
#endif

            if (string.IsNullOrEmpty(PlayerZeroSdk.GetHotLoadedAvatarId()))
                return;

            if (PlayerPrefs.HasKey(PZ_AVATAR_SESSION_ID))
            {
                if (debugMode)
                    Debug.Log("Sending avatar session ended event to Player Zero.");
                
                PlayerZeroSdk.SendEvent<AvatarSessionEndedEvent, AvatarSessionEndedProperties>(
                    new AvatarSessionEndedEvent()
                    {
                        Properties = new AvatarSessionEndedProperties()
                        {
                            SessionId = PlayerPrefs.GetString(PZ_AVATAR_SESSION_ID)
                        }
                    }
                );

                PlayerPrefs.DeleteKey(PZ_AVATAR_SESSION_ID);
            }

            if (PlayerPrefs.HasKey(PZ_SESSION_ID))
            {
                if (debugMode)
                    Debug.Log("Sending game session ended event to Player Zero.");
                
                PlayerZeroSdk.SendEvent<GameSessionEndedEvent, GameSessionEndedProperties>(
                    new GameSessionEndedEvent()
                    {
                        Properties = new GameSessionEndedProperties()
                        {
                            SessionId = PlayerPrefs.GetString(PZ_SESSION_ID)
                        }
                    }
                );

                PlayerPrefs.DeleteKey(PZ_SESSION_ID);
            }

            PlayerPrefs.Save();
            StopAllCoroutines();
        }
    }
}