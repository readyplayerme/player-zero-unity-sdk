using System.Collections;
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

        private IEnumerator Heartbeat()
        {
            while (true)
            {
                yield return new WaitForSeconds(HEARTBEAT_INTERVAL_IN_SECONDS);

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
                                SessionId = PlayerPrefs.GetString(PZ_AVATAR_SESSION_ID)
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
        }
    }
}