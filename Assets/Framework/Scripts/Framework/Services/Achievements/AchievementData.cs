using Achievements.Engines;

namespace Achievements
{
    public class AchievementData
    {
        public int Id;
        public AchievementTopic Topic;
        public AchievementState State;
        public EngineType EType;
        public int Value;
        public string PlayServicesId;
        public string GameCenterId;

        public AchievementData(int id, AchievementTopic topic, EngineType engineType, int value, string playServicesId, string gameCenterId)
        {
            this.Id = id;
            this.EType = engineType;
            this.State = AchievementState.ACTIVE;
            this.Topic = topic;
            this.Value = value;
            this.PlayServicesId = playServicesId;
            this.GameCenterId = gameCenterId;
        }
    }
}
