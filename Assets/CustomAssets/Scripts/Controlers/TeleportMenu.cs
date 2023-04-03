using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeleportMenu : MonoBehaviour
{
    #region Private Serialize Fields
    
    [SerializeField] Transform _teleportPanel;
    [SerializeField] Transform _teleportParent;

    #endregion //Private Serialize Fields

    #region Private Fields
    GameObject _player;
    Button _teleportButton;

    #endregion //Private Fields
    void Start()
    {
        foreach (Transform t in _teleportParent)
        {
            GameObject _teleportObj = Instantiate(Resources.Load<GameObject>("Btn_Teleport"), _teleportPanel);
            TMP_Text _teleportText = _teleportObj.transform.Find("Text (TMP)").GetComponent<TMP_Text>();
            _teleportText.text = t.name;
            _teleportButton = _teleportObj.GetComponent<Button>();
            _teleportButton.onClick.AddListener(delegate { TeleportCharacter(t.position); });
        }        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TeleportCharacter(Vector3 teleportPosition)
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _player.transform.position = teleportPosition;
    }
}
