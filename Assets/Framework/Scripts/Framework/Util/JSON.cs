using Newtonsoft.Json;

namespace Assets.Scripts.Framework.Util {
    public static class Json {
        public static string Serialize( object anObject ) {
            return JsonConvert.SerializeObject( anObject );
        }

        public static T Deserialize<T>( string jsonString ) {
            return JsonConvert.DeserializeObject<T>( jsonString );
        }
    }
}