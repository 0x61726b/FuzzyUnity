using UnityEngine;
using System.Collections;


public class GPGSnapshot
{
	#pragma warning disable 0649
	private string data;
	#pragma warning restore 0649

	public GPGSnapshotMetadata metadata;
	public bool hasDataAvailable { get { return data != null; } }
	public byte[] snapshotData { get { return data != null ? System.Convert.FromBase64String( data ) : null; } }


	public override string ToString()
	{
		return Prime31.JsonFormatter.prettyPrint( Prime31.Json.encode( this ) );
	}
}
