using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
// using com.gramgames.analytics;
using Assets.Scripts.Framework.Services;
using Assets.Scripts.Framework.Util;
using com.gramgames.analytics;

public class LifeProvider : Life {
    private int life;
    private long lastChargeStart = -1;
    private float elapsed = 0;
    private bool timerRunning = false;
    // IN SECONDS:
    private int timerTick = 1;

    private List<Action<int>> timeListeners = new List<Action<int>>();
    private List<Action<int>> lifeListeners = new List<Action<int>>();

    private const string LIFE_KEY = "_Life";
    private const string LIFE_CHARGE_START = "_Life_ChargeStart";
    private const int LIFE_MAX_LIVES = 10;
    // IN SECONDS:
    private const int LIFE_CHARGE_DURATION = 20 * 60;
    
    public void Start() {
		setLife(ServiceLocator.GetDB().GetInt(LIFE_KEY, LIFE_MAX_LIVES));

        CheckGeneratedLife();

		ServiceLocator.RegisterPauseListener( OnPause );
		ServiceLocator.RegisterUpdateListener( Update );
    }

    private void CheckGeneratedLife() {
        //Check generated life while offline
		lastChargeStart = ServiceLocator.GetDB().GetLong(LIFE_CHARGE_START, -1L);

        if (lastChargeStart != -1L)
        {
            long offlineTime = Util.GetTime() - lastChargeStart;
            int lifeGenerated = (int)Math.Floor((double)offlineTime / LIFE_CHARGE_DURATION);

            setLife(life + lifeGenerated);

            if (life < LIFE_MAX_LIVES && lastChargeStart > 0)
            {
                startCharging(Util.GetTime() - (offlineTime % LIFE_CHARGE_DURATION));
            }
            else
            {
                resetCharging();
            }
        }
        else if (life < LIFE_MAX_LIVES)
        {
            startCharging(Util.GetTime());
        }
    }

    public bool HasLife() {
        return life > 0;
    }

    public void OnPause( bool pause ) {
        if ( !pause ) {
            CheckGeneratedLife();
            if(isCharging())
                onTick();
        }
    }

    private bool isCharging()
    {
        return lastChargeStart != -1L;
    }

    public void Destroy() {
		ServiceLocator.RemovePauseListener( OnPause );
		ServiceLocator.RemoveUpdateListener( Update );
    }

	public void Awake()
	{

	}

    public void RewardLife( int amount ) {
		ServiceLocator.GetUpsight().TrackEvent( "economy" , "life" , "source" , "reward" , amount.ToString() );
        if ( life + amount >= LIFE_MAX_LIVES ) {
            Fill();
        } else {
            setLife( life + amount );
        }
    }

    public void Update() {
		if ( timerRunning ) {
            elapsed += Time.deltaTime;
            while ( elapsed > timerTick ) {
                elapsed -= timerTick;
                onTick();
            }
        }
    }

    private void OnApplicationPause(bool pauseStatus) {
        if (!pauseStatus) {
            //onTick();
        }
    }

    public int GetCurrentLives() {
        return this.life;
    }

    public int GetMaxLives() {
        return LIFE_MAX_LIVES;
    }

    public void AddTimeListener( Action<int> callback ) {
        if ( !timeListeners.Contains( callback ) ) {
            timeListeners.Add( callback );
            if ( !timerRunning ) {
                callback( 0 );
            } else {
                callback( getSecondsToNextLife() );
            }
        }
    }

    public void AddLifeListener( Action<int> callback ) {
        if ( !lifeListeners.Contains( callback ) ) {
            lifeListeners.Add( callback );
            callback( life );
        }
    }

    public void RemoveTimeListener( Action<int> callback ) {
        if ( timeListeners.Contains( callback ) ) {
            timeListeners.Remove( callback );
        }
    }

    public void RemoveLifeListener( Action<int> callback ) {
        if ( lifeListeners.Contains( callback ) ) {
            lifeListeners.Remove( callback );
        }
    }

    public void SpendLife() {
		ServiceLocator.GetAnalytics().TrackEvent( AnalyticEvent.LIFE_SPENT );
		ServiceLocator.GetUpsight().TrackEvent( "economy" , "life" , "sink" , "lose_lives" , "-1" );
        setLife( Math.Max( 0 , life - 1 ) );

        if ( lastChargeStart == -1 ) {
            startCharging( Util.GetTime() );   
        }

        notifyLifeListeners();
    }

    public int GetTimeToFull() {
        if ( life != LIFE_MAX_LIVES ) {
            return ( int ) ( ( ( LIFE_MAX_LIVES - ( life + 1 ) ) * LIFE_CHARGE_DURATION ) + ( LIFE_CHARGE_DURATION - ( Util.GetTime() - lastChargeStart ) ) );
        } else {
            return 0;
        }
        
    }

    public int GetTimeToNextLife() {
        return getSecondsToNextLife();
    }

    public void Fill() {
		ServiceLocator.GetAnalytics().TrackEvent( AnalyticEvent.LIFE_FILL );
		ServiceLocator.GetUpsight().TrackEvent( "economy" , "life" , "source" , "fill" , LIFE_MAX_LIVES.ToString() );
        resetCharging();
        setLife( LIFE_MAX_LIVES );
    }

    void notifyLifeListeners() {
        foreach ( Action<int> lifeList in lifeListeners ) {
            lifeList( life );
        }
    }

    void notifyTimeListeners( int secsToNextLife ) {
		foreach ( Action<int> timeList in timeListeners ) {
            timeList( secsToNextLife );
        }
    }

    void resetCharging() {
		ServiceLocator.GetDB().SetLong( LIFE_CHARGE_START , -1L , true );
        lastChargeStart = -1;

        resetTimer();
    }

    void startCharging( long startTime ) {
        lastChargeStart = startTime;
		ServiceLocator.GetDB().SetLong( LIFE_CHARGE_START , lastChargeStart , true );

        notifyTimeListeners( LIFE_CHARGE_DURATION );

        resetTimer();
        if ( !timerRunning ) {
            elapsed = 0;
            timerRunning = true;
        }

    }

    void resetTimer() {
        if ( timerRunning ) {
            timerRunning = false;
            elapsed = 0;
            notifyTimeListeners( 0 );
        }
    }

    int getSecondsToNextLife() {
        return ( int ) ( LIFE_CHARGE_DURATION - ( Util.GetTime() - lastChargeStart ) );
    }

    void onTick() {
        int left = getSecondsToNextLife();
        notifyTimeListeners( ( int ) left );
        if ( left <= 0 ) {
            setLife( life + 1 );
			ServiceLocator.GetUpsight().TrackEvent( "economy" , "life" , "source" , "time" , "1" );
            if ( life < LIFE_MAX_LIVES ) {
                startCharging( Util.GetTime() );
            } else {
                resetCharging();
            }
        }
    }

    void setLife( int newLife ) {
        int lifeNew = Math.Min( LIFE_MAX_LIVES , newLife );
        if ( lifeNew != life ) {
            life = lifeNew;
            notifyLifeListeners();
			ServiceLocator.GetDB().SetInt( LIFE_KEY , life , true );
        }
    }

}

