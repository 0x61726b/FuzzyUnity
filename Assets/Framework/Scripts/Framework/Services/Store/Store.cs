using System;
using System.Collections.Generic;

namespace Assets.Scripts.Framework.Services.Store {
    internal interface Store : Provider {
        List<StoreItem> GetStoreItems();
        void PurchaseItem( StoreItemType itemType, Action success, Action failure );
    }
}