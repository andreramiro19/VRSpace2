using Metaversando.WorkSpace;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AvatarPositionAdjustment : MonoBehaviour
{
    #region Private Serialize Fields

    [Header("Avatar Transform Adjustment")]
    [SerializeField] private GameObject m_Avatar;
    [Tooltip("Forward eye/head distance")]
    [SerializeField] private float m_AvatarOffsetZ = -0.08f;
    [Tooltip("Avatar Height to subtract from collider")]
    [SerializeField] private float m_AvatarOVRHeight = -1.0f;
    [Tooltip("Avatar Height to subtract from collider")]
    [SerializeField] private float m_AvatarDesktopHeight = -1.60f;

    private Vector3 m_AvatarAdjustedPosition;

    #endregion //Private Serialize Fields

    #region Mono Callbacks
    private void Start()
    {
        if (m_Avatar == null)
        {
            m_Avatar = FindObjectOfType<CustomAvatarEntity>().gameObject;
        }
    }
    #endregion //Mono Callbacks

    #region Public Methods

    public void AdjustAvatarPosition(bool isVR, bool photonIsMine)
    {
        if(!isVR)
        {
            m_Avatar.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            m_Avatar.transform.localPosition = new Vector3(0, m_AvatarDesktopHeight, m_AvatarOffsetZ);
            
        }
        else
        { 
            if (!photonIsMine)
                m_Avatar.transform.localPosition = new Vector3(0, m_AvatarOVRHeight, 0);
        }
            

    }

    #endregion //Public Methods
}
