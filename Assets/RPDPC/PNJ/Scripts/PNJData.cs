using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeetAndTalk;
using UnityEditor;
using System.Linq;

[CreateAssetMenu(menuName ="Game/PNJData")]
public class PNJData : ScriptableObject
{
    [SerializeField] DialogueCharacterSO _character;
    [SerializeField] List<DialogueContainerSO> _allDialogue = new List<DialogueContainerSO>();


#if UNITY_EDITOR
    [EditorCools.Button]
    public void CreateDialogue()
    {
        var dialogue = ScriptableObject.CreateInstance<DialogueContainerSO>();
        dialogue.name = $"Dialogue_{_allDialogue.Count + 1}";
        _allDialogue.Add(dialogue);

        AssetDatabase.AddObjectToAsset(dialogue, this);
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(dialogue);
    }

    [EditorCools.Button]
    public void DeleteLastDialogue()
    {
        if (_allDialogue == null || _allDialogue.Count <= 0) return;

        var dialogue = _allDialogue.Last();
        _allDialogue.Remove(dialogue);
        Undo.DestroyObjectImmediate(dialogue);
        AssetDatabase.SaveAssets();
    }
#endif
    //private void OnGUI()
    //{
    //    if (GUILayout.Button("Create Dialogue"))
    //    {
    //        CreateDialogue();
    //    }
    //}
}
