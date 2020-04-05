using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        DOTween.Sequence()
            .Append(GlobalFade.Instance.FadeOut())
            .AppendCallback(() => SceneManager.LoadScene("Questions"))
            .Append(GlobalFade.Instance.FadeIn());
    }

    public void OnVälja()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
    }
}

