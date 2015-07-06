using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Framework.SceneAttach {
    internal class SceneAttachment : MonoBehaviour {
        private const string OBJECT_NAME = "_SceneAttachment";
        private static SceneAttachment _instance;
        private static GameObject _container;
        private readonly List<Action<bool>> _pauseListeners = new List<Action<bool>>();
        private readonly List<Action> _updateListeners = new List<Action>();

        private static SceneAttachment GetInstance() {
            if( _instance != null ) {
                return _instance;
            }
            if( _container != null ) {
                Destroy( _container );
            }
            _container = new GameObject( OBJECT_NAME );
            DontDestroyOnLoad( _container );
            _instance = _container.AddComponent<SceneAttachment>();
            return _instance;
        }

        public static void AddPauseListener( Action<bool> pauseAction ) {
            List<Action<bool>> pList = GetInstance()._pauseListeners;
            if( !pList.Contains( pauseAction ) ) {
                pList.Add( pauseAction );
            }
        }

        public static void RemovePauseListener( Action<bool> pauseAction ) {
            List<Action<bool>> pList = GetInstance()._pauseListeners;
            if( pList.Contains( pauseAction ) ) {
                pList.Remove( pauseAction );
            }
        }

        public static void AddUpdateListener( Action updateAction ) {
            List<Action> uList = GetInstance()._updateListeners;
            if( !uList.Contains( updateAction ) ) {
                uList.Add( updateAction );
            }
        }

        public static void RemoveUpdateListener( Action updateAction ) {
            List<Action> uList = GetInstance()._updateListeners;
            if( uList.Contains( updateAction ) ) {
                uList.Remove( updateAction );
            }
        }

        private void Update() {
            if( _updateListeners.Count <= 0 ) {
                return;
            }
            List<Action> tempList = new List<Action>( _updateListeners );
            for( int i = 0; i < tempList.Count; i++ ) {
                tempList[i]();
            }
        }

        private void OnApplicationPause( bool pauseState ) {
            if( _pauseListeners.Count <= 0 ) {
                return;
            }
            List<Action<bool>> tempList = new List<Action<bool>>( _pauseListeners );
            for( int i = 0; i < tempList.Count; i++ ) {
                tempList[i]( pauseState );
            }
        }
    }
}