using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {
	public static SoundManager Instance;
	public AudioSource source;
bool muted = true;

	private void Awake() {
		if(Instance != null){
			Destroy(gameObject);
			return;
		}

		Instance = this;
		source = GetComponent<AudioSource>();
	}

	public void PlayMusic(){

	}

	public void PlaySfx(AudioClip sfx){
		source.clip = sfx;
		source.Play();
	}
}
