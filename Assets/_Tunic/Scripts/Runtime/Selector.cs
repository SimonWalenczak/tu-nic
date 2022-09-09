using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tunic
{
	public class Selector : MonoBehaviour
	{
		protected ISelectable selectable;
		protected void OnTriggerEnter(Collider other)
		{
			Debug.Log	("Trigger select");
			if (!other.TryGetComponent<ISelectable>(out ISelectable selectable)){
				if (!other.transform.root.TryGetComponent<ISelectable>(out selectable)) return;
			}
			if (selectable!=this.selectable) this.selectable?.UnSelect();
			Select(selectable);
		}
	    
		protected void OnTriggerExit(Collider other)
		{
			if (!other.TryGetComponent<ISelectable>(out ISelectable selectable)){
				if (!other.transform.root.TryGetComponent<ISelectable>(out selectable)) return;
			}
			if (selectable!=this.selectable)return;
			UnSelect(selectable);
		}
		
		protected virtual void UnSelect(ISelectable selectable)
		{
			selectable.UnSelect();
			this.selectable=null;
		}
		protected virtual void Select(ISelectable selectable)
		{
			selectable.Select();
			this.selectable = selectable;
		}
    }
}
