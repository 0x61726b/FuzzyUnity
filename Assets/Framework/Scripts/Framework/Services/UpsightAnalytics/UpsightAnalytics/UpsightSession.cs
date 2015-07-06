using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.Services;
using Assets.Scripts.Framework.Util;

namespace com.gramgames.analytics {
    internal class UpsightSession {
        private const string SESSION_NUMBER_KEY = "upsightSessionNumber";
        private const string ANALYTICS_UQ_ID_KEY = "upsightUniqueId";
        private const string LAST_SESSION_ENDTIME = "upsightLastSessionEndTime";
        private static readonly string APPLICATION_VERSION = Game.APPLICATION_VERSION.ToString();
        public long GoldBalance;
        public long Level;
        public long LifeBalance;
        public ulong SessionId;
        public long SessionNumber;
        public long SessionStart;
        public string UserUniqueId;

        public UpsightSession() {
            UserUniqueId = GetUniqueId();
            if( IsAdjacentSession() ) {
				SessionNumber = ServiceLocator.GetDB().GetLong( SESSION_NUMBER_KEY, 0L );
            } else {
				SessionNumber = ServiceLocator.GetDB().GetLong( SESSION_NUMBER_KEY, 0L ) + 1;
            }

			ServiceLocator.GetDB().SetLong( SESSION_NUMBER_KEY, SessionNumber, true );
            SessionId = GenerateUniqueId();
            SessionStart = Util.GetTime();
			// TODO: bura // GoldBalance = ServiceLocator.GetFinance().GetBalance();
			LifeBalance = ServiceLocator.GetLife().GetCurrentLives();
			Level = ServiceLocator.GetLevelProgress().GetCurrentLevel();

            RegisterListeners();
        }

        private static bool IsAdjacentSession() {
			long lastSessionEndTime = ServiceLocator.GetDB().GetLong( LAST_SESSION_ENDTIME, 0L );
            return (Util.GetTime() - lastSessionEndTime) < 600L;
        }

        public void EndSession() {
			ServiceLocator.GetDB().SetLong( LAST_SESSION_ENDTIME, Util.GetTime() );
            RemoveListeners();
        }

        public Dictionary<string, string> GetSummary() {
            Dictionary<string, string> summary = new Dictionary<string, string> {{"UserUniqueId", UserUniqueId}, {"SessionNumber", SessionNumber.ToString()}, {"SessionId", SessionId.ToString()}};
            long duration = Util.GetTime() - SessionStart;
            summary.Add( "SessionTime", duration.ToString() );
            summary.Add( "GoldBalance", GoldBalance.ToString() );
            summary.Add( "LifeBalance", LifeBalance.ToString() );
            summary.Add( "CurrentLevel", Level.ToString() );
            summary.Add( "ApplicationVersion", APPLICATION_VERSION );
            return summary;
        }

        private static string GetUniqueId() {
			string uniqueId = ServiceLocator.GetDB().GetString( ANALYTICS_UQ_ID_KEY, string.Empty );

            if( uniqueId != string.Empty ) {
                return uniqueId;
            }
            uniqueId = GenerateUniqueId().ToString();
			ServiceLocator.GetDB().SetString( ANALYTICS_UQ_ID_KEY, uniqueId, true );
            return uniqueId;
        }

        private void RegisterListeners() {
			// TODO: bura // ServiceLocator.GetFinance().AddBalanceUpdateListener( OnBalanceUpdated );
			ServiceLocator.GetLife().AddLifeListener( OnLifeUpdated );
			ServiceLocator.GetLevelProgress().AddLevelListener( OnLevelUpdated );
        }

        private void RemoveListeners() {
			// TODO: bura // ServiceLocator.GetFinance().RemoveBalanceUpdateListener( OnBalanceUpdated );
			ServiceLocator.GetLife().RemoveLifeListener( OnLifeUpdated );
			ServiceLocator.GetLevelProgress().RemoveLevelListener( OnLevelUpdated );
        }

        private void OnLevelUpdated( int level ) {
            Level = level;
        }

        private void OnBalanceUpdated( int balance ) {
            GoldBalance = balance;
        }

        private void OnLifeUpdated( int life ) {
            LifeBalance = life;
        }

        private static ulong GenerateUniqueId() {
            return new Random64( new Random() ).Next();
        }
    }
}