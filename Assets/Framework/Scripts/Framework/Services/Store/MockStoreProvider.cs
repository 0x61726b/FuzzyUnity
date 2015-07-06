using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Framework.Services.Store {
    internal class MockStoreProvider : Store {
        private readonly List<StoreItem> _items = new List<StoreItem> {
			new StoreItem( "protein_life_fill", "Fill Life (Protein)", "Fill life to maximum.", "2.71", "TL" )
        };

        public void Awake() {}

        public void Start() {
            _items.Sort( new StoreItem.StoreItemComparer() );
        }

        public void Destroy() {}

        public List<StoreItem> GetStoreItems() {
            return _items;
        }

        public void PurchaseItem( StoreItemType itemType, Action success, Action failure ) {
            Debug.Log( "MOCK PurchaseItem " + itemType );

            double price = Mathf.Round( getPrice( itemType )*100 );
            Debug.Log( "price value: " + price );

            switch( itemType ) {
                case StoreItemType.LIFE_FILL:
                    break;
            }

			LeanTween.delayedCall( 1.2f , success );
        }

        private static float getPrice( StoreItemType itemType ) {
            switch( itemType ) {
                case StoreItemType.LIFE_FILL:
                    return .99f;
                default:
                    return 0f;
            }
        }
    }
}