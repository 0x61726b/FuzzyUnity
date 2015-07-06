using System;

namespace Achievements.Engines {
public static class EngineFactory {
    public static Engine GetEngine(AchievementData data, Action onComplete) {
        switch (data.EType) {
            case EngineType.COUNTING:
                return new Counting(data.Id, data.Topic, data.Value, onComplete);
            case EngineType.VALUE:
                return new Value(data.Id, data.Topic, data.Value, onComplete);
            default:
                throw new Exception("Engine Type not implemented.");
        }
    }
}
}
