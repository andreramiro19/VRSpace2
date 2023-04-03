using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Oculus.Platform;
using TMPro;
using Oculus.Interaction;
using Metaversando.WorkSpace.Utils;

namespace Metaversando.WorkSpace
{
    public class PhotonLauncher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        /// Maximum Player in a room, if full, automatically creates another room

        [Header("Room Proprieties")]
        [Tooltip("JV: Maximum Player in a room, if full, automatically creates another room")]
        [SerializeField] private byte maxPlayersPerRoom = 4;
        [Tooltip("JV: Auto-connect")]
        [SerializeField] private bool autoConnect = false;
        [Tooltip("JV: User Avatar Id")]
        [SerializeField] private ulong m_userId;

        [Header("OnJoinRoom Proprieties")]
        [Tooltip("JV: Load another scene on Connect")]
        [SerializeField] private bool connectOtherScene;
        [Tooltip("JV: Scene name (string) toi connect to")]
        [SerializeField] private string sceneNameToLoad;

        [Header("UI Elements")]

        [Tooltip("Objects to Enable on connecting")]
        [SerializeField] private List<GameObject> loadingFeedbackObjects = new();
        [Tooltip("Objects to Disable on connecting")]
        [SerializeField] private List<GameObject> uiMainObjects = new();

        [Tooltip("JV: Connection Text")]
        [SerializeField] private TMP_Text m_screenText;

        [Header("Initial Player")]
        [Tooltip("Initial player to interact with menu")]
        [SerializeField] private GameObject[] VrPlayer;
        [SerializeField] private GameObject[] DesktopPlayer;

        [Header("Force Mode")]
        [Tooltip("Force VR or Desktop")]
        [SerializeField] private bool _forceVrPlayer;


        #endregion

        #region Private Fields

        private string projVersion = "1";
        private bool isConnecting;

        #endregion

        #region Public Fields

        public PlayerDataScriptableObject _playerData;

        #endregion

        #region Mono CallBacks

        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            if (SystemInfo.deviceType == DeviceType.Desktop && !_forceVrPlayer)
            {
                _playerData.playerDevice = PlayerDataScriptableObject.Device.Desktop;
                SwapEnableObjects.SwapLists(VrPlayer, DesktopPlayer, false);

            }
            else
            {
                _playerData.playerDevice = PlayerDataScriptableObject.Device.VR;
                SwapEnableObjects.SwapLists(VrPlayer, DesktopPlayer, true);
            }
        }

        void Start()
        {
            LoadingUI(false);
          

                // Using Fixed ID for testing
            m_userId = 7827586797311627;

            if (autoConnect)
                Connect();          
        }
        #endregion

        #region Public Methods

        public void Connect()
        {
            LoadingUI(true);

            if (PhotonNetwork.IsConnected)
                PhotonNetwork.JoinRandomRoom();
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = projVersion;
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                PhotonNetwork.JoinRandomOrCreateRoom();
                isConnecting = false;
                Debug.Log("-> JV: OnConnectedToMaster() Was called by PUN");
            }
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            // #Critical: failed to join a random room, maybe none exists or they are all full. create a new room.
            Debug.Log("-> JV: OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.LogWarning("Called OnJoinedRoom and SetStart");

            if (connectOtherScene)
                PhotonNetwork.LoadLevel(sceneNameToLoad);
            else
                SwapEnableObjects.Instance.SwapEnable(true);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            LoadingUI(false);
            isConnecting = false;           
            Debug.LogWarningFormat("-> JV: OnDisconnected() Was called by PUN with reason {0}", cause);
        }
        #endregion

        #region Set Id Number

        IEnumerator SetUserIdFromLoggedInUser()
        {
            if (OvrPlatformInit.status == OvrPlatformInitStatus.NotStarted)
                OvrPlatformInit.InitializeOvrPlatform();


            while (OvrPlatformInit.status != OvrPlatformInitStatus.Succeeded)
            {
                if (OvrPlatformInit.status == OvrPlatformInitStatus.Failed)
                {
                    Debug.LogError("OVR Platform failed to initialise");
                    m_screenText.text = "OVR Platform failed to initialise";
                    yield break;
                }
                yield return null;
            }

            Users.GetLoggedInUser().OnComplete(message =>
            {
                if (message.IsError)
                    Debug.LogError("Getting Logged in user error " + message.GetError());

                else
                {
                    m_userId = message.Data.ID;
                    if (autoConnect) Connect();
                }
            });
        }

        #endregion

        #region Loading UI

        /// <summary>
        /// Toggle Between active UIs - Loading or Main UI 
        /// </summary>
        /// <param name="loading"></param>
        void LoadingUI(bool loading)
        {
        }
        #endregion //Loading UI
    }
}
