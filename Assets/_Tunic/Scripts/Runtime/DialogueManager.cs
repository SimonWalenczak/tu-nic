using UnityEngine;
using UnityEngine.UI;

namespace Tunic
{
    public class DialogueManager : DS.Engine.DialogManager
    {
        public event System.Action OnStart, OnComplete;

	    [SerializeField] Fadeable canvas;
	    
	    
        private void OnEnable()
	    {
		    // gameObject.SetActive(true);
            canvas.FadeIn();
            OnStart?.Invoke();
            OnStart = null;
        }

        private void OnDisable()
        {
	        canvas.FadeOut();//.onComplete+=()=>gameObject.SetActive(false	);
            OnComplete?.Invoke();
            OnComplete = null;
        }

        private void OnApplicationQuit()
        {
            OnComplete = null;
            OnStart = null;
        }
    }
}
