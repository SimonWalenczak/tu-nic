using UnityEngine;

namespace Tunic
{
    public class Politician : MonoBehaviour, ISelectable
    {
        public DS.Engine.DSContainerSO Container => container;
        public Grabbable Hat => hat;

        [SerializeField] DS.Engine.DSContainerSO container;
        [SerializeField, Etienne.ReadOnly] Grabbable hat;
        [SerializeField] Transform hatParent;
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

        public void SetHat(Grabbable hat)
        {
            this.hat = hat;
            hat.SetAsHat(hatParent);
        }
    }
}
