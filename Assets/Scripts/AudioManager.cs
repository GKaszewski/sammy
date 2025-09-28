using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
        [SerializeField] private List<Sound> sounds;

        public static AudioManager Instance;

        private void Awake() {
                if (!Instance) Instance = this;
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
        
        public void Play(string audioFileName) {
                var sound = sounds.Find(sound => sound.name == audioFileName);
                sound?.source.Play();
        }
}