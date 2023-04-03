using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Device Data", menuName = "ScriptableObjects/DeviceSO" )]
public class DeviceSO: ScriptableObject
{
    public int playerId;
    public string playerName;
    public int avatarId;
    public float playerHeight;
    public float mouseSensitivity;
    public float avatarSpeed;
}
