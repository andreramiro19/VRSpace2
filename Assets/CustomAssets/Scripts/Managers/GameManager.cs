using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Oculus.Platform;


namespace Metaversando.WorkSpace
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields

        public static GameManager Instance;
        public PlayerDataScriptableObject playerData;

        [Tooltip("The prefab to use for representing the player")]
        public GameObject[] playerPrefab;

        [Tooltip("Player Runnig Speed")]
        public float playerSpeed = 2;
        [Tooltip("Player Look Speed")]
        public float lookSensitivity = .5f;

        #endregion

        #region Private Serialize Fields

        [Tooltip("JV: User Avatar Id")]
        [SerializeField] private ulong m_userId;

        [Tooltip("JV: User Avatar Id")]
        [SerializeField] private Transform spawnPoint;

        [Tooltip("JV: User Avatar Id")]
        [SerializeField] private bool sceneTestOffline;

        public event Action ChangeSpecs;

        #endregion

        #region Enums
        public enum GameState
        {
            Intro,
            Login,
            Ingame
        }
        #endregion

        #region Mono Callbacks

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {         
            

            if (sceneTestOffline && !PhotonNetwork.IsConnected)
                InstantiateTestAvatar();

            else
            {
                //StartCoroutine(SetUserIdFromLoggedInUser());
                m_userId = 7827586797311627;
                StartCoroutine(InstantiateWhenUserIdIsFound());
            }   
        }
        #endregion

        #region Photon CallBacks/Methods

        public override void OnPlayerEnteredRoom(Player otherPlayer)
        {
            Debug.LogFormat("-> JV: OnPlayerEnteredRoom() {0}", otherPlayer.NickName);

            if(PhotonNetwork.IsMasterClient)
                Debug.LogFormat("-> JV: Is MasterClient? {0}", PhotonNetwork.IsMasterClient);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat("-> JV:OnPlayerLeftRoom() {0}", otherPlayer.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("-> JV: OnPlayerLeft is MasterClient? {0}", PhotonNetwork.IsMasterClient);
            }
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadSceneAsync(0);            
        }
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        #endregion //Photon CallBacks/Methods

        #region Instantiate Avatar

        void InstantiateNetworkedAvatar()
        {
            Vector3 spawnPos = spawnPoint.position;
            Int64 userId = Convert.ToInt64(m_userId);
            object[] objects = new object[5];
            objects[0] = userId;
            objects[1] = playerData.avatarId;
            objects[2] = playerData.playerName;
            objects[3] = playerData.playerHeight;
            objects[4] = playerData.playerDevice;
            GameObject _myAvatar = PhotonNetwork.Instantiate(playerPrefab[0].name, spawnPos, Quaternion.identity, 0, objects);
            DontDestroyOnLoad(_myAvatar);
            Debug.Log("Called Instantiate AVATAR");
        }

        void InstantiateTestAvatar()
        {
            Vector3 spawnPos = spawnPoint.position;
            GameObject _myAvatar = Instantiate(playerPrefab[1], spawnPos, Quaternion.identity);
            DontDestroyOnLoad(_myAvatar);
            Debug.Log("Called Instantiate TEST AVATAR");
        }

        IEnumerator InstantiateWhenUserIdIsFound()
        {
            while (m_userId == 0)
            {
                Debug.Log("Waiting for User id to be set before connecting to room");
                yield return null;
            }
            InstantiateNetworkedAvatar();
        }
        #endregion //Instantiate Avatar

        public void Quit()
        {
            UnityEngine.Application.Quit();
        }

        public void SetCharRunSpeed(float value)
        {
            playerSpeed = value;
            ChangeSpecs();
        }

        public void SetMouseSensitivity(float value)
        {
            lookSensitivity = value;
            ChangeSpecs();
        }

        public void SetPlayerName(string n)
        {
            playerData.playerName = n;
        }
    }
}