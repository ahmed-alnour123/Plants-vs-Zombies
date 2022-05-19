using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager instance;

    public AudioSource soundTemplate;
    public List<SoundAndClip> sounds = new List<SoundAndClip>();

    private void Awake() {
        instance = this;
    }

    public void PlaySound(Sounds sound) {
        var source = Instantiate(soundTemplate);
        source.clip = GetClip(sound);
        source.Play();
    }

    private AudioClip GetClip(Sounds sound) {
        return sounds.Find(s => s.sound == sound).source;
    }
}

[System.Serializable]
public enum Sounds {
    Gun,
    Sword,
    Bomb,
    Coin,
    PlantHit,
    ZombieInstantiate,
    ZombieHit,
    ZombieDie,
}

[System.Serializable]
public class SoundAndClip {
    public Sounds sound;
    public AudioClip source;
}