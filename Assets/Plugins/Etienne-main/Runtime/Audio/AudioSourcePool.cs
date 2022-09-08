using UnityEngine;
namespace Etienne.Pools
{
    public class AudioSourcePool : ComponentPool<AudioSource>
	{
		public static PlayingSound Play(Cue cue, int index, Transform transform = null)
		{
			if (index < 0 || index >= cue.Clips.Length)
			{
				Debug.LogWarning("Index out of range, played random sound instead.");
				return cue.Sound.Play(transform);
			}
			return new Sound(cue.Clips[index], cue.Parameters).Play(transform);
		}

		public static PlayingSound Play(Sound sound, Transform transform = null)
		{
			if (instance == null) CreateInstance<AudioSourcePool>(100);

			AudioSource source = instance.Dequeue();
			source.SetSoundToSource(sound);
			source.Play();
			if (transform != null)
			{
				source.transform.parent = transform;
				source.transform.localPosition = Vector3.zero;
			}
			return new PlayingSound(sound, source);
		}

		public static PlayingSound PlayLooped(Sound sound, Transform transform = null)
		{
			if (instance == null) CreateInstance<AudioSourcePool>(100);

			AudioSource source = instance.Dequeue();
			source.SetSoundToSource(sound);
			source.loop = true;
			source.Play();
			if (transform != null)
			{
				source.transform.parent = transform;
				source.transform.localPosition = Vector3.zero;
			}
			return new PlayingSound(sound, source);
		}
	}
}
