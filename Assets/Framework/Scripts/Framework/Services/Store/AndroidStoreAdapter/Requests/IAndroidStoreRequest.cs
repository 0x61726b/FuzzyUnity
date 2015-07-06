#if UNITY_ANDROID
namespace Assets.Scripts.Framework.Services.Store.AndroidStoreAdapter.Requests {
    internal interface IAndroidStoreRequest {
        void Destroy();
        void Execute();
    }
}
#endif