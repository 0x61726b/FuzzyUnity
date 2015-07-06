using System;
using System.Collections.Generic;

namespace Assets.Scripts.Framework.Services.Store {
    public class StoreItem {
        public class StoreItemComparer : IComparer<StoreItem> {
            public int Compare( StoreItem item1, StoreItem item2 ) {
                int item1Val = 0;
                int item2Val = 0;
                int.TryParse( item1.Title.Split( ' ' )[0], out item1Val );
                int.TryParse( item2.Title.Split( ' ' )[0], out item2Val );
                return item1Val.CompareTo( item2Val );
            }
        }

        public static Action GetFunction( StoreItemType type ) {
            if( StoreConfig.ItemFunction.ContainsKey( type ) ) {
                return StoreConfig.ItemFunction[type];
            }
            throw new Exception( "Item function not implemented." );
        }

        public static string GetProductId( StoreItemType type ) {
            foreach( KeyValuePair<string, StoreItemType> pair in StoreConfig.StoreItems ) {
                if( pair.Value == type ) {
                    return pair.Key;
                }
            }
            throw new Exception( "Item type not configured properly." );
        }

        public static string[] GetAllProductIds() {
            List<string> ids = new List<string>();
            foreach( KeyValuePair<string, StoreItemType> pair in StoreConfig.StoreItems ) {
                ids.Add( pair.Key );
            }
            return ids.ToArray();
        }

#if UNITY_ANDROID

    public static StoreItem GetStoreItem( GoogleSkuInfo androidSku ) {
        if( StoreConfig.StoreItems.ContainsKey( androidSku.productId ) ) {
            return new StoreItem( androidSku.productId, androidSku.title, androidSku.description, androidSku.price, androidSku.priceCurrencyCode );
        }
        return null;
    }

#elif UNITY_IPHONE

        public static StoreItem GetStoreItem( StoreKitProduct product ) {
            return StoreConfig.StoreItems.ContainsKey( product.productIdentifier )
                       ? new StoreItem( product.productIdentifier, product.title, product.description, product.price, product.currencyCode )
                       : null;
        }

#endif

        public StoreItemType Type { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Price { get; private set; }
        public string CurrencyCode { get; private set; }

        public StoreItem( string id, string title, string description, string price, string currencyCode ) {
            Type = StoreConfig.StoreItems[id];
            Title = title;
            Description = description;
            Price = price;
            CurrencyCode = currencyCode;
            FixItemTitle( this );
        }

        private static void FixItemTitle( StoreItem item ) {
            string[] tokens = item.Title.Split( ' ' );
            if( tokens.Length > 2 && tokens[tokens.Length - 2].Equals( "(1010!" ) && tokens[tokens.Length - 1].Equals( "World)" ) ) {
                string newTitle = "";
                for( int i = 0; i < tokens.Length - 2; i++ ) {
                    newTitle += tokens[i];
                    if( i != tokens.Length - 3 ) {
                        newTitle += " ";
                    }
                }
                item.Title = newTitle;
            }
        }
    }
}