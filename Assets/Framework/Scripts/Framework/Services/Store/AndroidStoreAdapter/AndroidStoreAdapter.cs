#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.Services.Store.AndroidStoreAdapter.Requests;
using UnityEngine;

namespace Assets.Scripts.Framework.Services.Store.AndroidStoreAdapter {
    internal class AndroidStoreAdapter : Store {
        private List<StoreItem> _storeItems = new List<StoreItem>();
        private bool _isBillingSupported = true;

        public void Awake() {}

        public void Start() {
            Request( new Init( ( bool supported ) => {
                                   _isBillingSupported = supported;
                                   RequestComplete();
                               } ) );
            RequestInventory();
        }

        public void Destroy() {
            GoogleIAB.unbindService();
        }

        public void PurchaseItem( StoreItemType itemType, Action success, Action failure ) {
            Request( new Purchase(
                         itemType,
                         ( StoreItemType purchasedItem ) => {
                             PurchaseSuccess( purchasedItem );
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

        public List<StoreItem> GetStoreItems() {
            return _storeItems;
        }

        private void ConsumeItem( StoreItemType itemType ) {
            Request( new Consume(
                         itemType,
                         ( bool res ) => {
                             if( res ) {
                                 Debug.Log( "item successfully consumed: " + itemType );
                             } else {
                                 Debug.Log( "item consume FAILED: " + itemType );
                             }
                             RequestComplete();
                         }
                         )
                );
        }

        private void PurchaseSuccess( StoreItemType purchase ) {
            StoreItem.GetFunction( purchase )();
            ConsumeItem( purchase );
        }

        private void RequestInventory() {
            Request( new QueryInventory( ( List<StoreItem> itemLoaded ) => {
                                             _storeItems = itemLoaded;
                                             RequestComplete();
                                         }, ( List<StoreItem> purchases ) => {
                                                foreach( StoreItem purchase in purchases ) {
                                                    PurchaseSuccess( purchase.Type );
                                                }
                                            }, () => {
                                                   _isBillingSupported = false;
                                                   RequestComplete();
                                               } ) );
        }

        /////////////////////////////////////////////////
        // Request Queue
        /////////////////////////////////////////////////
        private readonly Queue<IAndroidStoreRequest> requestQueue = new Queue<IAndroidStoreRequest>();
        private IAndroidStoreRequest currentRequest;

        private void Request( IAndroidStoreRequest request ) {
            requestQueue.Enqueue( request );
            TriggerExecute();
        }

        private void TriggerExecute() {
            if( currentRequest == null && _isBillingSupported && requestQueue.Count != 0 ) {
                currentRequest = requestQueue.Dequeue();
                currentRequest.Execute();
            }
        }

        private void RequestComplete() {
            currentRequest.Destroy();
            currentRequest = null;
            TriggerExecute();
        }
    }
}
#endif