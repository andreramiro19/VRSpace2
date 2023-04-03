using Oculus.Interaction.DistanceReticles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Metaversando.WorkSpace
{
    public class DrawLineRenderWorld : MonoBehaviour
    {
        #region Private Serialize Fields

        [Header("Draw Board")]
        [Header("Graffiti Mode")]
        [Tooltip("If true draw over wherever hits")]
        [SerializeField] private bool worldPaint = false;

        [SerializeField]
        DistantInteractionLineRendererVisual _oculusLineRenderer;
        [SerializeField]
        private LineRenderer _lineRenderer;
        [SerializeField]
        Camera _camera;
        [SerializeField]
        GameObject brush;

        [SerializeField]
        Color _color;

        #endregion

        #region Private Fields

        Vector3 mousePosition;
        Vector3 _lastPosition;
        RaycastHit hit;
        Ray ray;

        #endregion

        #region Unity Callbacks
        private void Start()
        {
            if (_camera == null)
                _camera = Camera.main;

        }
        private void Update()
        {
            Draw();
        }

        #endregion

        private void Draw()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CreateBrush();
            }
            if (Input.GetMouseButton(0))
            {
                //old Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    if (!worldPaint && !hit.transform.CompareTag("DrawCanvas3D"))
                        return;

                    mousePosition = hit.point;
                    if (mousePosition != _lastPosition)
                    {
                        AddPoint(mousePosition);
                        _lastPosition = mousePosition;
                    }
                }
            }
            else
                _lineRenderer = null;
        }
        void CreateBrush()
        {
                GameObject brushInstance = Instantiate(brush, this.transform);
                _lineRenderer = brushInstance.GetComponent<LineRenderer>();
                _lineRenderer.startColor = _color;
                _lineRenderer.endColor = _color;
        }

        void AddPoint(Vector3 pointPos)
        {
            _lineRenderer.positionCount++;
            int positionIndex = _lineRenderer.positionCount -1;
            _lineRenderer.SetPosition(positionIndex, pointPos);
        }
    }

}
