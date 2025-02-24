using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using MeetAndTalk.GlobalValue;
using MeetAndTalk.Localization;
using PDC.Localization;
using System;
using Random = UnityEngine.Random;

namespace MeetAndTalk
{
    public class DialogueManager : DialogueGetData
    {
        [HideInInspector] public static DialogueManager Instance;
        public MeetAndTalk.Localization.LocalizationManager localizationManager;

        [HideInInspector] public DialogueUIManager dialogueUIManager;
        public AudioSource audioSource;

        public UnityEvent StartDialogueEvent;
        public UnityEvent EndDialogueEvent;
        public bool isDialogueInProcess;

        public Action<BaseNodeData> OnNode;
        private BaseNodeData currentDialogueNodeData;
        private BaseNodeData lastDialogueNodeData;

        private TimerChoiceNodeData _nodeTimerInvoke;
        private DialogueNodeData _nodeDialogueInvoke;
        private DialogueChoiceNodeData _nodeChoiceInvoke;

        float Timer;
        private uint soundID;

        private void Awake()
        {
            Instance = this;
            dialogueUIManager= DialogueUIManager.Instance;
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            Timer -= Time.deltaTime;
            if (Timer > 0) dialogueUIManager.TimerSlider.value = Timer;
        }

        public void SetupDialogue(DialogueContainerSO dialogue)
        {
            dialogueContainer = dialogue;
        }

        public void StartDialogue(DialogueContainerSO dialogue)
        {
            dialogueUIManager = DialogueUIManager.Instance;
            dialogueContainer = dialogue;

            if (dialogueContainer.StartNodeDatas.Count == 1) CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[0]));
            else { CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[Random.Range(0, dialogueContainer.StartNodeDatas.Count)])); }

            dialogueUIManager.dialogueCanvas .SetActive(true);
            isDialogueInProcess = true;
            StartDialogueEvent.Invoke();
        }

        public void StartDialogue(string ID)
        {
            dialogueUIManager = DialogueUIManager.Instance;

            // Try Get Start with ID
            bool withID = false;
            for(int i = 0; i < dialogueContainer.StartNodeDatas.Count; i++)
            {
                if(dialogueContainer.StartNodeDatas[i].startID == ID)
                {
                    CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[i]));
                    withID = true;
                }
            }
            if (!withID)
            {
                if (dialogueContainer.StartNodeDatas.Count == 1) CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[0]));
                else { CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[Random.Range(0, dialogueContainer.StartNodeDatas.Count)])); }
            }

            dialogueUIManager.dialogueCanvas.SetActive(true);
            isDialogueInProcess = true;
            StartDialogueEvent.Invoke();
        }

        public void StartDialogue()
        {
            dialogueUIManager= DialogueUIManager.Instance;

            if (dialogueContainer.StartNodeDatas.Count == 1) CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[0]));
            else { CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[Random.Range(0, dialogueContainer.StartNodeDatas.Count)])); }

            dialogueUIManager.dialogueCanvas.SetActive(true);
            isDialogueInProcess = true;
            StartDialogueEvent.Invoke();
        }

        public void CheckNodeType(BaseNodeData _baseNodeData)
        {
            switch (_baseNodeData)
            {
                case StartNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case DialogueNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case DialogueChoiceNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case TimerChoiceNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case EventNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case EndNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case RandomNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case IfNodeData nodeData:
                    RunNode(nodeData);
                    break;
                default:
                    break;
            }
        }


        private void RunNode(StartNodeData _nodeData)
        {
            CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[0]));
        }
        private void RunNode(RandomNodeData _nodeData)
        {
            CheckNodeType(GetNodeByGuid(_nodeData.DialogueNodePorts[Random.Range(0, _nodeData.DialogueNodePorts.Count)].InputGuid));
        }
        private void RunNode(IfNodeData _nodeData)
        {
            string ValueName = _nodeData.ValueName;
            GlobalValueIFOperations Operations = _nodeData.Operations;
            string OperationValue = _nodeData.OperationValue;

            GlobalValueManager manager = Resources.Load<GlobalValueManager>("GlobalValue");
            manager.LoadFile();

            Debug.Log("XXXX" + _nodeData.TrueGUID + "XXXX");
            CheckNodeType(GetNodeByGuid(manager.IfTrue(ValueName, Operations, OperationValue) ? _nodeData.TrueGUID : _nodeData.FalseGUID));
        }
        private void RunNode(DialogueNodeData _nodeData)
        {
            lastDialogueNodeData = currentDialogueNodeData;
            currentDialogueNodeData = _nodeData;
            OnNode?.Invoke(currentDialogueNodeData);

            GlobalValueManager manager = Resources.Load<GlobalValueManager>("GlobalValue");
            manager.LoadFile();

            var nameGlobal = PDC.Localization.LocalizationManager.GetLocalizedText(_nodeData.Character.CustomizedName.ValueName);
            nameGlobal = PDC.Localization.LocalizationManager.RemoveDiacritics(PDC.Localization.LocalizationManager.LocalizeText(nameGlobal));

            var name = PDC.Localization.LocalizationManager.GetLocalizedText(_nodeData.Character.characterName[0].LanguageGenericType); 
            name = PDC.Localization.LocalizationManager.RemoveDiacritics(PDC.Localization.LocalizationManager.LocalizeText(name));

            // Gloval Value Multiline
            if (dialogueUIManager.showSeparateName && dialogueUIManager.nameTextBox != null && _nodeData.Character != null && _nodeData.Character.UseGlobalValue) { dialogueUIManager.ResetText(""); dialogueUIManager.nameTextBox.text = nameGlobal; }
            // Normal Multiline
            else if (dialogueUIManager.showSeparateName && dialogueUIManager.nameTextBox != null && _nodeData.Character != null) { dialogueUIManager.ResetText(""); dialogueUIManager.nameTextBox.text = name; }
            // No Change Character Multiline
            else if (dialogueUIManager.showSeparateName && dialogueUIManager.nameTextBox != null && _nodeData.Character != null) { dialogueUIManager.ResetText(""); }
            // Global Value Inline
            else if (_nodeData.Character != null && _nodeData.Character.UseGlobalValue) dialogueUIManager.ResetText(nameGlobal);
            // Normal Inline
            else if (_nodeData.Character != null) dialogueUIManager.ResetText(name);
            // Last Change
            else dialogueUIManager.ResetText("");

            var t = PDC.Localization.LocalizationManager.GetLocalizedText(_nodeData.TextType[0].LanguageGenericType);
            //[#VALUE]
            t = PDC.Localization.LocalizationManager.LocalizeText(t);

            dialogueUIManager.SetFullText(t);

            // Character Avatar
            dialogueUIManager.SpriteLeft.SetActive(false); dialogueUIManager.SpriteRight.SetActive(false);
            if (_nodeData.AvatarPos == AvatarPosition.Left && _nodeData.Character != null) { dialogueUIManager.SpriteLeft.SetActive(true); dialogueUIManager.SpriteLeft.GetComponent<Image>().sprite = _nodeData.Character.GetAvatar(_nodeData.AvatarPos, _nodeData.AvatarType); }
            if (_nodeData.AvatarPos == AvatarPosition.Right && _nodeData.Character != null) { dialogueUIManager.SpriteRight.SetActive(true); dialogueUIManager.SpriteRight.GetComponent<Image>().sprite = _nodeData.Character.GetAvatar(_nodeData.AvatarPos, _nodeData.AvatarType); }

            dialogueUIManager.SkipButton.SetActive(!_nodeData.CantBeSkip);
            MakeButtons(new List<DialogueNodePort>());

            if (_nodeData.AudioClips.Find(clip => clip.languageEnum == localizationManager.SelectedLang()).LanguageGenericType != null)
            {
                if(audioSource.isPlaying) audioSource.Pause();
                audioSource.PlayOneShot(_nodeData.AudioClips.Find(clip => clip.languageEnum == localizationManager.SelectedLang()).LanguageGenericType);
            }

            string _audioName = _nodeData.AudioName.Find(clip => clip.languageEnum == localizationManager.SelectedLang()).LanguageGenericType;
            if (!_audioName.Equals(string.Empty))
            {
                soundID = AkSoundEngine.PostEvent(_audioName, GameManager.Instance.AudioDialogueGameObject);
                GameManager.Instance.DialogueManager.OnNode += StopAudio;
                GameManager.Instance.DialogueManager.EndDialogueEvent.AddListener(StopAudio);
            }

            _nodeDialogueInvoke = _nodeData;

            IEnumerator tmp() { yield return new WaitForSeconds(_nodeData.Duration); DialogueNode_NextNode(); }
            if(_nodeData.Duration != 0) StartCoroutine(tmp());
        }
        private void RunNode(DialogueChoiceNodeData _nodeData)
        {
            lastDialogueNodeData = currentDialogueNodeData;
            currentDialogueNodeData = _nodeData;
            OnNode?.Invoke(currentDialogueNodeData);

            GlobalValueManager manager = Resources.Load<GlobalValueManager>("GlobalValue");
            manager.LoadFile();

            var nameGlobal = PDC.Localization.LocalizationManager.GetLocalizedText(_nodeData.Character.CustomizedName.ValueName);
            nameGlobal = PDC.Localization.LocalizationManager.RemoveDiacritics(PDC.Localization.LocalizationManager.LocalizeText(nameGlobal));

            var name = PDC.Localization.LocalizationManager.GetLocalizedText(_nodeData.Character.characterName[0].LanguageGenericType);
            name = PDC.Localization.LocalizationManager.RemoveDiacritics(PDC.Localization.LocalizationManager.LocalizeText(name));

            // Gloval Value Multiline
            if (dialogueUIManager.showSeparateName && dialogueUIManager.nameTextBox != null && _nodeData.Character != null && _nodeData.Character.UseGlobalValue) { dialogueUIManager.ResetText(""); dialogueUIManager.nameTextBox.text = nameGlobal; }
            // Normal Multiline
            else if (dialogueUIManager.showSeparateName && dialogueUIManager.nameTextBox != null && _nodeData.Character != null) { dialogueUIManager.ResetText(""); dialogueUIManager.nameTextBox.text = name; }
            // No Change Character Multiline
            else if (dialogueUIManager.showSeparateName && dialogueUIManager.nameTextBox != null && _nodeData.Character != null) { dialogueUIManager.ResetText(""); }
            // Global Value Inline
            else if (_nodeData.Character != null && _nodeData.Character.UseGlobalValue) dialogueUIManager.ResetText(nameGlobal);
            // Normal Inline
            else if (_nodeData.Character != null) dialogueUIManager.ResetText(name);
            // Last Change
            else dialogueUIManager.ResetText("");

            var t = PDC.Localization.LocalizationManager.GetLocalizedText(_nodeData.TextType[0].LanguageGenericType);
            //[#VALUE]
            t = PDC.Localization.LocalizationManager.LocalizeText(t);

            dialogueUIManager.SetFullText(t);

            // Character Avatar
            dialogueUIManager.SpriteLeft.SetActive(false); dialogueUIManager.SpriteRight.SetActive(false);
            if (_nodeData.AvatarPos == AvatarPosition.Left && _nodeData.Character != null) { dialogueUIManager.SpriteLeft.SetActive(true); dialogueUIManager.SpriteLeft.GetComponent<Image>().sprite = _nodeData.Character.GetAvatar(_nodeData.AvatarPos, _nodeData.AvatarType); }
            if(_nodeData.AvatarPos == AvatarPosition.Right && _nodeData.Character != null) { dialogueUIManager.SpriteRight.SetActive(true); dialogueUIManager.SpriteRight.GetComponent<Image>().sprite = _nodeData.Character.GetAvatar(_nodeData.AvatarPos, _nodeData.AvatarType); }

            dialogueUIManager.SkipButton.SetActive(true);
            MakeButtons(new List<DialogueNodePort>());

            _nodeChoiceInvoke = _nodeData;

            IEnumerator tmp() { yield return new WaitForSeconds(_nodeData.Duration); ChoiceNode_GenerateChoice(); }
            StartCoroutine(tmp());

            if (_nodeData.AudioClips.Find(clip => clip.languageEnum == localizationManager.SelectedLang()).LanguageGenericType != null)
            {
                if (audioSource.isPlaying) audioSource.Pause();
                audioSource.PlayOneShot(_nodeData.AudioClips.Find(clip => clip.languageEnum == localizationManager.SelectedLang()).LanguageGenericType);
            }

            string _audioName = _nodeData.AudioName.Find(clip => clip.languageEnum == localizationManager.SelectedLang()).LanguageGenericType;
            if (!_audioName.Equals(string.Empty))
            {
                soundID = AkSoundEngine.PostEvent(_audioName, GameManager.Instance.AudioDialogueGameObject);
                GameManager.Instance.DialogueManager.OnNode += StopAudio;
                GameManager.Instance.DialogueManager.EndDialogueEvent.AddListener(StopAudio);
            }
        }
        private void RunNode(EventNodeData _nodeData)
        {
            foreach (var item in _nodeData.EventScriptableObjects)
            {
                if (item.DialogueEventSO != null)
                {
                    item.DialogueEventSO.RunEvent();
                }
            }
            CheckNodeType(GetNextNode(_nodeData));
        }
        private void RunNode(EndNodeData _nodeData)
        {
            switch (_nodeData.EndNodeType)
            {
                case EndNodeType.End:
                    dialogueUIManager.dialogueCanvas.SetActive(false);
                    isDialogueInProcess = false;
                    EndDialogueEvent.Invoke();
                    break;
                case EndNodeType.Repeat:
                    CheckNodeType(GetNodeByGuid(currentDialogueNodeData.NodeGuid));
                    break;
                case EndNodeType.GoBack:
                    CheckNodeType(GetNodeByGuid(lastDialogueNodeData.NodeGuid));
                    break;
                case EndNodeType.ReturnToStart:
                    CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[Random.Range(0,dialogueContainer.StartNodeDatas.Count)]));
                    break;
                case EndNodeType.StartDialogue:
                    StartDialogue(_nodeData.Dialogue);
                    break;
                default:
                    break;
            }
        }
        private void RunNode(TimerChoiceNodeData _nodeData)
        {
            lastDialogueNodeData = currentDialogueNodeData;
            currentDialogueNodeData = _nodeData;
            OnNode?.Invoke(currentDialogueNodeData);

            GlobalValueManager manager = Resources.Load<GlobalValueManager>("GlobalValue");
            manager.LoadFile();

            var nameGlobal = PDC.Localization.LocalizationManager.GetLocalizedText(_nodeData.Character.CustomizedName.ValueName);
            nameGlobal = PDC.Localization.LocalizationManager.RemoveDiacritics(PDC.Localization.LocalizationManager.LocalizeText(nameGlobal));

            var name = PDC.Localization.LocalizationManager.GetLocalizedText(_nodeData.Character.characterName[0].LanguageGenericType);
            name = PDC.Localization.LocalizationManager.RemoveDiacritics(PDC.Localization.LocalizationManager.LocalizeText(name));

            // Gloval Value Multiline
            if (dialogueUIManager.showSeparateName && dialogueUIManager.nameTextBox != null && _nodeData.Character != null && _nodeData.Character.UseGlobalValue) { dialogueUIManager.ResetText(""); dialogueUIManager.nameTextBox.text = nameGlobal; }
            // Normal Multiline
            else if (dialogueUIManager.showSeparateName && dialogueUIManager.nameTextBox != null && _nodeData.Character != null) { dialogueUIManager.ResetText(""); dialogueUIManager.nameTextBox.text = name; }
            // No Change Character Multiline
            else if (dialogueUIManager.showSeparateName && dialogueUIManager.nameTextBox != null && _nodeData.Character != null) { dialogueUIManager.ResetText(""); }
            // Global Value Inline
            else if (_nodeData.Character != null && _nodeData.Character.UseGlobalValue) dialogueUIManager.ResetText(nameGlobal);
            // Normal Inline
            else if (_nodeData.Character != null) dialogueUIManager.ResetText(name);
            // Last Change
            else dialogueUIManager.ResetText("");

            var t = PDC.Localization.LocalizationManager.GetLocalizedText(_nodeData.TextType[0].LanguageGenericType);
            //[#VALUE]
            t = PDC.Localization.LocalizationManager.LocalizeText(t);

            dialogueUIManager.SetFullText(t);

            // Character Avatar
            dialogueUIManager.SpriteLeft.SetActive(false); dialogueUIManager.SpriteRight.SetActive(false);
            if (_nodeData.AvatarPos == AvatarPosition.Left && _nodeData.Character != null) { dialogueUIManager.SpriteLeft.SetActive(true); dialogueUIManager.SpriteLeft.GetComponent<Image>().sprite = _nodeData.Character.GetAvatar(_nodeData.AvatarPos, _nodeData.AvatarType); }
            if (_nodeData.AvatarPos == AvatarPosition.Right && _nodeData.Character != null) { dialogueUIManager.SpriteRight.SetActive(true); dialogueUIManager.SpriteRight.GetComponent<Image>().sprite = _nodeData.Character.GetAvatar(_nodeData.AvatarPos, _nodeData.AvatarType); }

            dialogueUIManager.SkipButton.SetActive(true);
            MakeButtons(new List<DialogueNodePort>());

            _nodeTimerInvoke = _nodeData;

            IEnumerator tmp() { yield return new WaitForSecondsRealtime(_nodeData.Duration); TimerNode_GenerateChoice(); }
            StartCoroutine(tmp());

            if (_nodeData.AudioClips.Find(clip => clip.languageEnum == localizationManager.SelectedLang()).LanguageGenericType != null)
            {
                if (audioSource.isPlaying) audioSource.Pause();
                audioSource.PlayOneShot(_nodeData.AudioClips.Find(clip => clip.languageEnum == localizationManager.SelectedLang()).LanguageGenericType);
            }

            string _audioName = _nodeData.AudioName.Find(clip => clip.languageEnum == localizationManager.SelectedLang()).LanguageGenericType;
            if (!_audioName.Equals(string.Empty))
            {
                soundID = AkSoundEngine.PostEvent(_audioName, GameManager.Instance.AudioDialogueGameObject);
                GameManager.Instance.DialogueManager.OnNode += StopAudio;
                GameManager.Instance.DialogueManager.EndDialogueEvent.AddListener(StopAudio);
            }
        }

        private void StopAudio()
        {
            AkSoundEngine.StopPlayingID(soundID);
            GameManager.Instance.DialogueManager.OnNode -= StopAudio;
            GameManager.Instance.DialogueManager.EndDialogueEvent.RemoveListener(StopAudio);
        }
        private void StopAudio(BaseNodeData node)
        {
            AkSoundEngine.StopPlayingID(soundID);
            GameManager.Instance.DialogueManager.OnNode -= StopAudio;
            GameManager.Instance.DialogueManager.EndDialogueEvent.RemoveListener(StopAudio);
        }

        private void MakeButtons(List<DialogueNodePort> _nodePorts)
        {
            List<string> texts = new List<string>();
            List<UnityAction> unityActions = new List<UnityAction>();

            foreach (DialogueNodePort nodePort in _nodePorts)
            {
                var t = PDC.Localization.LocalizationManager.GetLocalizedText(nodePort.TextLanguage[0].LanguageGenericType);
                //[#VALUE]
                t = PDC.Localization.LocalizationManager.LocalizeText(t);

                texts.Add(t);
                UnityAction tempAction = null;
                tempAction += () =>
                {
                    CheckNodeType(GetNodeByGuid(nodePort.InputGuid));
                };
                unityActions.Add(tempAction);
            }

            dialogueUIManager.SetButtons(texts, unityActions, false);
        }
        private void MakeTimerButtons(List<DialogueNodePort> _nodePorts, float ShowDuration, float timer)
        {
            List<string> texts = new List<string>();
            List<UnityAction> unityActions = new List<UnityAction>();

            IEnumerator tmp() { yield return new WaitForSeconds(timer); TimerNode_NextNode(); }
            StartCoroutine(tmp());

            foreach (DialogueNodePort nodePort in _nodePorts)
            {
                if (nodePort != _nodePorts[0])
                {
                    var t = PDC.Localization.LocalizationManager.GetLocalizedText(nodePort.TextLanguage[0].LanguageGenericType);
                    //[#VALUE]
                    t = PDC.Localization.LocalizationManager.LocalizeText(t);

                    texts.Add(t);
                    UnityAction tempAction = null;
                    tempAction += () =>
                    {
                        StopAllCoroutines();
                        CheckNodeType(GetNodeByGuid(nodePort.InputGuid));
                    };
                    unityActions.Add(tempAction);
                }
            }

            dialogueUIManager.SetButtons(texts, unityActions, true);
            dialogueUIManager.TimerSlider.maxValue = timer; Timer = timer;
        }

        void DialogueNode_NextNode() { CheckNodeType(GetNextNode(_nodeDialogueInvoke)); }
        void ChoiceNode_GenerateChoice() { MakeButtons(_nodeChoiceInvoke.DialogueNodePorts);
            dialogueUIManager.SkipButton.SetActive(false);
        }
        void TimerNode_GenerateChoice() { MakeTimerButtons(_nodeTimerInvoke.DialogueNodePorts, _nodeTimerInvoke.Duration, _nodeTimerInvoke.time);
            dialogueUIManager.SkipButton.SetActive(false);
        }
        void TimerNode_NextNode() { CheckNodeType(GetNextNode(_nodeTimerInvoke)); }

        public void SkipDialogue()
        {
            StopAllCoroutines();

            switch (currentDialogueNodeData)
            {
                case DialogueNodeData nodeData:
                    DialogueNode_NextNode();
                    break;
                case DialogueChoiceNodeData nodeData:
                    ChoiceNode_GenerateChoice();
                    break;
                case TimerChoiceNodeData nodeData:
                    TimerNode_GenerateChoice();
                    break;
                default:
                    break;
            }
        }

        public void ForceEndDialog()
        {
            dialogueUIManager.dialogueCanvas.SetActive(false);
            isDialogueInProcess = false;
            EndDialogueEvent.Invoke();
        }
    }
}
