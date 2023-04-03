using System.Linq;
using Metaversando.WorkSpace;
using UnityEngine;
using Photon.Pun;

public class DrawMarker : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Private Serialize Fields

    [Header("Pen/Marker Draw Propierties")]
    [Tooltip("Pen contact transform")]
    [SerializeField] private Transform _tip;
    [Tooltip("Square Root of the brush size, _brushSize * _brushSize")]
    [SerializeField][Range(4, 32)] private int _brushSize;
    [Tooltip("Brush Color")]
    [SerializeField] Color _brushColor;
    [Header ("Slider Brush Size")]
    [Tooltip("Slide Brush Size Controler")]
    [SerializeField] SliderMeta _sliderMeta;
    [Header("Related Drawboard")]
    [Tooltip("fail...")]
    [SerializeField] DrawBoard _drawBoardRef;
    [Tooltip("Layer to hit with raycast")]
    [SerializeField] private int _layerToHit = 1 << 9;
    [Header ("Bools")]
    [Tooltip("Can Draw")]
    [SerializeField] bool _canDraw;
    [Tooltip("Use Interaction Comands")]
    [SerializeField] bool _isVR;
    #endregion

    #region Private Fields
    Color[] _colors;
    string _colorTotring;
    float _tipHeight;
    DrawBoard _drawBoard;
    bool _touchedLastFrame;
    bool _networkDraw;
    Vector3 _transformStartPosition;
    Vector2 _mousePosition;
    Vector2 _mouseLastPosition;
    Vector2 _touchPosition;
    Vector2 _touchLastPosition;
    Quaternion _lastRotation;
    RaycastHit _hit;
    Ray _ray;
    AudioSource _audioSource;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        //renderer = GetComponent<Renderer>();        
        if (_sliderMeta != null)
        {
            SliderMeta.ValueChanged += BrushSizeChange;
        }
        _tipHeight = _tip.localScale.y * 10;
        _transformStartPosition = transform.position;
        BrushColorChange(_brushColor);
        _audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (_networkDraw)
        {
            DrawNetwork();
        }
        if (photonView.IsMine && _drawBoardRef._insideDrawArea && _canDraw)
        {
            if (_isVR)
                DrawVR();

            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, 5, _layerToHit))
            {
                transform.position = _hit.point;

                if (Input.GetMouseButton(0))
                {
                    DrawDesktop();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    _drawBoard = null;
                    _touchedLastFrame = false;
                }
            }
            else
                PenPosReset();
        }
    }
    #endregion // unity callbacks

    #region Brush Methods
    public void BrushColorChange(Material mat)
    {
        _brushColor = mat.color;
        _colors = Enumerable.Repeat(_brushColor, _brushSize * _brushSize).ToArray();
        _tip.transform.GetComponent<MeshRenderer>().material.color = _brushColor;
        _colorTotring = "#" + ColorUtility.ToHtmlStringRGBA(_brushColor); 
    }
    public void BrushColorChange(Color color)
    {
        _brushColor = color;
        _colors = Enumerable.Repeat(_brushColor, _brushSize * _brushSize).ToArray();
        _tip.transform.GetComponent<MeshRenderer>().material.color = _brushColor;
        _colorTotring = "#" + ColorUtility.ToHtmlStringRGBA(_brushColor);
    }

    public void BrushSizeChange(float size)
    {
        _brushSize = (int)(4 + (124f * size));
        _colors = Enumerable.Repeat(_brushColor, _brushSize * _brushSize).ToArray();
    }
    #endregion

    #region Draw Functions
    private void DrawVR()
    {
        if (Physics.Raycast(origin: _tip.position, direction: transform.up, out _hit, _tipHeight))
        {
            if (_hit.transform.CompareTag("DrawCanvas2D"))
            {
                Debug.Log("hit canvas2d");
                if (_drawBoard == null)
                {
                    _drawBoard = _hit.transform.GetComponent<DrawBoard>();
                }
                _touchPosition = new Vector2(_hit.textureCoord.x, _hit.textureCoord.y);

                //DrawBoard Bounds
                var x = (int)(_touchPosition.x * _drawBoard.textureSize.x - (_brushSize / 2));
                var y = (int)(_touchPosition.y * _drawBoard.textureSize.y - (_brushSize / 2));

                //out of DrawBoard Bounds
                if (y < 0 || y > _drawBoard.textureSize.y || x < 0 || x > _drawBoard.textureSize.x)
                    return;
                _networkDraw = true;
                if (_touchedLastFrame)
                {
                    _drawBoard.boardTexture.SetPixels(x, y, blockWidth: _brushSize, blockHeight: _brushSize, _colors);

                    //lerp draw movement percentage
                    for (float f = 0.01f; f < 1; f += 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(a: _touchLastPosition.x, b: x, t: f);
                        var lerpY = (int)Mathf.Lerp(a: _touchLastPosition.y, b: y, t: f);
                        _drawBoard.boardTexture.SetPixels(lerpX, lerpY, blockWidth: _brushSize, blockHeight: _brushSize, _colors);
                    }
                    transform.rotation = _lastRotation;
                    _audioSource.Play();
                    _drawBoard.boardTexture.Apply();
                }
                _touchLastPosition = new Vector2(x, y);
                _lastRotation = transform.rotation;
                _touchedLastFrame = true;
                return;
            }
        }
        _drawBoard = null;
        _touchedLastFrame = false;
        _networkDraw = false;
    }
    private void DrawDesktop()
    {      
        if (_drawBoard == null)
        {
            _drawBoard = _hit.transform.GetComponent<DrawBoard>();
        }
        _mousePosition = new Vector2(_hit.textureCoord.x, _hit.textureCoord.y);

        //DrawBoard Bounds
        var x = (int)(_mousePosition.x * _drawBoard.textureSize.x - (_brushSize / 2));
        var y = (int)(_mousePosition.y * _drawBoard.textureSize.y - (_brushSize / 2));

        //out of DrawBoard Bounds
        if (y < 0 || y > _drawBoard.textureSize.y || x < 0 || x > _drawBoard.textureSize.x)
        {
            Debug.Log("Out of Bounds");
            return;
        }

        if (_touchedLastFrame)
        {
            _drawBoard.boardTexture.SetPixels(x, y, blockWidth: _brushSize, blockHeight: _brushSize, _colors);

            //lerp draw movement percentage
            for (float f = 0.01f; f < 1; f += 0.01f)
            {
                var lerpX = (int)Mathf.Lerp(a: _mouseLastPosition.x, b: x, t: f);
                var lerpY = (int)Mathf.Lerp(a: _mouseLastPosition.y, b: y, t: f);
                _drawBoard.boardTexture.SetPixels(lerpX, lerpY, blockWidth: _brushSize, blockHeight: _brushSize, _colors);
            }
            transform.rotation = _lastRotation;
            _drawBoard.boardTexture.Apply();
        }
        _mouseLastPosition = new Vector2(x, y);
        _lastRotation = Quaternion.Euler(90, 0, 0);
        _touchedLastFrame = true;
        _audioSource.Play();
        return;
    }
    private void DrawNetwork()
    {
        Debug.Log("DrawNetwork");
        DrawVR();
    }
    
    private void PenPosReset()
    {
        if (transform.position != _transformStartPosition)
        {
            transform.SetPositionAndRotation(_transformStartPosition, Quaternion.Euler(Vector3.zero));
        }
    }
    #endregion

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_networkDraw);
            stream.SendNext(_brushSize);
            stream.SendNext(_colorTotring);
        }

        else
        {
            _networkDraw = (bool)stream.ReceiveNext();

            if((int)stream.ReceiveNext() != _brushSize)
            {
                _brushSize = (int)stream.ReceiveNext();
                BrushSizeChange(_brushSize);
            }
            if (stream.ReceiveNext().ToString() != _colorTotring)
            {
                _colorTotring = stream.ReceiveNext().ToString();
                ColorUtility.TryParseHtmlString(_colorTotring, out _brushColor);
                BrushColorChange(_brushColor);
            }               
        }
    }
    #endregion

}
