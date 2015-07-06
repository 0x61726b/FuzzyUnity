using UnityEngine;

namespace Assets.Scripts.Framework.Services.AudioService.Dictionary {
    internal class AudioDict : MonoBehaviour {
        private static AudioDict instance;
        
		public AudioClip UI_GENERIC;
		public AudioClip LEVEL_START;
		public AudioClip LEVEL_FAIL;
		public AudioClip LEVEL_SUCCESS;

		public AudioClip PIECE_CONNECT;
		public AudioClip PIECE_DISCONNECT;
		public AudioClip PIECE_SLIDE;

        public static AudioClip GetEffectClip( Effect effect ) {
            switch( effect ) {
				case Effect.UI_GENERIC:
					return GetInstance().UI_GENERIC;
				case Effect.LEVEL_START:
					return GetInstance().LEVEL_START;
				case Effect.LEVEL_FAIL:
					return GetInstance().LEVEL_FAIL;
				case Effect.LEVEL_SUCCESS:
					return GetInstance().LEVEL_SUCCESS;
				case Effect.PIECE_CONNECT:
					return GetInstance().PIECE_CONNECT;
				case Effect.PIECE_DISCONNECT:
					return GetInstance().PIECE_DISCONNECT;
				case Effect.PIECE_SLIDE:
					return GetInstance().PIECE_SLIDE;
	            default:
	                Debug.LogError( "Effect not matched with a clip." );
	            	return null;
            }
        }

        private static AudioDict GetInstance() {
            if( instance != null ) {
                return instance;
            }
            GameObject dictGo = Instantiate( Resources.Load( "Services/Audio/AudioDict" ), Vector3.zero, Quaternion.identity ) as GameObject;
            dictGo.name = "AudioDictionary";
            instance = dictGo.GetComponent<AudioDict>();
            return instance;
        }
    }
}