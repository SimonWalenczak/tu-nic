using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tunic
{
    public class WorldButton : MonoBehaviour, ISelectable
	{
		[SerializeField] Transform button;
		[SerializeField] float pressDepth=.03f;
		[SerializeField] Politician polititian;
		
		Fadeable fadeable;
		protected void Awake()
		{
			fadeable = GetComponentInChildren<Fadeable>();
		}

		void ISelectable.Select()
		{
			fadeable.FadeIn();
		}

		void ISelectable.UnSelect()
		{
			fadeable.FadeOut();
		}
		
		public void Press(){
			button.DOComplete();
			button.DOMoveY(button.position.y-pressDepth,.2f).SetLoops(2,LoopType.Yoyo);
		}
    }
}
