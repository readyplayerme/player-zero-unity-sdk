using System;
using System.Collections;
using System.Globalization;
using PlayerZero.Api.V1;
using PlayerZero.Data;
using UnityEngine;

namespace PlayerZero.Runtime.Sdk
{
    public class PlayerZeroAnalytics : MonoBehaviour
    {
        private const string PZ_SESSION_ID = "PZ_SESSION_ID";
        private const string PZ_AVATAR_SESSION_ID = "PZ_AVATAR_SESSION_ID";
        private const int HEARTBEAT_INTERVAL_IN_SECONDS = 60;

        private static PlayerZeroAnalytics _instance;
        private static Settings _settings;

        private Coroutine _heartbeatTimer;

        public bool debugMode;
        
        private long lastPlayerActivityAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        private Vector3 lastMousePosition;
        
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
                                    GameSessionId = sessionId
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
        
        private void Update()
        {
            if (DetectLegacyInput())
            {
                lastPlayerActivityAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
        }
        
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
        
#if ENABLE_INPUT_SYSTEM
    private void OnNewInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        // Any input event from any device
        lastPlayerActivityAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
#endif

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