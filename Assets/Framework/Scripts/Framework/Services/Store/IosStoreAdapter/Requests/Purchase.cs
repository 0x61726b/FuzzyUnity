#if UNITY_IPHONE
using System;
using UnityEngine;

namespace Assets.Scripts.Framework.Services.Store.IosStoreAdapter.Requests {
    internal class Purchase : IIosStoreRequest {
        private readonly Action _failureCallback;
        private readonly StoreItemType _itemToPurchase;
        private readonly Action<StoreItemType, string> _successCallback;

        public Purchase( StoreItemType item, Action<StoreItemType, string> success, Action fail ) {
            _successCallback = success;
            _failureCallback = fail;
            _itemToPurchase = item;
        }

        public void Execute() {
            StoreKitManager.purchaseSuccessfulEvent += OnSuccess;
            StoreKitManager.purchaseCancelledEvent += OnFail;
            StoreKitManager.purchaseFailedEvent += OnFail;
            StoreKitBinding.purchaseProduct( StoreItem.GetProductId( _itemToPurchase ), 1 );
        }

        public void Destroy() {
            StoreKitManager.purchaseSuccessfulEvent -= OnSuccess;
            StoreKitManager.purchaseCancelledEvent -= OnFail;
            StoreKitManager.purchaseFailedEvent -= OnFail;
        }

        private void OnSuccess( StoreKitTransaction transaction ) {
            _successCallback( _itemToPurchase, transaction.transactionIdentifier );
        }

        private void OnFail( string error ) {
            Debug.Log( "Purchase Request : Failed: " + error );
            _failureCallback();
        }
    }
}

#endif