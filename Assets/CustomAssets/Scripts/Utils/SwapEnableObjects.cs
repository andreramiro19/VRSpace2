using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Metaversando.WorkSpace.Utils
{
    /// <summary>
    /// Swaps ENABLE state of selected objects and components 
    /// </summary>
    public class SwapEnableObjects : MonoBehaviour
    {
        #region Public Fields

        public static SwapEnableObjects Instance;

        #endregion //Public Fields 

        #region Private Serialize Fields

        [Header("Behaviors and Object to SWAP ENABLE")]
        [Tooltip("If true dont swap at Start")]
        [SerializeField] private bool startEnable = true;

        [Header("Behaviors and Object to START ENABLED")]
        [Tooltip("List of Objects to find scripts to START ENABLED(enable only components)")]
        [SerializeField] private List<GameObject> objScriptsToEnableAtStart;
        [Tooltip("List of scripts to START ENABLED (solo scripts)")]
        [SerializeField] private List<Behaviour> scriptsToEnableAtStart;
        [Tooltip("List of Objects to to START ENABLED (enable whole object)")]
        [SerializeField] private List<GameObject> objectsToEnableAtStart;

        [Header("Behaviors and Object to START DISABLED")]
        [Tooltip("List of Objects to find scripts to START ENABLED (disable only components)")]
        [SerializeField] private List<GameObject> objScriptsToDisableAtStart;
        [Tooltip("List of scripts to START DISABLED (solo scripts)")]
        [SerializeField] private List<Behaviour> scriptsToDisableAtStart;
        [Tooltip("List of Objects to to START DISABLED (disabled whole object)")]
        [SerializeField] private List<GameObject> objectsToDisableAtStart;

        #endregion //Private Serialize Fields

        private void Awake()
        {
            //inverted to start enabled
            SwapEnable(!startEnable);
            Instance = this;
        }
        public void SwapEnable(bool swap)
        {
                foreach (GameObject obj in objScriptsToEnableAtStart)
                    foreach (Behaviour behaviour in obj.GetComponents<Behaviour>())
                        if(behaviour != null)
                            behaviour.enabled = !swap;

                foreach (Behaviour behaviour in scriptsToEnableAtStart)
                    if (behaviour != null)
                        behaviour.enabled = !swap;

                foreach (GameObject obj in objectsToEnableAtStart)
                    if (obj != null) 
                        obj.SetActive(!swap);

                foreach (GameObject obj in objectsToDisableAtStart)
                    if (obj != null) 
                        obj.SetActive(swap);

            foreach (GameObject obj in objScriptsToDisableAtStart)
                foreach (Behaviour behaviour in obj.GetComponents<Behaviour>())
                         if (behaviour != null)
                            behaviour.enabled = swap;
            foreach (Behaviour behaviour in scriptsToDisableAtStart)                   
                    if (behaviour != null)
                        behaviour.enabled = swap;
        }

        public static void SwapLists(GameObject[] listA, GameObject[] listB, bool activateA)
        {           
            foreach(GameObject o in listA)
            {
                o.SetActive(activateA);
            }

            foreach (GameObject o in listB)
            {
                o.SetActive(!activateA);
            }
        }
    }
}