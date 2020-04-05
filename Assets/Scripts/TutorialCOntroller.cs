using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class TutorialCOntroller : Singleton<TutorialCOntroller>
{
    public GameObject sündmus;
    public GameObject tileinfo;
    public GameObject üldinfo;

    private bool isTutorial = true;
    
    private void Start()
    {
        sündmus.SetActive(true);
        Time.timeScale = 0.0f;
    }

    // private void Update()
    // {
    //     if (isTutorial)
    //     {
    //         Time.timeScale = 0.0f;
    //     }
    // }

    public void ValdOk()
    {
        sündmus.SetActive(false);        
        tileinfo.SetActive(true);        
    }

    public void TileInfoOk()
    {
        tileinfo.SetActive(false);        
        üldinfo.SetActive(true);      
    }
    
    public void TutorialFinished()
    {
        üldinfo.SetActive(false);      
        Time.timeScale = 1.0f;
        isTutorial = false;
    }
}