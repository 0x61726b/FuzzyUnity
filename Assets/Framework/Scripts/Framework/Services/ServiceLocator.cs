using System;
using System.Collections.Generic;
using UnityEngine;
using com.gramgames.analytics;
using Assets.Scripts.Framework.Services.AudioService;

namespace Assets.Scripts.Framework.Services {
    internal class ServiceLocator : MonoBehaviour {
        private static readonly Dictionary<Service, Provider> serviceDict = new Dictionary<Service, Provider>();

		private static ServiceLocator _listener;
		private static GameObject _obj;
		private static readonly List<Action> updateListeners = new List<Action>();
		private static readonly List<Action<bool>> pauseListeners = new List<Action<bool>>();

        public static void Boot( Dictionary<Service, Provider> services ) {
            foreach( KeyValuePair<Service, Provider> pair in services ) {
                if( serviceDict.ContainsKey( pair.Key ) ) {
                    throw new Exception( "Service already loaded for: " + pair.Key );
                }
                serviceDict.Add( pair.Key, pair.Value );
                pair.Value.Awake();
            }

            StartServices();
        }

		public static void RegisterUpdateListener( Action listener ) {
			if( !updateListeners.Contains( listener ) ) {
				updateListeners.Add( listener );
			}
		}
		
		public static void RemoveUpdateListener( Action listener ) {
			if( updateListeners.Contains( listener ) ) {
				updateListeners.Remove( listener );
			}
		}
		
		public static void RegisterPauseListener( Action<bool> listener ) {
			if( !pauseListeners.Contains( listener ) ) {
				pauseListeners.Add( listener );
			}
		}
		
		public static void RemovePauseListener( Action<bool> listener ) {
			if( pauseListeners.Contains( listener ) ) {
				pauseListeners.Remove( listener );
			}
		}

		private static void StartServices() {
			if( _listener == null ) {
				init();
			}

			foreach( KeyValuePair<Service, Provider> pair in serviceDict ) {
                pair.Value.Start();
            }
        }

        public static DB GetDB() {
            return GetService<DB>( Service.DB );
		}
		
		public static Analytics GetAnalytics() {
			return GetService<Analytics>( Service.Analytics );
		}
		
		public static UAnalytics GetUpsight() {
			return GetService<UAnalytics>( Service.UAnalytics );
		}

        public static Settings GetSettings() {
            return GetService<Settings>( Service.Settings );
        }

        public static Audio GetAudio() {
            return GetService<Audio>( Service.Audio );
		}
		
		public static Localization GetLocalization() {
			return GetService<Localization>( Service.Localization );
		}
		
		public static Store.Store GetStore() {
			return GetService<Store.Store>( Service.Store );
		}
		
		public static Life GetLife() {
			return GetService<Life>( Service.Life );
		}
		
		public static Achievement GetAchievement() {
			return GetService<Achievement>( Service.Achievement );
		}
		
		public static LevelProgress GetLevelProgress() {
			return GetService<LevelProgress>( Service.LevelProgress );
		}

        private static T GetService<T>( Service serviceType ) {
            if( serviceDict.ContainsKey( serviceType ) ) {
                return ( T ) serviceDict[serviceType];
            }
            throw new Exception( "Service not ready: "+ serviceType);
        }

		public static Notification GetNotification() {
			return GetService<Notification>(Service.Notification);
		}

		private static void init() {
			_obj = new GameObject( "Locator" );
			_listener = _obj.AddComponent<ServiceLocator>();
		}
		
		private void Update() {
			ServiceLocator.update();
		}
		
		private void OnApplicationPause( bool pause ) {
			ServiceLocator.pause( pause );
		}

		private static void update() {
			if( updateListeners.Count > 0 ) {
				foreach( Action updateListener in updateListeners ) {
					updateListener();
				}
			}
		}
		
		private static void pause( bool pause ) {
			if( pauseListeners.Count > 0 ) {
				foreach( Action<bool> pauseListener in pauseListeners ) {
					pauseListener( pause );
				}
			}
		}

        public static void Destroy() {
			Destroy( _obj );
			_listener = null;
			_obj = null;
			foreach( KeyValuePair<Service, Provider> pair in serviceDict ) {
				pair.Value.Destroy();
			}
		}
    }
}