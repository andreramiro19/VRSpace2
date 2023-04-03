using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DOORS MUST BE AT 0, DOR MESHES CENTRALIZED ON DOOR OBJECT
/// </summary>
public class DoorController : MonoBehaviour 
{
    [TextArea]
    public string MyTextArea = "DOORS MUST BE OBJECTS AT (0,0,0), DOOR MESHES CENTRALIZED ON DOOR OBJECT";

    #region Public Fields 

    [Header("Doors")]
    [Tooltip("Door Center Transform")]
    public Transform doorC;
    [Tooltip("Door Left Transform")]
    public Transform doorL;
    [Tooltip("Door Right Transform")]
    public Transform doorR;
    [Tooltip("Speed that the door will open")]
    public float doorSpeed = 0.5f;
    [Tooltip("Total door aperture")]
    public float doorGap = 3f;
    [Tooltip("Has two doors?")]
    public bool doubleDoor;
    [Tooltip("Door is open?")]
    public bool isOpen;

    #endregion //public fields

    #region Private Fields

    #endregion // Private Fields

    #region Coroutines
    public IEnumerator OperateDoors()
    {
        if (doubleDoor)
        {
            Vector3 _gapZeroL = new(0, doorL.localPosition.y, 0);
            Vector3 _gapZeroR = new(0, doorR.localPosition.y, 0);
            if (isOpen)
            {
                while (isOpen)
                {
                    doorL.localPosition = Vector3.Lerp(doorL.localPosition, _gapZeroL, doorSpeed);
                    doorR.localPosition = Vector3.Lerp(doorR.localPosition, _gapZeroR, doorSpeed);
                    if (doorL.localPosition == _gapZeroL && doorR.localPosition == _gapZeroR)
                    {
                        isOpen = false;
                    }
                    yield return null;
                }
            }
            else //door is closed
            {
                Vector3 _doorGapL = new(doorGap / 2 *-1, doorL.localPosition.y, 0);
                Vector3 _doorGapR = new(doorGap / 2, doorR.localPosition.y, 0);
                while (!isOpen)
                {
                    doorL.localPosition = Vector3.Lerp(doorL.localPosition, _doorGapL, doorSpeed);
                    doorR.localPosition = Vector3.Lerp(doorR.localPosition, _doorGapR, doorSpeed);
                    if (doorL.localPosition == _doorGapL && doorR.localPosition == _doorGapR)
                    {
                        isOpen = true;
                    }
                    yield return null;
                }
            }
        }
        if (!doubleDoor)
        {
            if (isOpen)
            {
                while (isOpen)
                {
                    doorC.localPosition = Vector3.Lerp(doorC.localPosition, Vector3.zero, doorSpeed);
                    if (doorC.localPosition == Vector3.zero)
                    {
                        isOpen = false;
                    }
                    yield return !isOpen;
                }
            }
            else
            {
                Vector3 _doorGap = new(doorGap, 0, 0);
                while (!isOpen)
                {
                    doorC.localPosition = Vector3.Lerp(doorC.localPosition, _doorGap, doorSpeed);
                    if (doorC.localPosition == _doorGap)
                    {
                        isOpen = true;
                    }
                    yield return isOpen;
                }
            }
        }
    }

    #endregion //Coroutines

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(OperateDoors());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(OperateDoors());
        }
    }
}
