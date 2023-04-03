using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "ScriptableObjects/PlayerDataScriptableObject" )]
public class PlayerDataScriptableObject : ScriptableObject
{
    public int playerId;
    public string playerName;
    public int avatarId;
    public float playerHeight;
    public Device playerDevice;

    public float mouseSensitivity;
    public float avatarSpeed;

    public enum Device {VR,Desktop}
}
