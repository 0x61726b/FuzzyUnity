using UnityEngine;
using System.Collections.Generic;


#if UNITY_IPHONE || UNITY_ANDROID
public class UpsightReward
{
	public string name { get; private set; }
	public int quantity { get; private set; }
	public string receipt { get; private set; }


	public static UpsightReward rewardFromJson( string json )
	{
		var reward = new UpsightReward();

		var dict = MiniJSON.Json.Deserialize( json ) as Dictionary<string,object>;
		if( dict != null )
		{
			if( dict.ContainsKey( "name" ) )
				reward.name = dict["name"].ToString();

			if( dict.ContainsKey( "quantity" ) )
				reward.quantity = int.Parse( dict["quantity"].ToString() );

			if( dict.ContainsKey( "receipt" ) )
				reward.receipt = dict["receipt"].ToString();
		}

		return reward;
	}


	public override string ToString()
	{
		return string.Format( "[UpsightReward: name={0}, quantity={1}, receipt={2}]", name, quantity, receipt );
	}
}
#endif