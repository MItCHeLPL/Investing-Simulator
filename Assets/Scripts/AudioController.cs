using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{
	public string Name;

	public AudioSource Source;

	public AudioClip Clip;

	public bool PlayOnAwake = false;

	public bool Loop = false;

	[Range(0f, 2f)]
	public float Volume = 1;
	[HideInInspector] public float DefaultVolume = 1;

	[Range(0.1f, 3f)]
	public float Pitch = 1;

	[Range(0f, 1f)]
	public float SpatialBlend = 0;
}


public class AudioController : MonoBehaviour
{
	[SerializeField] private List<Sound> _sounds;

	[HideInInspector] public bool IsMuted = false;


	private void Start()
	{
		//Set up source settings
		foreach (Sound s in _sounds)
		{
			s.Source.clip = s.Clip;

			s.Source.playOnAwake = s.PlayOnAwake;

			s.Source.loop = s.Loop;

			s.Source.volume = s.Volume;
			s.DefaultVolume = s.Volume;

			s.Source.pitch = s.Pitch;

			s.Source.spatialBlend = s.SpatialBlend;

			if (s.PlayOnAwake)
			{
				Play(s);
			}
		}

		GetSettingsFromPlayerPrefs();
	}


	public void Play(string name)
	{
		Sound s = _sounds.Find(sound => sound.Name == name); //Find source

		Play(s);
	}
	public void Play(string name, float delay)
	{
		Sound s = _sounds.Find(sound => sound.Name == name); //Find source

		Play(s, delay);
	}
	public void Play(Sound s, float delay = 0.0f)
	{
		if (s != null)
		{
			s.Source.PlayDelayed(delay);
		}
	}


	public void Stop(string name)
	{
		Sound s = _sounds.Find(sound => sound.Name == name); //Find source

		Stop(s);
	}
	public void Stop(Sound s)
	{
		if (s != null && IsPlaying(s))
		{
			s.Source.Stop();
		}
	}


	public bool IsPlaying(string name)
	{
		Sound s = _sounds.Find(sound => sound.Name == name); //Find source

		return IsPlaying(s);
	}
	public bool IsPlaying(Sound s)
	{
		if (s != null)
		{
			return s.Source.isPlaying;
		}

		return false;
	}


	public void UpdateMute()
	{
		//unmute
		if(IsMuted)
		{
			foreach (Sound s in _sounds)
			{
				s.Source.volume = s.Volume;

				if (s.PlayOnAwake)
				{
					Play(s);
				}
			}

			IsMuted = false;
			PlayerPrefs.SetInt("Audio_Muted", 0);
		}

		//mute
		else
		{
			foreach (Sound s in _sounds)
			{
				s.Source.volume = 0;
			}

			IsMuted = true;
			PlayerPrefs.SetInt("Audio_Muted", 1);
		}
	}
	public void UpdateMute(bool value)
	{
		//unmute
		if (!value)
		{
			foreach (Sound s in _sounds)
			{
				s.Source.volume = s.Volume;

				if (s.PlayOnAwake)
				{
					Play(s);
				}
			}

			IsMuted = false;
			PlayerPrefs.SetInt("Audio_Muted", 0);
		}

		//mute
		else
		{
			foreach (Sound s in _sounds)
			{
				s.Source.volume = 0;
			}

			IsMuted = true;
			PlayerPrefs.SetInt("Audio_Muted", 1);
		}
	}

	public void GetSettingsFromPlayerPrefs()
	{
		if (PlayerPrefs.HasKey("Audio_Muted"))
		{
			int keyValue = PlayerPrefs.GetInt("Audio_Muted"); //Get volume value from user settings
			bool boolValue = keyValue == 1;

			UpdateMute(boolValue);
		}
	}
	public bool GetSettingFromPlayerPrefs(string key)
	{
		bool boolValue = false;

		if (PlayerPrefs.HasKey(key))
		{
			int keyValue = PlayerPrefs.GetInt(key); //Get volume value from user settings
			boolValue = keyValue == 1;
		}

		return boolValue;
	}
}