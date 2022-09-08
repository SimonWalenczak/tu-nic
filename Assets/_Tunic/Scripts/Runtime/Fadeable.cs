using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tunic
{
    public class Fadeable : MonoBehaviour
	{
		CanvasGroup group;
		
		protected void Awake()
		{
			group	= GetComponent<CanvasGroup>();
			group.alpha=0f;
		}
    	
	    public void Fade(bool fadein)
	    {
	    	if (fadein)FadeIn();
	    	else FadeOut();
	    }
	    
	    public void FadeIn()
		{
	    	group.DOFade(1f,.2f);
	    }
	    
	    public void FadeOut()
	    {
			group.DOFade(0f,.2f);
	    }
    }
}
