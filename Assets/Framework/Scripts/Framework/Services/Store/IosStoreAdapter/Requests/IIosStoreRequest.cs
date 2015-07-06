
#if UNITY_IPHONE

namespace Assets.Scripts.Framework.Services.Store.IosStoreAdapter.Requests {
    internal interface IIosStoreRequest {
        void Destroy();
        void Execute();
    }
}

#endif