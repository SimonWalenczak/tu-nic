using UnityEngine;

namespace Tunic
{
    public class Politician : MonoBehaviour, ISelectable
    {
        
	    public DS.Engine.DSContainerSO Container {
	    	get {
	    		Debug.Log(WorldButton.lastQuestion);
	    		switch (WorldButton.lastQuestion)
	    		{
	    		case Role.Conservatism:return containerConservatism;
	    		case Role.Progressism:return containerProgressism;
	    		case Role.War:return containerWar;
	    		case Role.Ecology:return containerEcology;
	    		default:return container;
	    		}
	    	}
	    }
	    public Grabbable Hat => hat;

	    [SerializeField] DS.Engine.DSContainerSO container;
	    [SerializeField] DS.Engine.DSContainerSO containerConservatism;
	    [SerializeField] DS.Engine.DSContainerSO containerProgressism;
	    [SerializeField] DS.Engine.DSContainerSO containerWar;
	    [SerializeField] DS.Engine.DSContainerSO containerEcology;
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
