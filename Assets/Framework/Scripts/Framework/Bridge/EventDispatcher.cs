using System;
using System.Collections.Generic;

namespace Assets.Scripts.Framework.Bridge {
    internal class EventDispatcher<T> {
        private readonly Dictionary<T, List<Action<Dictionary<string, object>>>> _eventBindings = new Dictionary<T, List<Action<Dictionary<string, object>>>>();

        public void OnEvent( T anEvent, Dictionary<string, object> parameters ) {
            if (!_eventBindings.ContainsKey(anEvent)) {
                return;
            }
            List<Action<Dictionary<string, object>>> actionsToCall = new List<Action<Dictionary<string, object>>>( _eventBindings[anEvent] );
            for( int i = 0; i < actionsToCall.Count; i++ ) {
                actionsToCall[i]( parameters );
            }
        }

        public void RegisterEvent( T anEvent, Action<Dictionary<string, object>> action ) {
            if( !_eventBindings.ContainsKey( anEvent ) ) {
                _eventBindings.Add( anEvent, new List<Action<Dictionary<string, object>>>() );
            }
            if( !_eventBindings[anEvent].Contains( action ) ) {
                _eventBindings[anEvent].Add( action );
            }
        }

        public void RemoveEvent( T anEvent, Action<Dictionary<string, object>> action ) {
            if( !_eventBindings.ContainsKey( anEvent ) ) {
                return;
            }
            if( _eventBindings[anEvent].Contains( action ) ) {
                _eventBindings[anEvent].Remove( action );
            }
        }
    }
}