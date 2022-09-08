using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tunic
{
	public class DialogueManager : DS.Engine.DialogManager
	{
		[SerializeField] Fadeable canvas;
	    private void OnEnable()
	    {
	    	canvas.FadeIn();
	    }
	    
		private void OnDisable()
		{
			canvas.FadeOut();
		}
    }
}
