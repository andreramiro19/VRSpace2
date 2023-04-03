using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using TMPro;
using System;

namespace Metaversando.WorkSpace
{
    /// <summary>
    /// Player manager
    /// </summary>
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;
        public static PlayerManager Instance;

        [Tooltip("The Player's UI GameObject Prefab")]
        public TMP_Text _playerTag;

        #endregion

        #region Private Serialize Fields

        [Header("Behaviors and Object to Enable if VR")]

        [Tooltip("JV: Use Desktop Settings if desktop")]
        [SerializeField] private bool useDesktopSettings = false;

        [Tooltip("List of Objects to find scripts to enable if player owned using VR")]
        [SerializeField] private List<GameObject> objScriptsToEnableVR;

        [Tooltip("List of scripts to enable in this Object if player owned using VR")]
        [SerializeField] private List<Behaviour> scriptsToEnableVR;

        [Tooltip("List of Objects to enable if player owned using VR")]
        [SerializeField] private List<GameObject> objectsToEnableVR;

        [Header("Behaviors and Object to Enable if Desktop")]
        [Tooltip("List of Objects to find scripts to enable if player owned using Desktop")]
        [SerializeField] private List<GameObject> objScriptsToEnableDesktop;

        [Tooltip("List of scripts to enable in this Object if player owned using Desktop")]
        [SerializeField] private List<Behaviour> scriptsToEnableDesktop;

        [Tooltip("List of Objects to enable if player owned using Desktop")]
        [SerializeField] private List<GameObject> objectsToEnableDesktop;

        #endregion

        #region private fields
        PlayerDataScriptableObject _playerdata;

        #endregion

        #region MonoBehaviour CallBacks

        void Awake()
        {
            if (photonView.IsMine)
            {
                EnablePlayerFuncionalities();
                PlayerManager.LocalPlayerInstance = this.gameObject;
                Instance = this;
                _playerdata = GameManager.Instance.playerData;
                DontDestroyOnLoad(this.gameObject);
                Debug.Log("photon is mine");
            }
            else
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                _playerTag.text = GetUsernameFromInstantiationdata();
                Destroy(rb);
                this.enabled = false;
            }
        }

        private void Start()
        {
            if (!photonView.IsMine)
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                _playerTag.text = GetUsernameFromInstantiationdata();
                Destroy(rb);
            }
        }

        private void CalledOnLevelWasLoaded()
        {
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }
        #endregion

        #region Custom

        void EnablePlayerFuncionalities()
        {
            if (GameManager.Instance.playerData.playerDevice == PlayerDataScriptableObject.Device.Desktop)
            {
                foreach (GameObject obj in objectsToEnableDesktop)
                    obj.SetActive(true);

                foreach (GameObject obj in objScriptsToEnableDesktop)
                    foreach (Behaviour behaviour in obj.GetComponents<Behaviour>())
                        behaviour.enabled = true;

                foreach (Behaviour behaviour in scriptsToEnableDesktop)
                    behaviour.enabled = true;
            }

            else if (GameManager.Instance.playerData.playerDevice == PlayerDataScriptableObject.Device.VR)
            {
                foreach (GameObject obj in objectsToEnableVR)
                    obj.SetActive(true);

                foreach (GameObject obj in objScriptsToEnableVR)
                    foreach (Behaviour behaviour in obj.GetComponents<Behaviour>())
                        behaviour.enabled = true;

                foreach (Behaviour behaviour in scriptsToEnableVR)
                    behaviour.enabled = true;
            }
        }
        #endregion

        #region IPunObservable implementation
        /*
        /// <summary>
        /// Process of sendind or receiving data from server, conected to photon view
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="info"></param>
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(IsFiring);
                stream.SendNext(Health);
            }
            else
            {
                // Network player, receive data
                this.IsFiring = (bool)stream.ReceiveNext();
                this.Health = (float)stream.ReceiveNext();
            }
        }
        */
        #endregion

        public string GetUsernameFromInstantiationdata()
        {
            object[] instantiationData = photonView.InstantiationData;
            string data_as_int = instantiationData[2].ToString();
            return data_as_int;
        }
    }   
}