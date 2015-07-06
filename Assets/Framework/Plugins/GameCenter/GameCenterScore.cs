using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Prime31;


#if UNITY_IPHONE || UNITY_STANDALONE_OSX
public class GameCenterScore
{
	public string category;
	public string formattedValue;
	public long value;
	public UInt64 context;
	public long rawDate;
	public DateTime date
	{
		get
		{
			var intermediate = new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );
			return intermediate.AddSeconds( rawDate );
		}
	}
	public string playerId;
	public int rank;
	public bool isFriend;
	public string alias;
	public int maxRange; // this is only properly set when retrieving all scores without limiting by playerId


	public GameCenterScore()
	{}


	public GameCenterScore( Dictionary<string,object> dict )
	{
		if( dict.ContainsKey( "category" ) )
			category = dict["category"] as string;

		if( dict.ContainsKey( "formattedValue" ) )
			formattedValue = dict["formattedValue"] as string;

		if( dict.ContainsKey( "value" ) )
			value = Int64.Parse( dict["value"].ToString() );

		if( dict.ContainsKey( "context" ) )
			context = UInt64.Parse( dict["context"].ToString() );

		if( dict.ContainsKey( "playerId" ) )
			playerId = dict["playerId"] as string;

		if( dict.ContainsKey( "rank" ) )
			rank = int.Parse( dict["rank"].ToString() );

		if( dict.ContainsKey( "isFriend" ) )
			isFriend = (bool)dict["isFriend"];

		if( dict.ContainsKey( "alias" ) )
			alias = dict["alias"] as string;
		else
			alias = "Anonymous";

		if( dict.ContainsKey( "maxRange" ) )
			maxRange = int.Parse( dict["maxRange"].ToString() );

		// grab and convert the date
		if( dict.ContainsKey( "date" ) )
			rawDate = long.Parse( dict["date"].ToString() );
	}


	public override string ToString()
	{
		 return string.Format( "<Score> category: {0}, formattedValue: {1}, date: {2}, rank: {3}, alias: {4}, maxRange: {5}, value: {6}, context: {7}",
			category, formattedValue, date, rank, alias, maxRange, value, context );
	}

}
#endif