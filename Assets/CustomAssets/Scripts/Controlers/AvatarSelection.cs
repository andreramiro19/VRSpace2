using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Oculus.Avatar2;
using Unity.VisualScripting;

namespace Metaversando.WorkSpace
{
    public class AvatarSelection : MonoBehaviour
    {
        #region Private Serialize Fields

        [Header ("Avatar GameObjects")]
        [SerializeField] private SampleAvatarEntity playerAvatarDesktop;
        [SerializeField] private SampleAvatarEntity playerAvatarVr;
        //[SerializeField] private Transform avatarShowcaseParent = default;
        //[SerializeField] private TMP_Text _avatarText = default;
        //[SerializeField] private float turnSpeed = 90f;

        [Header("UI Elements")]
        [Tooltip ("Scrollview Content")]
        [SerializeField] private Transform desktopUI;
        [Tooltip("Scrollview Content")]
        [SerializeField] private Transform ovrUI;

        #endregion

        #region Public Fields

        public static AvatarSelection Instance;
        public int selectedAvatarId = 0;
        public PlayerDataScriptableObject playerData;

        #endregion

        #region Mono Callback

        void Start()
        {
            Instance = this;
            if(playerData.playerDevice == PlayerDataScriptableObject.Device.VR)
                foreach (Transform o in ovrUI)
                {
                    o.name = o.GetSiblingIndex().ToString();
                    Toggle toggle = o.GetComponent<Toggle>();
                    toggle.group = ovrUI.GetComponent<ToggleGroup>();
                    toggle.onValueChanged.AddListener(delegate{
                        SelectAvatar(o.name);
                    });
                }
            else
                foreach (Transform o in desktopUI)
                {
                    o.name = o.GetSiblingIndex().ToString();
                    Toggle toggle = o.GetComponent<Toggle>();
                    toggle.group = desktopUI.GetComponent<ToggleGroup>();
                    toggle.onValueChanged.AddListener(delegate {
                        SelectAvatar(o.name);
                    });
                }
        }
        #endregion

        void SelectAvatar(string avatarId)
        {
            selectedAvatarId = int.Parse(avatarId);
            if(playerAvatarDesktop.isActiveAndEnabled) 
                playerAvatarDesktop.ReloadAvatarManually(avatarId, SampleAvatarEntity.AssetSource.Zip);
            else 
                playerAvatarVr.ReloadAvatarManually(avatarId, SampleAvatarEntity.AssetSource.Zip);

            playerData.avatarId = selectedAvatarId;
        }

    }
}
