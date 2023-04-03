using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Metaversando.WorkSpace
{
    public class ElevatorController : MonoBehaviour
    {
        #region Serialize Private Fields    

        [Header("Elevator")]

        [Tooltip("Elevator Transform")]
        [SerializeField] Transform _elevator;
        [Tooltip("Elevator move speed")]
        [SerializeField] float elevatorSpeed = 1f;
        [Tooltip("Time from opening doors to check next stacked call floor, it will close doors and move")]
        [SerializeField] float _stationedTimer = 8f;
        [Tooltip("Mininal distance between target to consider movement complete")]
        [SerializeField] float _minDistanceToStop = .01f;
        [Tooltip("Distance to Lerp")]
        [SerializeField] float _distanceToLerp = .5f;

        [Header("Floors")]
        [Tooltip("Elevator Shaft contains all Floor")]
        [SerializeField] Transform _shaft;
        [Tooltip("Each floor Transform (set OnStart)")]
        [SerializeField] List<Transform> _floors = new();
        [Tooltip("Each floor Doors")]
        [SerializeField] List<DoorController> _floorDoors = new();
        [Tooltip("Starting Floor")]
        [SerializeField] int startingFloor = 0;

        #endregion

        #region Private Fields

        int currentFloor;
        int desiredFloor;
        bool goingUp;
        bool isMoving = false;
        List<Vector3> _floorsHeight = new();
        List<int> callFloorStack = new();
        Vector3 _floorAdjusted;
        float _floorMinDistance;
        float _distanceFromFloor;

        #endregion

        #region Mono Callbacks
        void Start()
        {
            currentFloor = startingFloor;
            Vector3 lastPosition = Vector3.zero;

            foreach (Transform t in _shaft)
            {
                _floors.Add(t);
                _floorsHeight.Add(new Vector3(0, Vector3.Distance(lastPosition, t.position), 0));
                lastPosition = t.position;
                _floorDoors.Add(t.GetComponentInChildren<DoorController>());
                int _floorInt = t.GetSiblingIndex();
                Button _btn = t.Find("ButtonHandler").gameObject.GetComponentInChildren<Button>();
                Debug.Log(_btn);
                _btn.onClick.AddListener(delegate { CallElevator(_floorInt);});
            }
        }
        #endregion //Mono Callbacks

        #region Public Methods
        public void CallElevator(int floor)
        {
            //if elevator is in this floor, open doors, or move elevator
            if (floor == currentFloor)
            {
                Debug.Log("CurrentFloor {$}" + currentFloor );
                if (!_floorDoors[currentFloor].isOpen)
                    StartCoroutine(_floorDoors[currentFloor].OperateDoors());
                else 
                    return;
            }
            
            //if elevator is n not here and must called
            else
            {
                if (_floorDoors[currentFloor].isOpen)
                    StartCoroutine(_floorDoors[currentFloor].OperateDoors());

                Debug.Log("Called Elevator");
                if (callFloorStack.Count > 0)
                {
                    Debug.Log("Called Elevator Has Stacks");
                    if (isMoving)
                    {
                        if (goingUp)
                        {
                            // if this call is between current floor and desired floor
                            if (floor < desiredFloor && floor > currentFloor)
                            {
                                callFloorStack.Insert(0, floor);
                                desiredFloor = floor;
                            }
                            else
                                callFloorStack.Add(floor);
                        }
                        else
                        {
                            if (floor > desiredFloor && floor < currentFloor)
                            {
                                callFloorStack.Insert(0, floor);
                                desiredFloor = floor;
                            }
                            else
                                callFloorStack.Add(floor);
                        }
                    }
                    // if elevator is stopped on another floor
                    else
                    {
                        callFloorStack.Add(floor);
                        desiredFloor = floor;
                    }
                }
                else
                {
                    callFloorStack.Add(floor);
                    desiredFloor = floor;
                    StartCoroutine(MoveElevator());
                }
                
            }
        }

        #endregion //Public Methods

        #region Coroutines

        IEnumerator MoveElevator()
        {
            Debug.Log("Before Move = Desired floor " + desiredFloor + " Current Floor = " + currentFloor);

            _floorAdjusted = new(_elevator.localPosition.x, _floors[desiredFloor].localPosition.y, _elevator.localPosition.z);
            _distanceFromFloor = Vector3.Distance(_elevator.localPosition, _floorAdjusted);
            isMoving = true;

            while (_distanceFromFloor >= _minDistanceToStop)
            {   
                if(_distanceFromFloor <= _distanceToLerp) //Decrease Elevator speed till stop
                {
                    Debug.Log("isLerping");
                    _elevator.localPosition = Vector3.Lerp(_elevator.localPosition, _floorAdjusted, elevatorSpeed * Time.fixedDeltaTime);
                }
                else // Move at same speed
                {
                    Debug.Log("MovingTowards");
                    _elevator.localPosition = Vector3.MoveTowards(_elevator.localPosition, _floorAdjusted, elevatorSpeed * Time.fixedDeltaTime);
                }               
                _distanceFromFloor = Vector3.Distance(_elevator.localPosition, _floorAdjusted);
                Debug.Log(_distanceFromFloor);
                yield return null;
            }
            isMoving = false;
            currentFloor = desiredFloor;
            StartCoroutine(_floorDoors[currentFloor].OperateDoors());
            callFloorStack.Remove(desiredFloor);
            StartCoroutine(StationedTimer());
            Debug.Log(" After Move = Desired floor {$} " + desiredFloor + " Current Floor = " + currentFloor);
        }

        IEnumerator StationedTimer()
        {
            yield return new WaitForSeconds(_stationedTimer);
            StartCoroutine(_floorDoors[currentFloor].OperateDoors());
            if (callFloorStack.Count > 0)
            {
                desiredFloor = callFloorStack[0];
                StartCoroutine(MoveElevator());
            }
        }
        #endregion //coroutines
    }
}