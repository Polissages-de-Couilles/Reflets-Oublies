using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineEffectManager : MonoBehaviour
{
    CinemachineVirtualCamera Cam;
    Tween shakeTween;
    [SerializeField] private Volume CamVolume;
    private VolumeProfile CamVolumeProfile => CamVolume.profile;
    bool isBloom = false;
    bool isFilmGrain = false;

    private void Awake()
    {
        Cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = Cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        if(shakeTween != null && shakeTween.IsPlaying()) shakeTween.Pause();
        shakeTween = DOTween.To(() => cinemachineBasicMultiChannelPerlin.m_AmplitudeGain, x => cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = x, 0f, time);
        shakeTween.Play();
    }

    public void Bloom(float threshold, float intensity, float duration, bool snap = false)
    {
        if(isBloom) return;
        isBloom = true;

        Debug.Log(CamVolumeProfile.Has<Bloom>());
        if (CamVolumeProfile.TryGet(out Bloom bloom))
        {
            var _threshold = bloom.threshold.value;
            var _intensity = bloom.intensity.value;

            IEnumerator BloomCoroutine()
            {
                if (snap)
                {
                    bloom.threshold.value = threshold;
                    bloom.intensity.value = intensity;
                    yield return new WaitForSeconds(duration);
                    bloom.threshold.value = _threshold;
                    bloom.intensity.value = _intensity;
                }
                else
                {
                    Sequence s = DOTween.Sequence();
                    
                    var thresholdTween = DOTween.To(() => bloom.threshold.value, x => bloom.threshold.value = x, threshold, duration / 2f).Play();
                    s.Append(thresholdTween);
                    
                    var intensityTween = DOTween.To(() => bloom.intensity.value, x => bloom.intensity.value = x, intensity, duration / 2f).Play();
                    s.Append(intensityTween);
                    
                    yield return s.WaitForCompletion();

                    thresholdTween = DOTween.To(() => bloom.threshold.value, x => bloom.threshold.value = x, _threshold, duration / 2f).Play();
                    s.Append(thresholdTween);

                    intensityTween = DOTween.To(() => bloom.intensity.value, x => bloom.intensity.value = x, _intensity, duration / 2f).Play();
                    s.Append(intensityTween);

                    yield return s.WaitForCompletion();
                }
                yield return null;
                isBloom = false;
            }

            StartCoroutine(BloomCoroutine());
        }
    }
    public void FilmGrain(float intensity, float duration, bool snap = false)
    {
        if (isFilmGrain) return;
        isFilmGrain = true;

        if (CamVolumeProfile.TryGet(out FilmGrain filmGrain))
        {
            var _intensity = filmGrain.intensity.value;

            IEnumerator BloomCoroutine()
            {
                if (snap)
                {
                    filmGrain.intensity.value = intensity;
                    yield return new WaitForSeconds(duration);
                    filmGrain.intensity.value = _intensity;
                }
                else
                {
                    var intensityTween = DOTween.To(() => filmGrain.intensity.value, x => filmGrain.intensity.value = x, intensity, duration / 2f).Play();

                    yield return intensityTween.WaitForCompletion();

                    intensityTween = DOTween.To(() => filmGrain.intensity.value, x => filmGrain.intensity.value = x, _intensity, duration / 2f).Play();

                    yield return intensityTween.WaitForCompletion();
                }
                yield return null;
                isFilmGrain = false;
            }

            StartCoroutine(BloomCoroutine());
        }
    }
}
