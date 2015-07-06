#if UNITY_IPHONE
using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.Services.Store.IosStoreAdapter.Requests;

namespace Assets.Scripts.Framework.Services.Store.IosStoreAdapter {
    internal class IosStoreAdapter : Store {
        /////////////////////////////////////////////////
        // Request Queue
        /////////////////////////////////////////////////
        private readonly Queue<IIosStoreRequest> _requestQueue = new Queue<IIosStoreRequest>();
        private IIosStoreRequest _currentRequest;
        private bool _isBillingSupported = true;
        private List<StoreItem> _storeItems = new List<StoreItem>();
        public void Awake() {}

        public void Start() {
            _isBillingSupported = StoreKitBinding.canMakePayments();
            RequestInventory();
        }

        public void Destroy() {}

        public List<StoreItem> GetStoreItems() {
            return _storeItems;
        }

        public void PurchaseItem( StoreItemType itemType, Action success, Action failure ) {
            Request( new Purchase(
                         itemType,
                         ( StoreItemType purchasedItem, string trxId ) => {
                             PurchaseSuccess( purchasedItem, trxId );
                             success();
                             RequestComplete();
                         },
                         () => {
                             failure();
                             RequestComplete();
                         }
                         )
                );
        }

        private void PurchaseSuccess( StoreItemType purchase, string transactionId ) {
            StoreItem.GetFunction( purchase )();
            Request( new Consume( transactionId ) );
            RequestComplete();
        }

        private void RequestInventory() {
            Request( new QueryInventory( ( List<StoreItem> itemLoaded ) => {
                                             _storeItems = itemLoaded;
                                             RequestComplete();
                                         } ) );
        }

        private void Request( IIosStoreRequest request ) {
            _requestQueue.Enqueue( request );
            TriggerExecute();
        }

        private void TriggerExecute() {
            if( _currentRequest == null && _isBillingSupported && _requestQueue.Count != 0 ) {
                _currentRequest = _requestQueue.Dequeue();
                _currentRequest.Execute();
            }
        }

        private void RequestComplete() {
            _currentRequest.Destroy();
            _currentRequest = null;
            TriggerExecute();
        }
    }
}
#endif