using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metaversando.WorkSpace
{
    public class ControllerManager : MonoBehaviour
    {
        #region Public Fields

        public static ControllerManager Instance;

        #endregion

        #region Private Fields
        
        private string m_DeviceType;

        #endregion

        #region Mono Callbacks

        void Start()
        {
            Instance = this;


            //Check if the device running this is a console
            if (SystemInfo.deviceType == DeviceType.Console)
            {
                //Change the text of the label
                m_DeviceType = "Console";

            }

            //Check if the device running this is a desktop
            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                m_DeviceType = "Desktop";
            }

            //Check if the device running this is a handheld
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                m_DeviceType = "Handheld";
            }

            //Check if the device running this is unknown
            if (SystemInfo.deviceType == DeviceType.Unknown)
            {
                m_DeviceType = "Unknown";
            }

            //Output the device type to the console window
            Debug.Log("Device type : " + m_DeviceType);

        }

        #endregion
    }
}
