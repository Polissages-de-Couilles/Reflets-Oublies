using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MeetAndTalk;

namespace PDC.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour, ILocalization
    {
        TextMeshProUGUI m_TextMeshProUGUI;
        [SerializeField] string _key;
        public string Key => _key;

        public Action OnLanguageChange { get; set; }

        public DialogueContainerSO testDialogue;

        public void Start()
        {
            LocalizationManager.OnLocalizationReady += () =>
            {
                m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
                m_TextMeshProUGUI.text = GetLocalizedText(Key);
                OnLanguageChange += () => m_TextMeshProUGUI.text = GetLocalizedText(Key);

                DialogueManager.Instance.StartDialogue(testDialogue);
            };
        }

        public string GetLocalizedText(string key)
        {
            var t = LocalizationManager.GetLocalizedText(key);
            //[#VALUE]
            return LocalizationManager.LocalizeText(t);
        }
    }
}