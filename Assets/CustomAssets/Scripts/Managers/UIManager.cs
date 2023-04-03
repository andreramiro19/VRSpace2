using Metaversando.WorkSpace;
using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //TODO Redirect all UI code to this script

    #region Private Serialized Fields
    [Header("Pointable Canvas")]
    [Tooltip("If VR must activate pointable canvas")]
    [SerializeField] PointableCanvasModule module;
    
    #endregion

    #region Public Fields

    public static UIManager Instance;
    public PlayerDataScriptableObject playerData; //GAMBI

    #endregion

    #region Private Fields

    private bool _isVr;
    #endregion

    #region Unity Callback
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if(playerData.playerDevice == PlayerDataScriptableObject.Device.VR)
        {
            _isVr = true;
        }
        else
            _isVr = false;

        module.enabled = _isVr;

    }
    void Update()
    {
        
    }

    #endregion //Unity Callback

    #region
    public void ActivateCanvas(int gameState)
    {

    }

    #endregion

}
