#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Framework.Services.Store.AndroidStoreAdapter.Requests {
    internal class QueryInventory : IAndroidStoreRequest {
        private readonly Action<List<StoreItem>> _onItemsLoaded;
        private readonly Action<List<StoreItem>> _onPreviousPurchase;
        private readonly Action _onFailure;

        public QueryInventory( Action<List<StoreItem>> itemLoaded, Action<List<StoreItem>> previousPurchase, Action failure ) {
            _onItemsLoaded = itemLoaded;
            _onPreviousPurchase = previousPurchase;
            _onFailure = failure;
        }

        public void Execute() {
            GoogleIABManager.queryInventorySucceededEvent += OnQuerySuccessful;
            GoogleIABManager.queryInventoryFailedEvent += OnQueryFailed;
            GoogleIAB.queryInventory( StoreItem.GetAllProductIds() );
        }

        public void Destroy() {
            GoogleIABManager.queryInventorySucceededEvent -= OnQuerySuccessful;
            GoogleIABManager.queryInventoryFailedEvent -= OnQueryFailed;
        }

        private void OnQuerySuccessful( List<GooglePurchase> purchases, List<GoogleSkuInfo> skus ) {
            List<StoreItem> previousPurchases = new List<StoreItem>();

            foreach( GooglePurchase purchase in purchases ) {
                if( purchase.purchaseState == GooglePurchase.GooglePurchaseState.Purchased ) {
                    foreach( GoogleSkuInfo skuInfo in skus ) {
                        if( purchase.productId == skuInfo.productId ) {
                            previousPurchases.Add( StoreItem.GetStoreItem( skuInfo ) );
                        }
                    }
                }
            }
            if( previousPurchases.Count > 0 ) {
                _onPreviousPurchase( previousPurchases );
            }

            List<StoreItem> availableItems = new List<StoreItem>();
            foreach( GoogleSkuInfo sku in skus ) {
                StoreItem newItem = StoreItem.GetStoreItem( sku );
                availableItems.Add( newItem );
            }
            availableItems.Sort( new StoreItem.StoreItemComparer() );
            _onItemsLoaded( availableItems );
        }

        private void OnQueryFailed( string error ) {
            Debug.Log( "Query Inventory Request : Store query failed with error: " + error );
            _onFailure();
        }
    }
}
#endif