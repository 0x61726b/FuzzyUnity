#if UNITY_ANDROID
using System;
using UnityEngine;

namespace Assets.Scripts.Framework.Services.Store.AndroidStoreAdapter.Requests {
    internal class Init : IAndroidStoreRequest {
        private readonly Action<bool> _callback;

        public Init( Action<bool> supportedCallback ) {
            _callback = supportedCallback;
        }

        public void Execute() {
            GoogleIABManager.billingSupportedEvent += OnBillingSupported;
            GoogleIABManager.billingNotSupportedEvent += OnBillingNotSupported;
            GoogleIAB.init( StoreConfig.GOOGLE_STOREKEY );
            GoogleIAB.setAutoVerifySignatures( true );
        }

        public void Destroy() {
            GoogleIABManager.billingSupportedEvent -= OnBillingSupported;
            GoogleIABManager.billingNotSupportedEvent -= OnBillingNotSupported;
        }

        private void OnBillingSupported() {
            _callback( true );
        }

        private void OnBillingNotSupported( string error ) {
            Debug.Log( "Init Request : Billing not supported: " + error );
            _callback( false );
        }
    }
}
#endif