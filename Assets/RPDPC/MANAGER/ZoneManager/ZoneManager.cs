using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZoneManager : MonoBehaviour
{
    public enum ZoneName
    {
        Ruine_Start,
        Bosquet,
        Village,
        Plaine,
        Cimetiere,
        Vallee_des_Murmures,
        Temple,
        Foret_des_Reflets,
        Ruines_de_l_Oubli,
        Ecluse,
        Temple_du_Reflet
    }

    [System.Serializable]
    public struct Zone
    {
        public ZoneName Name;
        public List<Collider> Collider;
    }

    [SerializeField] List<Zone> zones = new List<Zone>();
    [SerializeField] TextMeshProUGUI _ui;
    [SerializeField] List<Image> _images;
    public Zone CurrentZone { get; private set; }
    Coroutine _displayZoneNameCoroutine;
    Vector3 defaultPos;
    List<Tween> _tweenList = new();

    public void Start()
    {
        defaultPos = _ui.transform.parent.transform.localPosition;
    }

    public void OnZoneChange(Zone zone)
    {
        if (zone.Name == CurrentZone.Name) return;
        CurrentZone = zone;

        DisplayZoneName(zone);
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
        _ui.text = zone.Name.ToString().Replace("_", " ");
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
