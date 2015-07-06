using System.Collections;
using Assets.Scripts.Framework.Services.AudioService.Dictionary;
using UnityEngine;

namespace Assets.Scripts.Framework.Services.AudioService.SFX {
    internal class EffectPlayer : MonoBehaviour {
        public static EffectPlayer NewPlayer() {
            GameObject gameObject = new GameObject( "EffectPlayer" );
            EffectPlayer newPlayer = gameObject.AddComponent<EffectPlayer>();
            return newPlayer;
        }

        public void PlayEffect( Effect effect ) {
            AudioClip clip = AudioDict.GetEffectClip( effect );
            if( clip != null ) {
                StartCoroutine( PlayAndDestroyRoutine( clip ) );
            }
        }

        private IEnumerator PlayAndDestroyRoutine( AudioClip clip ) {
            AudioSource audio = gameObject.AddComponent<AudioSource>();
            audio.clip = clip;
            audio.dopplerLevel = 0;
            audio.spatialBlend = 0;
            audio.spread = 360;
            audio.minDistance = 0;
            audio.maxDistance = 1000;
            audio.rolloffMode = AudioRolloffMode.Custom;
            audio.Play();
            do {
                yield return null;
            } while( audio.isPlaying );
            Destroy( audio );
        }
    }
}