using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etienne
{
	using Pools;
	public class PlayingSound
	{
		private event Action onComplete;

		public Sound Sound;
		public AudioSource AudioSource;
		
		bool isplaying;

		public PlayingSound(Sound sound, AudioSource audioSource)
		{
			Sound = sound;
			AudioSource = audioSource;
			WaitForCompletion();
			onComplete += ReQueue;
		}

		private void ReQueue()
		{
			AudioSourcePool.Instance.Enqueue(AudioSource);
		}

		public PlayingSound(Sound sound, AudioSource audioSource, Action onComplete) : this(sound, audioSource)
		{
			OnComplete(onComplete);
		}

		public PlayingSound OnComplete(Action onComplete)
		{
			this.onComplete += onComplete;
			return this;
		}

		public PlayingSound Kill()
		{
			return this;
		}
		
		private async void WaitForCompletion()
		{
			isplaying=true;
			await System.Threading.Tasks.Task.Delay((int)(Sound.Clip.length * 1010));
			if (!Application.isPlaying || AudioSource == null || !isplaying) return;
		}
		
		void Complete(){
			onComplete?.Invoke();
			isplaying=false;
		}
	}
}
