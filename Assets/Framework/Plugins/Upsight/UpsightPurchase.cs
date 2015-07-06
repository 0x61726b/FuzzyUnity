using UnityEngine;
using System.Collections.Generic;


#if UNITY_IPHONE || UNITY_ANDROID
public class UpsightPurchase
{
	public string placement { get; private set; }
	public int quantity { get; private set; }
	public string productIdentifier { get; private set; }

	// the following properties are Android only
	public string store { get; private set; }
	public string receipt { get; private set; }
	public string title { get; private set; }
	public double price { get; private set; }


	public static UpsightPurchase purchaseFromJson( string json )
	{
		var purchase = new UpsightPurchase();

		var dict = MiniJSON.Json.Deserialize( json ) as Dictionary<string,object>;
		if( dict != null )
		{
			if( dict.ContainsKey( "placement" ) && dict["placement"] != null )
				purchase.placement = dict["placement"].ToString();

			if( dict.ContainsKey( "quantity" ) && dict["quantity"] != null )
				purchase.quantity = int.Parse( dict["quantity"].ToString() );

			if( dict.ContainsKey( "productIdentifier" ) && dict["productIdentifier"] != null )
				purchase.productIdentifier = dict["productIdentifier"].ToString();

			if( dict.ContainsKey( "store" ) && dict["store"] != null )
				purchase.store = dict["store"].ToString();

			if( dict.ContainsKey( "receipt" ) && dict["receipt"] != null )
				purchase.receipt = dict["receipt"].ToString();

			if( dict.ContainsKey( "title" ) && dict["title"] != null )
				purchase.title = dict["title"].ToString();

			if( dict.ContainsKey( "price" ) && dict["price"] != null )
				purchase.price = double.Parse( dict["price"].ToString() );
		}

		return purchase;
	}


	public override string ToString()
	{
		return string.Format( "[UpsightPurchase: placement={0}, quantity={1}, productIdentifier={2}, store={3}, receipt={4}, title={5}, price={6}]", placement, quantity, productIdentifier, store, receipt, title, price );
	}
}
#endif