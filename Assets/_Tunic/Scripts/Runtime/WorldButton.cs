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

		public static bool first = true;

		Planet planet;
		
		Fadeable fadeable;
		protected void Awake()
		{
			fadeable = GetComponentInChildren<Fadeable>();
		}

        private void Start()
        {
			planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<Planet>();
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

			int score = first ? 20 : 10;

			if (polititian.role == Politician.Role.War)
				planet.cursors[0].score += score;

			if (polititian.role == Politician.Role.Ecology)
				planet.cursors[1].score += score;

			if (polititian.role == Politician.Role.Progressism)
				planet.cursors[2].score += score;

			if (polititian.role == Politician.Role.Conservatism)
				planet.cursors[3].score += score;

			StartCoroutine(planet.ApplyCursors());

			first = false;
		}

        private void OnApplicationQuit()
        {
			first = true;
        }
    }
}
