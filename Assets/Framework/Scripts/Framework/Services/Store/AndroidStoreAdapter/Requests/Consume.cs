#if UNITY_ANDROID
using System;
using UnityEngine;

namespace Assets.Scripts.Framework.Services.Store.AndroidStoreAdapter.Requests {
    internal class Consume : IAndroidStoreRequest {
        private readonly Action<bool> _callback;
        private readonly StoreItemType _consumeType;

        public Consume( StoreItemType itemType, Action<bool> consumeCallback ) {
            _consumeType = itemType;
            _callback = consumeCallback;
        }

        public void Execute() {
            GoogleIABManager.consumePurchaseSucceededEvent += OnConsume;
            GoogleIABManager.consumePurchaseFailedEvent += OnConsumeFailed;
            GoogleIAB.consumeProduct( StoreItem.GetProductId( _consumeType ) );
        }

        public void Destroy() {
            GoogleIABManager.consumePurchaseSucceededEvent -= OnConsume;
            GoogleIABManager.consumePurchaseFailedEvent -= OnConsumeFailed;
        }

        private void OnConsume( GooglePurchase purchase ) {
            _callback( true );
        }

        private void OnConsumeFailed( string error ) {
            Debug.Log( "Consume Request : Failed with error: " + error );
            _callback( false );
        }
    }
}
#endif