using System;
using System.Collections.Generic;

namespace Assets.Scripts.Framework.Bridge {
    internal static class UIBridge {
        private static readonly EventDispatcher<UIEvent> UiDispatcher = new EventDispatcher<UIEvent>();
        private static readonly EventDispatcher<GameEvent> GameDispatcher = new EventDispatcher<GameEvent>();

        public static void PublishEvent(GameEvent anEvent, Dictionary<string, object> parameters = null) {
            GameDispatcher.OnEvent(anEvent, parameters);
        }

        public static void PublishEvent(UIEvent anEvent, Dictionary<string, object> parameters = null) {
            UiDispatcher.OnEvent(anEvent, parameters);
        }

        public static void AddListener(UIEvent anEvent, Action<Dictionary<string, object>> action) {
            UiDispatcher.RegisterEvent(anEvent, action);
        }

        public static void AddListener(GameEvent anEvent, Action<Dictionary<string, object>> action) {
            GameDispatcher.RegisterEvent(anEvent, action);
        }

        public static void RemoveListener(UIEvent anEvent, Action<Dictionary<string, object>> action) {
            UiDispatcher.RemoveEvent(anEvent, action);
        }

        public static void RemoveListener(GameEvent anEvent, Action<Dictionary<string, object>> action) {
            GameDispatcher.RemoveEvent(anEvent, action);
        }
    }
}