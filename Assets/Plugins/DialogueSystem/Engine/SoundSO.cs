using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etienne {
	[CreateAssetMenu(menuName ="Etienne/Audio/Sound",order =220)]
    public class SoundSO : ScriptableObject {
        public Sound Sound => sound;
        [SerializeField] private Sound sound;
    }
}