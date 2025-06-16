using DG.Tweening;
using PDC.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZoneManager : MonoBehaviour
{
    public enum ZoneName
    {
        UI_ZONE_1,
        UI_ZONE_2,
        UI_ZONE_3,
        UI_ZONE_4,
        UI_ZONE_5,
        UI_ZONE_6,
        UI_ZONE_7,
        UI_ZONE_8,
        UI_ZONE_9,
        UI_ZONE_10,
        UI_ZONE_11
    }

    [System.Serializable]
    public struct Zone
    {
        public ZoneName Name;
        public List<Collider> Collider;
        public AudioClip AmbianceSound;
        public AudioClip Music;
    }

    [SerializeField] List<Zone> zones = new List<Zone>();
    [SerializeField] TextMeshProUGUI _ui;
    [SerializeField] List<Image> _images;
    [SerializeField] Material _fog;
    public Zone CurrentZone { get; private set; }
    Coroutine _displayZoneNameCoroutine;
    Vector3 defaultPos;
    List<Tween> _tweenList = new();
    public Action<Zone> OnZoneChangeEvent;

    public void Start()
    {
        if(_ui == null)
        {
            this.enabled = false;
            return;
        }
        defaultPos = _ui.transform.parent.transform.localPosition;
        //var color = _fog.GetColor("FogColor");
        //Debug.Log(color);
        ////_fog.SetColor("FogColor", new Color(color.r, color.g, color.b, 0f));
        //_fog.DOColor(new Color(color.r, color.g, color.b, 0f), "FogColor", 1f);
        OnZoneChange(zones.First());
    }

    public void OnZoneChange(Zone zone)
    {
        if (zone.Name == CurrentZone.Name) return;
        CurrentZone = zone;
        OnZoneChangeEvent?.Invoke(zone);

        DisplayZoneName(zone);

        GameManager.Instance.AudioManager.SwitchZone(zone);

        Color color;
        switch (zone.Name)
        {
            //Ruine Start
            case ZoneName.UI_ZONE_1:
                GameManager.Instance.CamManager.SetFog(0.1f, 1f, false);
                break;

            //Bosquet
            case ZoneName.UI_ZONE_2:
                break;

            //Village
            case ZoneName.UI_ZONE_3:
                GameManager.Instance.CamManager.SetFog(0.1f, 1f, false);
                break;

            //Plaine
            case ZoneName.UI_ZONE_4:
                GameManager.Instance.CamManager.SetFog(0.1f, 1f, false);
                break;

            //Cimetiere
            case ZoneName.UI_ZONE_5:
                color = _fog.GetColor("FogColor");
                _fog.DOColor(new Color(color.r, color.g, color.b, 0f), "FogColor", 1f);
                GameManager.Instance.CamManager.SetFog(0.1f, 1f, true);
                break;

            //Vallee des murmures
            case ZoneName.UI_ZONE_6:
                color = _fog.GetColor("FogColor");
                _fog.DOColor(new Color(color.r, color.g, color.b, 0.6666667f), "FogColor", 1f);
                GameManager.Instance.CamManager.SetFog(0.35f, 1f, true);
                break;

            //Temple
            case ZoneName.UI_ZONE_7:
                break;

            //Foret des reflets
            case ZoneName.UI_ZONE_8:
                GameManager.Instance.CamManager.SetFog(0.1f, 1f, false);
                break;

            //Ruine de l'oublie
            case ZoneName.UI_ZONE_9:
                GameManager.Instance.CamManager.SetFog(0.1f, 1f, true);
                break;

            //Ecluse
            case ZoneName.UI_ZONE_10:
                GameManager.Instance.CamManager.SetFog(0.1f, 1f, false);
                break;

            //Temple du reflet
            case ZoneName.UI_ZONE_11:
                break;

            default:
                break;
        }
    }

    private void DisplayZoneName(Zone zone)
    {
        if (_displayZoneNameCoroutine != null)
        {
            for (int i = 0; i < _tweenList.Count; i++)
            {
                _tweenList[i].Pause();
                _tweenList[i].Kill();
            }
            _tweenList.Clear();
            foreach (var image in _images)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
            }
            _ui.color = new Color(_ui.color.r, _ui.color.g, _ui.color.b, 0f);
            StopCoroutine(_displayZoneNameCoroutine);
            _ui.transform.parent.transform.localPosition = defaultPos;
        }
        _displayZoneNameCoroutine = StartCoroutine(DisplayZoneNameCoroutine(zone));
    }

    IEnumerator DisplayZoneNameCoroutine(Zone zone)
    {
        _ui.text = LocalizationManager.LocalizeText(zone.Name.ToString(), true);
        var pos = _ui.transform.parent.transform.localPosition;

        foreach (var image in _images)
        {
            _tweenList.Add(image.DOColor(new Color(image.color.r, image.color.g, image.color.b, 1f), 1f));
        }
        _tweenList.Add(_ui.DOColor(new Color(_ui.color.r, _ui.color.g, _ui.color.b, 1f), 1f));
        var t = _ui.transform.parent.transform.DOLocalMove(pos - new Vector3(0, 175f, 0), 1f);
        _tweenList.Add(t);
        yield return t.WaitForCompletion();

        yield return new WaitForSeconds(1f);

        foreach (var image in _images)
        {
            _tweenList.Add(image.DOColor(new Color(image.color.r, image.color.g, image.color.b, 0f), 1f));
        }
        _tweenList.Add(_ui.DOColor(new Color(_ui.color.r, _ui.color.g, _ui.color.b, 0f), 1f));
        _tweenList.Remove(t);
        t = _ui.transform.parent.transform.DOLocalMove(pos, 1f);
        _tweenList.Add(t);
        yield return t.WaitForCompletion();
        _displayZoneNameCoroutine = null;
    }

    public Zone GetZone(ZoneName zoneName)
    {
        return zones.Find(x => x.Name == zoneName);
    }

}
