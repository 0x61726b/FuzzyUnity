
#if UNITY_IPHONE

namespace Assets.Scripts.Framework.Services.Store.IosStoreAdapter.Requests {
    internal class Consume : IIosStoreRequest {
        private readonly string _trxId;

        public Consume( string transactionId ) {
            _trxId = transactionId;
        }

        public void Execute() {
            StoreKitBinding.finishPendingTransaction( _trxId );
        }

        public void Destroy() {}
    }
}

#endif