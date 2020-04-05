using DG.Tweening;
using UnityEngine.UI;

public class GlobalFade : Singleton<GlobalFade>
{
    public Image Fade;
    
    public Tween FadeOut(float seconds = .5f)
    {
        return Fade.DOFade(1.0f, seconds)
            .SetUpdate(true)
            .SetEase(Ease.OutExpo);
    }
    
    public Tween FadeIn(float seconds = .7f)
    {
        return Fade.DOFade(0.0f, seconds)
            .SetUpdate(true)
            .SetEase(Ease.InQuint);
    }
}
