using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Framework.Services.Store {
    public enum StoreItemType {
        LIFE_FILL
    }

    internal static class StoreConfig {
        private const string IS_FIRST_PURCHASE_KEY = "purchase_is_first_purchase";

        public static readonly Dictionary<string, StoreItemType> StoreItems = new Dictionary<string, StoreItemType> {
            {"protein_life_fill", StoreItemType.LIFE_FILL}
        };

        public static readonly Dictionary<StoreItemType, Action> ItemFunction = new Dictionary<StoreItemType, Action> {
            {
				StoreItemType.LIFE_FILL, () => {
					TrackRevenue("life_fill", StoreItemType.LIFE_FILL, 1);
				}
			}
        };

        private static void TrackRevenue( string itemName, StoreItemType itemType, int amt ) {
            int price = ( int ) Mathf.Round( GetPrice( itemType ) * 100 );
            ServiceLocator.GetUpsight().TrackEvent( "iap", "purchase", "life", itemName, price.ToString() );
            ServiceLocator.GetAnalytics().TrackRevenue( price );
            
			CheckAndFireFirstPurchase( itemName, price.ToString() );
        }

        private static void CheckAndFireFirstPurchase( string itemName, string amt ) {
            if( ServiceLocator.GetDB().GetBool( IS_FIRST_PURCHASE_KEY, false ) ) {
                return;
            }
            ServiceLocator.GetDB().SetBool( IS_FIRST_PURCHASE_KEY, true, true );
            ServiceLocator.GetUpsight().TrackEvent( "iap", "first_purchase", "life", itemName, amt );
        }

        private static float GetPrice( StoreItemType itemType ) {
            switch( itemType ) {
                case StoreItemType.LIFE_FILL:
                    return .99f;
                default:
                    return 0f;
            }
        }

#if UNITY_ANDROID
		public const string GOOGLE_STOREKEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAo3crFub3Dhq812UfoMkp7x+OdR4entKsxDDOx7VbkzRqu8kkMmd/CQgkDEY9O2vOjebQ10VwvWRVyRFmJVkMiWy9FAoUtTtzNK25n8dBJcePew5e0kKwGcm4cRmXi6S2sjsB6HXFrzeT869W+OzbMGOJQoW1azT8eDkEikZrNaLVEZqfl+ddKMc6Yc01ZSEhWu93BUg2l7IJobt2m47vOB7TmC/YsJekZzoRp8n668hvlZuP6P4j4YuYddvdZPdRoRHPzWraR5X35jLpf3pnb8aAmnKBcgiCoe+VMQeP3tlolbcFrS6odDIBuf6d33a1pA3kX3IoePsJL/0njwA26QIDAQAB";
#endif
    }
}