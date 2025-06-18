using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCameraEffect : StagingEvent
{
    enum Type
    {
        ShakeCamera,
        Vignette
    }
    [SerializeField] Type type;
    [SerializeField] float intensity;
    [SerializeField] float duration;

    public override void PlayEvent()
    {
        base.PlayEvent();
        StartCoroutine(PlayEffect());
    }

    IEnumerator PlayEffect()
    {
        yield return null;

        switch (type)
        {
            case Type.ShakeCamera:
                GameManager.Instance.CamManager.ShakeCamera(intensity, duration);
                break;
            
            case Type.Vignette:
                break;
            
            default:
                break;
        }

        yield return new WaitForSeconds(duration);
        OnEventFinished?.Invoke();
    }
}
