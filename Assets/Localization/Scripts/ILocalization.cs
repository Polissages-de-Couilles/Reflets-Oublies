using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDC.Localization
{
    public interface ILocalization
    {
        public string Key { get; }
        public string GetLocalizedText(string key);
    }
}