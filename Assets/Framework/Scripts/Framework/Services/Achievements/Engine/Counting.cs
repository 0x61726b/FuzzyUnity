using System;
using UnityEngine;
using Assets.Scripts.Framework.Services;

namespace Achievements.Engines {
public class Counting : Engine {
    private const string ENGINE_PERSISTENCE_KEY = "achievement_countingengine_";

    private int id;
    private int currentIndex;
    private int threshold;
    private Action onThreshold;
    private AchievementTopic topic;

    public Counting(int id, AchievementTopic topic, int threshold, Action onThreshold) {
        this.id = id;
        this.topic = topic;
        this.onThreshold = onThreshold;
        this.threshold = threshold;
        this.currentIndex = ServiceLocator.GetDB().GetInt(ENGINE_PERSISTENCE_KEY + id, 0);
    }

    public bool IsRelevant(AchievementTopic topic) {
        return this.topic == topic;
    }

    public bool IsEngineOf(int id) {
        return this.id == id;
    }

    public void Trigger(int step) {
        currentIndex += step;
		ServiceLocator.GetDB().SetInt(ENGINE_PERSISTENCE_KEY + id, currentIndex, false);
        checkThreshold();
    }

    private void checkThreshold() {
        if (currentIndex == threshold) {
            onThreshold();
        }
    }
}
}
