using Assets.Scripts.Framework.Services.AudioService.Dictionary;
using Assets.Scripts.Framework.Services.AudioService.SFX;

namespace Assets.Scripts.Framework.Services.AudioService {
    internal class AudioProvider : Audio {
        private EffectController sfx;

        public void Awake() {}

        public void Start() {
            sfx = new EffectController();
        }

        public void Destroy() {
            sfx.Destroy();
        }

        public void PlayEffect( Effect effect ) {
			sfx.PlayEffect( effect );
        }
    }
}