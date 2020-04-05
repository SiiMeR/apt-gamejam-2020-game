using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAlusta()
    {
        SceneManager.LoadScene("Questions");
    }

    public void OnVälja()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
    }
}

