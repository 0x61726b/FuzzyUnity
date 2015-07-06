using Assets.Scripts.Framework.Services.AudioService.Dictionary;

namespace Assets.Scripts.Framework.Services.AudioService {
    internal interface Audio : Provider {
        void PlayEffect( Effect effect );
    }
}