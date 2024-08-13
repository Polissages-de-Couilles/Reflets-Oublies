using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDC.Localization;
using TMPro;

namespace MeetAndTalk
{
    public class UITextLocalizer : MonoBehaviour
    {
        [SerializeField] TMP_Text UIText;
        [SerializeField] string textKey;

        // Start is called before the first frame update
        void Start()
        {
                var t = LocalizationManager.GetLocalizedText(textKey);
                t = LocalizationManager.LocalizeText(t);
                UIText.SetText(t);
        }
    }
}
