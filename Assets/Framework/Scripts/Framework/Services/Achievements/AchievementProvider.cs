using System;
using Achievements.Engines;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Framework.Services;

namespace Achievements
{
    class AchievementProvider : Achievement
    {
        private const string ACHIEVEMENT_COMPLETED_KEY = "achiement_isComplete_";
        List<Engine> engines;
        List<Action<string, string>> listeners = new List<Action<string, string>>();

        public void Start()
        {
            load();
            bootEngines();
		}
		
		public void Destroy()
		{
		}
		
		public void Awake()
		{
		}

        public void Trigger(AchievementTopic topic, int value)
        {
            Engine[] cloneEngines = new Engine[engines.Count];
            engines.CopyTo(cloneEngines);
            for (int i = 0; i < cloneEngines.Length; i++)
            {
                if (cloneEngines[i].IsRelevant(topic))
                {
                    cloneEngines[i].Trigger(value);
                }
            }
        }

        public List<string> GetCompletedPlayServicesAchievementIds()
        {
            List<string> returnList = new List<string>();

            foreach (AchievementData aData in AchievementConfig.Achievements)
            {
                if (aData.State == AchievementState.COMPLETED)
                {
                    returnList.Add(aData.PlayServicesId);
                }
            }

            return returnList;
        }

        public List<string> GetCompletedGameCenterAchievementIds()
        {
            List<string> returnList = new List<string>();

            foreach (AchievementData aData in AchievementConfig.Achievements)
            {
                if (aData.State == AchievementState.COMPLETED)
                {
                    returnList.Add(aData.GameCenterId);
                }
            }

            return returnList;
        }

        public void AddCompletionListener(Action<string, string> listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void RemoveCompletionListener(Action<string, string> listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }

        public void CompleteAchievementByGameCenterId(string gameCenterId)
        {
            foreach (AchievementData aData in AchievementConfig.Achievements)
            {
                if (aData.GameCenterId == gameCenterId && aData.State != AchievementState.COMPLETED)
                {
                    completeAchievement(aData);
                }
            }
        }

        public void CompleteAchievementByPlayServicesId(string playServicesId)
        {
            foreach (AchievementData aData in AchievementConfig.Achievements)
            {
                if (aData.PlayServicesId == playServicesId && aData.State != AchievementState.COMPLETED)
                {
                    completeAchievement(aData);
                }
            }
        }



        private void notifyListeners(string playServicesId, string gameCenterId)
        {
            Action<string, string>[] cloneListeners = new Action<string, string>[listeners.Count];
            listeners.CopyTo(cloneListeners);
            for (int i = 0; i < cloneListeners.Length; i++)
            {
                cloneListeners[i](playServicesId, gameCenterId);
            }
        }

        private void bootEngines()
        {
            AchievementData[] achievements = AchievementConfig.Achievements;
            engines = new List<Engine>();
            for (int i = 0; i < achievements.Length; i++)
            {
                if (achievements[i].State == AchievementState.ACTIVE)
                {
                    engines.Add(EngineFactory.GetEngine(achievements[i], getCompleteFunction(achievements[i])));
                }
            }
        }

        private void completeAchievement(AchievementData aData)
        {
            Debug.Log("---> Achievement Completed: " + aData.Id);
            ServiceLocator.GetDB().SetBool(ACHIEVEMENT_COMPLETED_KEY + aData.Id, true, true);
            aData.State = AchievementState.COMPLETED;
            List<Engine> toRemove = new List<Engine>();

            foreach (Engine engine in engines)
            {
                if (engine.IsEngineOf(aData.Id))
                {
                    toRemove.Add(engine);
                }
            }

            foreach (Engine engine in toRemove)
            {
                engines.Remove(engine);
            }
        }

        private Action getCompleteFunction(AchievementData aData)
        {
            return () =>
                   {
                       completeAchievement(aData);
                       notifyListeners(aData.PlayServicesId, aData.GameCenterId);
                   };
        }

        private void load()
        {
            foreach (AchievementData aData in AchievementConfig.Achievements)
            {
				if (ServiceLocator.GetDB().GetBool(ACHIEVEMENT_COMPLETED_KEY + aData.Id, false))
                {
                    aData.State = AchievementState.COMPLETED;
                }
            }
        }

    }
}
