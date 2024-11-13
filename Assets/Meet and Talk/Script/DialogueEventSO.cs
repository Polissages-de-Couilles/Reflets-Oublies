using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MeetAndTalk.Event
{
    /// <summary>
    /// Basic Event Class at Meet and Talk.
    /// </summary>
    public abstract class DialogueEventSO : ScriptableObject
    {
        #region Variables
        // Here you can add the variables you want to change in the Scriptable Object
        #endregion

        /// <summary>.
        /// The RunEvent function is called by the Event Node
        /// It can also be called manually
        /// </summary>.
        public abstract void RunEvent();
    }

    public static class EventSOCreator
    {
#if UNITY_EDITOR
        [MenuItem("Assets/Create/Dialogue/New Event SO")]
        public static void NewEvent()
        {
            string script = "Assets/Meet and Talk/Script/Events/EventSOTemplate.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(script, "EventSO.cs");
        }
#endif
    }
}