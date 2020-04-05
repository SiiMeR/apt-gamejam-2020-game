using DG.Tweening;
using UnityEngine.UI;

public class GlobalFade : Singleton<GlobalFade>
{
    public Image Fade;
    
    public Tween FadeOut(float seconds = .7f)
    {
        return Fade.DOFade(1.0f, seconds)
            .SetEase(Ease.OutExpo);
    }
    
    public Tween FadeIn(float seconds = .7f)
    {
        return Fade.DOFade(0.0f, seconds)
            .SetEase(Ease.InQuint);
    }
}
