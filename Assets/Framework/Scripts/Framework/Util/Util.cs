using System;
using System.Globalization;
using UnityEngine;

namespace Assets.Scripts.Framework.Util {
    public static class Util {
        public static readonly long ONEMINUTE = 60;
        private static readonly DateTime unixstart = new DateTime( 1970, 1, 1 );

        public static Vector3 GetPositionForGridIndex( int x, int y, float left, float top, float cubesize, float spacing ) {
            return new Vector3( left + (x*(cubesize + spacing)) + cubesize/2, top - (y*(cubesize + spacing)) - cubesize/2, 0 );
        }

        public static int GetMinForRange( int range, int center ) {
            return ( int ) Mathf.Ceil( center - (( float ) (range - 1)/2) );
        }

        public static int GetMaxForRange( int range, int center ) {
            return ( int ) Mathf.Ceil( center + (( float ) (range - 1)/2) );
        }

        public static long GetTime() {
            return ( long ) (DateTime.UtcNow.Subtract( unixstart )).TotalSeconds;
        }

        public static long GetTimestampFromDateTime( DateTime timestamp ) {
            return ( long ) (timestamp.ToUniversalTime().Subtract( unixstart )).TotalSeconds;
        }

        public static string ColorToHex( Color32 color ) {
            return color.r.ToString( "X2" ) + color.g.ToString( "X2" ) + color.b.ToString( "X2" );
        }

        public static Color HexToColor( string hex ) {
            byte r = byte.Parse( hex.Substring( 0, 2 ), NumberStyles.HexNumber );
            byte g = byte.Parse( hex.Substring( 2, 2 ), NumberStyles.HexNumber );
            byte b = byte.Parse( hex.Substring( 4, 2 ), NumberStyles.HexNumber );
            return new Color32( r, g, b, 255 );
        }

        public static Rect GetScreen( Camera camera, float depth ) {
            Vector3 bottomLeft = camera.ViewportToWorldPoint( new Vector3( 0, 0, depth ) );
            Vector3 topRight = camera.ViewportToWorldPoint( new Vector3( 1, 1, depth ) );
            return new Rect( bottomLeft.x, topRight.z, Mathf.Abs( topRight.x - bottomLeft.x ), Mathf.Abs( bottomLeft.z - topRight.z ) );
        }
    }
}