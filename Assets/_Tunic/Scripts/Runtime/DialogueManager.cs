using UnityEngine;

namespace Tunic
{
    public class DialogueManager : DS.Engine.DialogManager
    {
        public event System.Action OnStart, OnComplete;

        [SerializeField] Fadeable canvas;
        private void OnEnable()
        {
            canvas.FadeIn();
            OnStart?.Invoke();
            OnStart = null;
        }

        private void OnDisable()
        {
            canvas.FadeOut();
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
