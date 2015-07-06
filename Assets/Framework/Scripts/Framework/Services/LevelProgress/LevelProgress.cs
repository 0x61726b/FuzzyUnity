using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.Services;

interface LevelProgress : Provider {
    void AddStarListener( Action<int,int> listener );
    void AddScoreListener( Action<int,int> listener );
    void AddLevelListener( Action<int> listener );
    void RemoveStarListener( Action<int,int> listener );
    void RemoveScoreListener( Action<int,int> listener );
    void RemoveLevelListener( Action<int> listener );

    bool HasCompleted( int level );
    int GetCurrentLevel();
    int GetStars( int level );
    int GetTotalStarCount();
    int GetScore( int level );
    void SetProgress( int level, int starCount, int score );
    void OverwriteProgress( int currentLevel, IDictionary<int, int> levelScore, IDictionary<int, int> levelStars );
}