using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tunic
{
	public class Talk : Selector
	{
		[SerializeField] InputActionReference talkInput;
		
		protected void Awake()
		{
			enabled = false;
			talkInput.action.performed += StartTalking;
		}
		
		protected void OnEnable()
		{
			talkInput.action.Enable();
		}
		
		protected void OnDisable()
		{
			talkInput.action.Disable();
		}
		
		protected override void Select(ISelectable selectable)
		{
			base.Select(selectable);
			enabled = true;
		}
		
		protected override void UnSelect(ISelectable selectable)
		{
			base.UnSelect(selectable);
			enabled = false;
		}
		
		void StartTalking(InputAction.CallbackContext c)
		{
			Debug.Log($"Start Talking to {selectable}");
			if (!(selectable is Politician politician)) return;
			var dialogue = GameObject.FindObjectOfType<DialogueManager>();
			dialogue.SetDialog(politician.Container); 
			UnSelect(selectable);
		}
		
		protected void OnApplicationQuit()
		{
			talkInput.action.performed -= StartTalking;
		}
    }
}
