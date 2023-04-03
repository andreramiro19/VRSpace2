using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class DrawBoard : MonoBehaviour
{
    #region Private Serialize Fields

    [SerializeField] Texture[] _textureList;
    [SerializeField] Transform _penStartPosition;
    Material _boardMaterial;
    Color _boardColor;

    
    MeshRenderer _mesh;
    Collider _colliderDrawArea;


    #endregion

    #region Public fields
    public bool _insideDrawArea;
    public Texture2D boardTexture;
    public Vector2 textureSize = new(x: 2048, y: 1024);
    [Tooltip ("Suggested 0.630f")]
    public float _boardAlphaCut = 0.630f;

    #endregion //Public fields

    #region Unity Callbacks
    void Start()
    {
        _mesh = GetComponent<MeshRenderer>();
        _boardMaterial = _mesh.material;
        _boardColor = _boardMaterial.color;
        ClearBoard();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _insideDrawArea = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _insideDrawArea = false;
        }
           
    }

    #endregion //Unity Callbacks

    public void ClearBoard()
    {
            var r = GetComponent<Renderer>();
            boardTexture = new Texture2D(width: (int)textureSize.x, height: (int)textureSize.y);
            r.material.mainTexture = boardTexture;
    }

    public void ChangeBoardTransparency()
    {       

        if (_boardMaterial.color == new Color(1, 1, 1, _boardAlphaCut))
            _boardMaterial.color = Color.white;
        else
            _boardMaterial.color = new(1, 1, 1, _boardAlphaCut);
    }

    void InstantiatePen()
    {
        PhotonNetwork.Instantiate("PenMarker", _penStartPosition.position, Quaternion.identity);
    }


}
