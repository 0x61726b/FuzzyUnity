#if UNITY_ANDROID
using System;
using UnityEngine;

namespace Assets.Scripts.Framework.Services.Store.AndroidStoreAdapter.Requests {
    internal class Purchase : IAndroidStoreRequest {
        private readonly Action<StoreItemType> _successCallback;
        private readonly Action _failureCallback;
        private readonly StoreItemType _itemToPurchase;

        public Purchase( StoreItemType item, Action<StoreItemType> success, Action fail ) {
            _successCallback = success;
            _failureCallback = fail;
            _itemToPurchase = item;
        }

        public void Execute() {
            GoogleIABManager.purchaseSucceededEvent += onSuccess;
            GoogleIABManager.purchaseFailedEvent += onFail;
            GoogleIAB.purchaseProduct( StoreItem.GetProductId( _itemToPurchase ) );
        }

        public void Destroy() {
            GoogleIABManager.purchaseSucceededEvent -= onSuccess;
            GoogleIABManager.purchaseFailedEvent -= onFail;
        }

        private void onSuccess( GooglePurchase purchase ) {
            _successCallback( _itemToPurchase );
        }

        private void onFail( string error, int response ) {
            Debug.Log( "Purchase Request : Failed: " + error + ", Response: " + response );
            _failureCallback();
        }
    }
}
#endif