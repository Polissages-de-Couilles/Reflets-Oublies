using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventFade : DialogueEventTimeEvent
{
    public enum FadeType
    {
        FadeIn,
        FadeOut,
        FadeOutIn,
        FadeInOut,
    }
    public FadeType fadeType;

    public float duration;

    public override void RunEvent()
    {
        IsEventEnd = false;
        GameManager.Instance.StartCoroutine(FadeCoroutine());
    }

    IEnumerator FadeCoroutine()
    {
        IsEventEnd = false;
        yield return null;

        switch (fadeType)
        {
            case FadeType.FadeIn:
                yield return FadeIn(duration);
                break;
            
            case FadeType.FadeOut:
                yield return FadeOut(duration);
                break;
            
            case FadeType.FadeOutIn:
                yield return FadeOut(duration / 2f);
                yield return FadeIn(duration / 2f);
                break;
            
            case FadeType.FadeInOut:
                yield return FadeIn(duration / 2f);
                yield return FadeOut(duration / 2f);
                break;
            
            default:
                break;
        }

        EventEnd();
    }

    IEnumerator FadeIn(float duration)
    {
        yield return GameManager.Instance.FadeObject.DOColor(new Color(GameManager.Instance.FadeObject.color.r, GameManager.Instance.FadeObject.color.g, GameManager.Instance.FadeObject.color.b, 1f), duration).WaitForCompletion();
    }

    IEnumerator FadeOut(float duration)
    {
        yield return GameManager.Instance.FadeObject.DOColor(new Color(GameManager.Instance.FadeObject.color.r, GameManager.Instance.FadeObject.color.g, GameManager.Instance.FadeObject.color.b, 0f), duration).WaitForCompletion();
    }
}
