using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorInside : MonoBehaviour
{
    #region Public Fields
    public delegate void OnTriggerDelegate(bool isInside, GameObject player);
    public static OnTriggerDelegate playerInsideDelegate;
    public bool _playerInside = false;
    public GameObject _player;

    #endregion //Public Fields

    #region Private Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player = other.gameObject;
            _playerInside = true;
            Debug.Log("player inside " + _playerInside);
            playerInsideDelegate(_playerInside, _player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerInside = false;
            Debug.Log("player inside " + _playerInside);
            playerInsideDelegate(_playerInside, _player);
        }
    }
    #endregion //Private Methods
}
