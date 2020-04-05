using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GlobalFade : Singleton<GlobalFade>
{
    public Image Fade;
    
    public Tween FadeOut(float seconds = 1f)
    {
        return Fade.DOFade(1.0f, seconds)
            .SetEase(Ease.OutExpo);
    }
    
    public Tween FadeIn(float seconds = 1f)
    {
        return Fade.DOFade(0.0f, seconds)
            .SetEase(Ease.InExpo);
    }
}
