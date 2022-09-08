using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tunic
{
	public class Politician : MonoBehaviour, ISelectable
	{
		public DS.Engine.DSContainerSO Container => container;
		[SerializeField] DS.Engine.DSContainerSO container;
		Fadeable fadeable;
		protected void Awake()
		{
			fadeable = GetComponentInChildren<Fadeable>();
		}
	    
		void ISelectable.Select(){
			fadeable.FadeIn();
		}
	    
		void ISelectable.UnSelect(){
			fadeable.FadeOut();
		}
    }
}
