using UnityEngine;
using System.Collections;


public class GPGSnapshotMetadata
{
	public double lastModifiedTimestamp;
	public string description;
	public string name;


	public override string ToString()
	{
		return Prime31.JsonFormatter.prettyPrint( Prime31.Json.encode( this ) );
	}
}
