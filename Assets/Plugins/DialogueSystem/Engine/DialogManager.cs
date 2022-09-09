using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DS.Engine {
    using Data;

	public class DialogManager : MonoBehaviour {
        [Header("Container")]
        [SerializeField] private DSContainerSO container;
        [Header("Text")]
#if TMP
        [SerializeField] private TMPro.TextMeshProUGUI textDialog;
        [SerializeField] private TMPro.TextMeshProUGUI textCharacterName;
#else
    	[SerializeField] private Text textDialog;
		[SerializeField] private Text textCharacterName;
#endif
        [Header("Images")]
        [SerializeField] private Image imageCharacter;
        [SerializeField] private Image imageBackground;
        [Header("Buttons")]
        [SerializeField] private GameObject buttonParent;
	    [SerializeField] private GameObject buttonPrefab;
	    
	    private Etienne.PlayingSound playingSound;

        private Dictionary<string, DSNodeData> dialogues = new Dictionary<string, DSNodeData>();
        private DSNodeData currentDialogue;

		private void Awake() {
            if (container == null) return;
			dialogues.Clear();
            foreach(DSNodeData dialogue in container.Nodes) {
                dialogues.Add(dialogue.Id, dialogue);
            }
            currentDialogue = container.FirstNode;
        }

		public void SetDialog(DSContainerSO container){
			this.container = container;
			Awake();
            Start();
			enabled=true;
		}

        private void Start() => UpdateDialogSequence(currentDialogue);

        private void UpdateDialogSequence(int index) {
            if(index >= currentDialogue.Choices.Count) return;
            UpdateDialogSequence(dialogues[currentDialogue.Choices[index].NextDialogueID]);
        }

        public void UpdateDialogSequence(DSNodeData dialogue) {
            if (dialogue == null) return;
            UpdateTexts(dialogue);
            UpdateChoices(dialogue);
	        UpdateSprites(dialogue);
	        playingSound?.Kill();
	        if(dialogue.Sound != null) playingSound = dialogue.Sound.Sound.Play();
	        //todo stop sound
	        //if(dialogue.StopSound != null) Etienne.AudioManager.TryStopLoopingSound(dialogue.StopSound.Sound);
	        currentDialogue = dialogue;
        }
	    
		
        private void UpdateTexts(DSNodeData dialogue) {
            if(!string.IsNullOrEmpty(dialogue.Text)) {
                textDialog.transform.parent.gameObject.SetActive(true);
                textDialog.text = dialogue.Text;
                textCharacterName.transform.parent.gameObject.SetActive(true);
                textCharacterName.text = dialogue.CharacterName;
            } else {
                textDialog.transform.parent.gameObject.SetActive(false);
                textCharacterName.transform.parent.gameObject.SetActive(false);
            }
        }

        private void UpdateChoices(DSNodeData dialogue) {
            List<DSChoiceData> validChoices = new List<DSChoiceData>();
            foreach(DSChoiceData choice in dialogue.Choices) {
                if(!string.IsNullOrEmpty(choice.NextDialogueID)) validChoices.Add(choice);
            }
	        if (validChoices.Count==0)
	        {
	        	LastDialogueButton();
	        	return;
	        }
	        HandleButtons(validChoices, dialogue.Choices);
        }

		private void LastDialogueButton() {
			Button[] children = buttonParent.GetComponentsInChildren<Button>();
			foreach(Button child in children) GameObject.Destroy(child.gameObject);
			GameObject go = GameObject.Instantiate(buttonPrefab, buttonParent.transform);
			Button button = go.GetComponent<Button>();
#if TMP
			TMPro.TextMeshProUGUI text = go.GetComponentInChildren<TMPro.TextMeshProUGUI>();
#else
			Text text = go.GetComponentInChildren<Text>();
#endif
			text.text = "Quit";
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() => enabled=false);
		}
		
        private void HandleButtons(List<DSChoiceData> validChoices, List<DSChoiceData> allChoices) {

            Button[] children = buttonParent.GetComponentsInChildren<Button>();
            foreach(Button child in children) GameObject.Destroy(child.gameObject);

            List<(Button button, int index)> buttons = new List<(Button button, int index)>();
            for(int i = 0; i < validChoices.Count; i++) {
                int index = allChoices.IndexOf(validChoices[i]);
                GameObject go = GameObject.Instantiate(buttonPrefab, buttonParent.transform);
                Button button = go.GetComponent<Button>();
#if TMP
                TMPro.TextMeshProUGUI text = go.GetComponentInChildren<TMPro.TextMeshProUGUI>();
#else
                Text text = go.GetComponentInChildren<Text>();
#endif
                text.text = validChoices[i].Name;
                buttons.Add((button, index));
            }

            foreach((Button button, int index) button in buttons) {
                button.button.onClick.RemoveAllListeners();
                button.button.onClick.AddListener(() => UpdateDialogSequence(button.index));
            }
        }

        private void UpdateSprites(DSNodeData dialogue) {
            if(dialogue.CharacterSprite != null) imageCharacter.sprite = dialogue.CharacterSprite;
            if(dialogue.BackgroundSprite != null) imageBackground.sprite = dialogue.BackgroundSprite;
        }
        
        
    }
}