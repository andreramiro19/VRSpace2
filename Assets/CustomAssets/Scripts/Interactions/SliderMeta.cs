using CSCore;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Metaversando.WorkSpace
{
    public class SliderMeta : MonoBehaviour
    {
        #region Private Serialize Fields

        [SerializeField] Transform _slideBar;
        [SerializeField] Transform _slideHandle;
        [SerializeField] Transform _slideTop;
        [SerializeField] Transform _slideBot;
        [SerializeField][Range(0f, 1f)] float _slideCurrent;
        [SerializeField][Range(0f, 4f)] float _slideSize;
        [SerializeField] UnityEvent OnValueChange;
        #endregion

        #region Public Fields

        public float SliderOutput { get { return _slideSize;}}

        public delegate void OnValueChanged(float slidePosition);
        public static event OnValueChanged ValueChanged;


        #endregion

        #region Private Fields

        float _barMax, _barMin, _barNormalized;
        bool _touchedLastFrame;
        Vector3 _transformStartPosition;
        Vector2 _mousePosition, _mouseLastPosition;
        RaycastHit _hit;
        Ray _ray;

        #endregion        

        #region Unity Callbacks
        // Start is called before the first frame update
        void Start()
        {
            _barMax = _slideBar.transform.localScale.y;
            _slideBar.localPosition = new Vector3(0,_barMax / 2,0);
            _barMin = 0;
            _slideTop.localPosition = new Vector3(0, _barMax, 0);
            _slideCurrent = _slideHandle.transform.localPosition.y / _barMax;
        }

        // Update is called once per frame
        void Update()
        {
            //_slideHandle.transform.localPosition = new(0, _slideCurrent * _barMax, 0);

            if(Input.GetMouseButton(0))
            {
                InteractWithHandle();
            }
        }
        #endregion

        void InteractWithHandle()
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, 5))
            {
                if (!_hit.transform.CompareTag("Interactable"))
                    return;

                _mousePosition = _hit.point;

                if (_touchedLastFrame)
                {
                    if(_mousePosition.y < _mouseLastPosition.y)
                    {
                        //is falling
                        if(_slideHandle.transform.localPosition.y <= _barMin)
                            return;
                    }
                    else if(_mousePosition.y > _mouseLastPosition.y)
                    {
                        //is rising
                        if (_slideHandle.transform.localPosition.y >= _barMax)
                            return;
                    }

                    _slideHandle.transform.localPosition += new Vector3(0, _mousePosition.y - _mouseLastPosition.y, 0);
                    ValueChanged(_slideHandle.transform.localPosition.y/_barMax);
                }

                _mouseLastPosition = _mousePosition;
                _touchedLastFrame = true;
                return;
            }

            _touchedLastFrame = false;
        }
    }
}
