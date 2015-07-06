using System;
using Assets.Scripts.Framework.Services;

interface Life : Provider {
    int GetCurrentLives();
    int GetMaxLives();
    void SpendLife();
    int GetTimeToFull();
	int GetTimeToNextLife();
    void Fill();
    bool HasLife();
    void RewardLife( int amount );
    void AddTimeListener( Action<int> callback );
    void AddLifeListener( Action<int> callback );
    void RemoveTimeListener( Action<int> callback );
    void RemoveLifeListener( Action<int> callback );
}