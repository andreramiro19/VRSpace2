using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.VisualScripting;
using System.Runtime.InteropServices.WindowsRuntime;

public class SlideShowController : MonoBehaviour
{
    #region Private Serialize Fields

    [SerializeField] List<Texture2D> _textureList = new();

    #endregion

    #region Private Fields

    Material _materialThis;
    Texture _showingTexture;
    bool _loadingTextures;
    #endregion

    #region Public Fields


    #endregion

    #region Unity Callbacks
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadTextureList());
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    public void NextTexture()
    {
        for (int i = 0; i < _textureList.Count; i++)
        {
            if (_showingTexture == _textureList[i] && i != _textureList.Count - 1)
            {
                ChangeSlideTexture(i + 1);
                break;
            }
        }
    }

    public void PrevTexture()
    {
        for (int i = 0; i <= _textureList.Count; i++)
        {
            if (_showingTexture == _textureList[i] && i != 0)
            {
                ChangeSlideTexture(i - 1);
                break;
            }
        }
    }

    IEnumerator LoadTextureList()
    {

        string[] files = System.IO.Directory.GetFiles(Application.dataPath + "/ExternalImg");

        if(files == null)
            yield return null;
        foreach (string file in files)
        {
            _loadingTextures = true;
            StartCoroutine(LoadTextureFromCache(file));
            while (_loadingTextures)
                yield return null;
        }
        ChangeSlideTexture(0);
    }
    IEnumerator LoadTextureFromCache(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.Log("WRONG FILE PATH");
            yield break;
        }
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + filePath);
        yield return www.SendWebRequest();
        //texture loaded
        Texture2D _ = DownloadHandlerTexture.GetContent(www);
        if (_ != null)
        {
            _textureList.Add(_);
            Debug.Log(www.result);
        }
        _loadingTextures = false;
    }

    void ChangeSlideTexture(int i)
    {
        _showingTexture = _textureList[i];
        _materialThis = GetComponent<MeshRenderer>().material;
        _materialThis.mainTexture = _showingTexture;
    }
}
