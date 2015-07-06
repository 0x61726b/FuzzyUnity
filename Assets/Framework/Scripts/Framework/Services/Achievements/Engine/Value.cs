using System;

namespace Achievements.Engines {
public class Value : Engine {
    private int id;
    private Action onValueReached;
    private int targetValue;
    private AchievementTopic topic;

    public Value(int id, AchievementTopic topic, int targetValue, Action onValueReached) {
        this.id = id;
        this.topic = topic;
        this.onValueReached = onValueReached;
        this.targetValue = targetValue;
    }

    public bool IsRelevant(AchievementTopic topic) {
        return this.topic == topic;
    }

    public bool IsEngineOf(int id) {
        return this.id == id;
    }

    public void Trigger(int value) {
        if (value == targetValue) {
            onValueReached();
        }
    }
}
}
