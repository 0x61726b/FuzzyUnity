using Assets.Scripts.Framework.Services.AudioService.Dictionary;

namespace Assets.Scripts.Framework.Services.AudioService.SFX {
    internal class EffectController {
        private readonly EffectPlayer _player;
        private bool _isSfxOn = true;

        public EffectController() {
            _player = EffectPlayer.NewPlayer();
			_isSfxOn = ServiceLocator.GetSettings().GetCurrentSFXState();
			ServiceLocator.GetSettings().AddSFXStateListener(OnSfxSettingsChanged );
        }

        public void PlayEffect( Effect effect ) {
            if( _isSfxOn ) {
                _player.PlayEffect( effect );
            }
        }

		private void OnSfxSettingsChanged(bool status) {
			_isSfxOn = status;
        }

        public void Destroy() {
			ServiceLocator.GetSettings().RemoveSFXStateListener(OnSfxSettingsChanged );
        }
    }
}