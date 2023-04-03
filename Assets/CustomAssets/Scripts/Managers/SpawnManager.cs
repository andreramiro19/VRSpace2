using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Public Fields

    public GameObject spawnPoints;
    public Dictionary<GameObject, bool> spawnPointsDict = new();


    public static SpawnManager Instance;

    #endregion

    #region Mono Callbacks

    private void Start()
    {
        foreach (Transform spawnPoint in spawnPoints.transform)
        {
            spawnPointsDict.Add(spawnPoint.gameObject, false);
        }
        Instance = this;
    }
    #endregion

    #region Public Methods
    public Vector3 AvailableSpawnSlot(){

        foreach (var Slot in spawnPointsDict)
        {
            if (!Slot.Value)
            {
                Debug.Log("-->JV: Slot" + Slot.Key.name);
                spawnPointsDict[Slot.Key] = true;
                return Slot.Key.transform.position;
            }
        }
        Debug.LogError("-->JV: All Slots Ocupied");
        return Vector3.zero;              

    }
    #endregion
}
