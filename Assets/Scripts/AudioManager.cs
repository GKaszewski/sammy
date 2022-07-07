using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
        public List<Sound> sounds;

        public static AudioManager instance;

        private void Awake() {
                if (!instance) instance = this;
                else {
                        Destroy(gameObject);
                        return;
                }
                
                DontDestroyOnLoad(gameObject);
                
                foreach (var sound in sounds) {
                        sound.source = gameObject.AddComponent<AudioSource>();
                        sound.source.clip = sound.clip;
                        sound.source.volume = sound.volume;
                        sound.source.pitch = sound.pitch;
                        sound.source.loop = sound.loop;
                }
        }

        private void Start() {
        }
        
        public void Play(string name) {
                var sound = sounds.Find(sound => sound.name == name);
                sound?.source.Play();
        }
}