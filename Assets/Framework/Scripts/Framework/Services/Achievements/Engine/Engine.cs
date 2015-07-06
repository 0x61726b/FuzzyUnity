namespace Achievements.Engines
{
    public interface Engine
    {
        void Trigger(int step);
        bool IsRelevant(AchievementTopic topic);
        bool IsEngineOf(int id);
    }
}
