using System;
using System.Collections.Generic;
using Achievements;
using Assets.Scripts.Framework.Services;

interface Achievement : Provider {
    void Trigger(AchievementTopic topic, int value);
    List<string> GetCompletedPlayServicesAchievementIds();
    List<string> GetCompletedGameCenterAchievementIds();
    void AddCompletionListener(Action<string, string> listener);
    void RemoveCompletionListener(Action<string, string> listener);
    void CompleteAchievementByGameCenterId(string gameCenterId);
    void CompleteAchievementByPlayServicesId(string playServicesId);
}
