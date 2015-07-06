#if UNITY_IPHONE
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Framework.Services.Store.IosStoreAdapter.Requests {
    internal class QueryInventory : IIosStoreRequest {
        private readonly Action<List<StoreItem>> _onItemsLoaded;

        public QueryInventory( Action<List<StoreItem>> itemLoaded ) {
            _onItemsLoaded = itemLoaded;
        }

        public void Execute() {
            StoreKitManager.productListReceivedEvent += OnQuerySuccessful;
            StoreKitBinding.requestProductData( StoreItem.GetAllProductIds() );
        }

        public void Destroy() {
            StoreKitManager.productListReceivedEvent -= OnQuerySuccessful;
        }

        private void OnQuerySuccessful( List<StoreKitProduct> productList ) {
            List<StoreItem> availableItems = new List<StoreItem>();
            foreach( StoreKitProduct sku in productList ) {
                availableItems.Add( StoreItem.GetStoreItem( sku ) );
            }
            availableItems.Sort( new StoreItem.StoreItemComparer() );
            _onItemsLoaded( availableItems );
        }
    }
}

#endif