using UnityEngine;

namespace Tunic
{
    public class Politician : MonoBehaviour, ISelectable
    {
        public DS.Engine.DSContainerSO Container => container;
        public Grabbable Hat => hat;

        [SerializeField] DS.Engine.DSContainerSO container;
        [SerializeField, Etienne.ReadOnly] Grabbable hat;
        [SerializeField] GameObject button;
        [SerializeField] Transform hatParent;
        Fadeable fadeable;

        Renderer buttonRenderer;
        Color cubeColor;

        Animator animator;
        public enum Role
        {
            Conservatism,
            Progressism,
            War,
            Ecology
        }

        public Role role;

        protected void Awake()
        {
            fadeable = GetComponentInChildren<Fadeable>();
            buttonRenderer = button.GetComponent<Renderer>();
            cubeColor = buttonRenderer.material.color;

            animator = GetComponentInChildren<Animator>();
        }

        void ISelectable.Select()
        {
            fadeable.FadeIn();
        }

        void ISelectable.UnSelect()
        {
            fadeable.FadeOut();
        }

        public void SetHat(Grabbable hat)
        {
            if (hat != null)
            {
                Debug.Log("pas de chapeaux !");
            }

            this.hat = hat;

            animator.SetTrigger("haveHat");
            
            hat.SetAsHat(hatParent);
            buttonRenderer.material.color = hat.refColor;
        }

        internal void NoHat()
        {
            hat = null;
            buttonRenderer.material.color = cubeColor;
        }
    }
}
