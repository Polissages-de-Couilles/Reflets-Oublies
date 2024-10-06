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
    bool isVignette = false;
    bool isChromaticAberration = false;
    bool isColorAdjustments = false;

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

            IEnumerator EffectCoroutine()
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

            StartCoroutine(EffectCoroutine());
        }
    }
    public void FilmGrain(float intensity, float duration, bool snap = false)
    {
        if (isFilmGrain) return;
        isFilmGrain = true;

        if (CamVolumeProfile.TryGet(out FilmGrain filmGrain))
        {
            var _intensity = filmGrain.intensity.value;

            IEnumerator EffectCoroutine()
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

            StartCoroutine(EffectCoroutine());
        }
    }
    public void Vignette(float intensity, float duration, bool snap = false, bool infini = false)
    {
        Vignette(Color.black, intensity, duration, snap, infini);
    }
    public void Vignette(Color color, float intensity, float duration, bool snap = false, bool infini = false)
    {
        if (isVignette) return;
        isVignette = true;

        if (CamVolumeProfile.TryGet(out Vignette vignette))
        {
            var _intensity = vignette.intensity.value;
            var _color = vignette.color.value;

            IEnumerator EffectCoroutine()
            {
                if (snap)
                {
                    vignette.color.value = color;
                    vignette.intensity.value = intensity;
                    if (!infini)
                    {
                        yield return new WaitForSeconds(duration);
                        vignette.intensity.value = _intensity;
                        vignette.color.value = _color;
                    }
                }
                else
                {
                    Sequence s = DOTween.Sequence();

                    var thresholdTween = DOTween.To(() => vignette.color.value, x => vignette.color.value = x, color, infini ? duration : duration / 2f).Play();
                    s.Append(thresholdTween);

                    var intensityTween = DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, intensity, infini ? duration : duration / 2f).Play();
                    s.Append(intensityTween);

                    if (!infini)
                    {
                        yield return s.WaitForCompletion();

                        thresholdTween = DOTween.To(() => vignette.color.value, x => vignette.color.value = x, _color, duration / 2f).Play();
                        s.Append(thresholdTween);

                        intensityTween = DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, _intensity, duration / 2f).Play();
                        s.Append(intensityTween);

                        yield return s.WaitForCompletion();
                    }
                }
                yield return null;
                isVignette = false;
            }

            StartCoroutine(EffectCoroutine());
        }
    }
    public void ChromaticAberration(float intensity, float duration, bool snap = false)
    {
        if (isChromaticAberration) return;
        isChromaticAberration = true;

        if (CamVolumeProfile.TryGet(out ChromaticAberration chromaticAberration))
        {
            var _intensity = chromaticAberration.intensity.value;

            IEnumerator EffectCoroutine()
            {
                if (snap)
                {
                    chromaticAberration.intensity.value = intensity;
                    yield return new WaitForSeconds(duration);
                    chromaticAberration.intensity.value = _intensity;
                }
                else
                {
                    var intensityTween = DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, intensity, duration / 2f).Play();

                    yield return intensityTween.WaitForCompletion();

                    intensityTween = DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, _intensity, duration / 2f).Play();

                    yield return intensityTween.WaitForCompletion();
                }
                yield return null;
                isChromaticAberration = false;
            }

            StartCoroutine(EffectCoroutine());
        }
    }
    public void ColorAdjustments(float contrast, float saturation, float duration, bool snap = false, bool infini = false)
    {
        ColorAdjustments(Color.white, contrast, saturation, duration, snap, infini);
    }
    public void ColorAdjustments(Color colorFilter, float contrast, float saturation, float duration, bool snap = false, bool infini = false)
    {
        if (isColorAdjustments) return;
        isColorAdjustments = true;

        if (CamVolumeProfile.TryGet(out ColorAdjustments colorAdjustments))
        {
            var _contrast = colorAdjustments.contrast.value;
            var _colorFilter = colorAdjustments.colorFilter.value;
            var _saturation = colorAdjustments.saturation.value;

            IEnumerator EffectCoroutine()
            {
                if (snap)
                {
                    colorAdjustments.colorFilter.value = colorFilter;
                    colorAdjustments.contrast.value = contrast;
                    colorAdjustments.saturation.value = saturation;
                    if (!infini)
                    {
                        yield return new WaitForSeconds(duration);
                        colorAdjustments.contrast.value = _contrast;
                        colorAdjustments.colorFilter.value = _colorFilter;
                        colorAdjustments.saturation.value = _saturation;
                    }
                }
                else
                {
                    Sequence s = DOTween.Sequence();

                    var thresholdTween = DOTween.To(() => colorAdjustments.colorFilter.value, x => colorAdjustments.colorFilter.value = x, colorFilter, infini ? duration : duration / 2f).Play();
                    s.Append(thresholdTween);

                    var intensityTween = DOTween.To(() => colorAdjustments.contrast.value, x => colorAdjustments.contrast.value = x, contrast, infini ? duration : duration / 2f).Play();
                    s.Append(intensityTween);

                    var saturationTween = DOTween.To(() => colorAdjustments.saturation.value, x => colorAdjustments.saturation.value = x, saturation, infini ? duration : duration / 2f).Play();
                    s.Append(saturationTween);

                    if (!infini)
                    {
                        yield return s.WaitForCompletion();

                        thresholdTween = DOTween.To(() => colorAdjustments.colorFilter.value, x => colorAdjustments.colorFilter.value = x, _colorFilter, duration / 2f).Play();
                        s.Append(thresholdTween);

                        intensityTween = DOTween.To(() => colorAdjustments.contrast.value, x => colorAdjustments.contrast.value = x, _contrast, duration / 2f).Play();
                        s.Append(intensityTween);

                        saturationTween = DOTween.To(() => colorAdjustments.saturation.value, x => colorAdjustments.saturation.value = x, _saturation, duration / 2f).Play();
                        s.Append(saturationTween);

                        yield return s.WaitForCompletion();
                    }
                }
                yield return null;
                isColorAdjustments = false;
            }

            StartCoroutine(EffectCoroutine());
        }
    }
}
