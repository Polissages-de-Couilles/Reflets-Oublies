using PDC.Localization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    public int Priority { get => priority; set => priority = value; }
    [SerializeField] protected private int priority = 0;
    public float Range { get => range; set => range = value; }
    [SerializeField] protected float range = 3f;
    public bool IsAtRange => Distance <= Range;
    public float Distance => Vector3.Distance(GameManager.Instance.Player.transform.position, this.gameObject.transform.position);
    public virtual string Text => LocalizationManager.LocalizeText(textKey, true);
    protected string text;
    [SerializeField] protected string textKey;
    [SerializeField] protected TextMeshProUGUI textUI;
    [SerializeField] protected RectTransform worldUI;
    public bool UIShowAnyway => _uiShowAnyway;
    [SerializeField] protected bool _uiShowAnyway = false;

    private void Start()
    {
        LocalizationManager.OnLocaReady(() =>
        {
            text = Text;
        });
    }

    public abstract void OnInteraction();

    public virtual void FixedUpdate()
    {
        if (textUI.isActiveAndEnabled)
        {
            Quaternion lookRotation = Camera.main.transform.rotation;
            textUI.transform.rotation = lookRotation;
            worldUI.transform.rotation = lookRotation;
            //textUI.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
            //worldUI.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }

    public void SetUI(bool active)
    {
        if(textKey == string.Empty)
        {
            textUI.text = string.Empty;
            return;
        }
        textUI.text = text;
        textUI.gameObject.SetActive(active);
        worldUI.gameObject.SetActive(active);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
