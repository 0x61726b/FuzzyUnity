using System;
using System.Collections.Generic;
using com.gramgames.analytics;
using UnityEngine;
using Achievements;
using Assets.Scripts.Framework.Services;

class ProgressProvider : LevelProgress
{
    private const string CURRENT_LEVEL_KEY = "levelprogress_currentlevel";
    private const string LEVEL_SCORE_PREFIX = "levelprogress_level_score_";
    private const string LEVEL_STAR_PREFIX = "levelprogress_level_star_";

    int currentTargetLevel = 1;
    Dictionary<int, int> scoreDict = new Dictionary<int, int>();
    Dictionary<int, int> starDict = new Dictionary<int, int>();

    List<Action<int, int>> starListener = new List<Action<int, int>>();
    List<Action<int, int>> scoreListener = new List<Action<int, int>>();
    List<Action<int>> levelListener = new List<Action<int>>();

    public void Start()
    {
        load();
	}
	
	public void Destroy()
	{
		
	}
	
	public void Awake()
	{
		
	}

    public int GetCurrentLevel()
    {
        return currentTargetLevel;
    }

    public int GetStars(int level)
    {
        if (starDict.ContainsKey(level))
        {
            return starDict[level];
        }
        else
        {
            return 0;
        }
    }

    public int GetTotalStarCount()
    {
        int totalStarCount = 0;

        for (int i = 1; i < currentTargetLevel; i++)
        {
            if (starDict.ContainsKey(i))
            {
                totalStarCount += starDict[i];
            }
        }
        return totalStarCount;
    }

    public int GetScore(int level)
    {
        if (scoreDict.ContainsKey(level))
        {
            return scoreDict[level];
        }
        else
        {
            return 0;
        }
    }

    public bool HasCompleted(int level)
    {
        return scoreDict.ContainsKey(level) && scoreDict[level] > 0;
    }

    public void SetProgress(int level, int starCount, int score)
    {
        bool change = false;

        if (level == currentTargetLevel)
        {
            currentTargetLevel = level + 1;
            change = true;
            notifyLevel(currentTargetLevel);
			ServiceLocator.GetUpsight().TrackEvent("player", "level_funnel", "", "level_" + level.ToString(), level.ToString());
        }
        if (!starDict.ContainsKey(level) || starCount > starDict[level])
        {
            int increase = starCount;
            if (starDict.ContainsKey(level))
            {
                increase -= starDict[level];
                starDict.Remove(level);
            }
            starDict.Add(level, starCount);
            change = true;
            notifyStar(level, starCount);

			// ServiceLocator.GetAchievement().Trigger(AchievementTopic.STARS, increase);
        }
        if (!scoreDict.ContainsKey(level) || score > scoreDict[level])
        {
            if (scoreDict.ContainsKey(level))
            {
                scoreDict.Remove(level);
            }
            scoreDict.Add(level, score);
            change = true;
            notifyScore(level, score);
        }
        if (change)
        {
            persist();
        }
    }

    public void OverwriteProgress(int currentLevel, IDictionary<int, int> levelScore, IDictionary<int, int> levelStars)
    {
		DB db = ServiceLocator.GetDB();

        currentTargetLevel = db.GetInt(CURRENT_LEVEL_KEY, 1);
        for (int i = 1; i < currentTargetLevel; i++)
        {
            db.Remove(LEVEL_SCORE_PREFIX + i);
            db.Remove(LEVEL_STAR_PREFIX + i);
        }
        db.Remove(CURRENT_LEVEL_KEY);

        currentTargetLevel = currentLevel;
        if (currentLevel == 0)
        {
            currentTargetLevel = 1;
        }
        notifyLevel(currentTargetLevel);

        scoreDict = new Dictionary<int, int>();
        foreach (KeyValuePair<int, int> pair in levelScore)
        {
            scoreDict.Add(pair.Key, pair.Value);
            notifyScore(pair.Key, pair.Value);
        }
        starDict = new Dictionary<int, int>();
        foreach (KeyValuePair<int, int> pair in levelStars)
        {
            starDict.Add(pair.Key, pair.Value);
            notifyStar(pair.Key, pair.Value);
        }
        persist();
    }

    private void notifyLevel(int level)
    {
        foreach (Action<int> listener in levelListener)
        {
            listener(level);
        }
    }

    private void notifyStar(int level, int starCount)
    {
        foreach (Action<int, int> listener in starListener)
        {
            listener(level, starCount);
        }
    }

    private void notifyScore(int level, int score)
    {
        foreach (Action<int, int> listener in scoreListener)
        {
            listener(level, score);
        }
    }

    private void persist()
    {
		DB db = ServiceLocator.GetDB();

        db.SetInt(CURRENT_LEVEL_KEY, currentTargetLevel, false);
        foreach (KeyValuePair<int, int> pair in scoreDict)
        {
            db.SetInt(LEVEL_SCORE_PREFIX + pair.Key, pair.Value, false);
        }
        foreach (KeyValuePair<int, int> pair in starDict)
        {
            db.SetInt(LEVEL_STAR_PREFIX + pair.Key, pair.Value, false);
        }
        db.Flush();
    }

    private void load()
    {
		DB db = ServiceLocator.GetDB();

        currentTargetLevel = db.GetInt(CURRENT_LEVEL_KEY, 1);
        for (int i = 1; i < currentTargetLevel; i++)
        {
            scoreDict.Add(i, db.GetInt(LEVEL_SCORE_PREFIX + i, 0));
            starDict.Add(i, db.GetInt(LEVEL_STAR_PREFIX + i, 0));
        }

    }

    public void AddStarListener(Action<int, int> listener)
    {
        if (!starListener.Contains(listener))
        {
            starListener.Add(listener);
        }
    }

    public void AddScoreListener(Action<int, int> listener)
    {
        if (!scoreListener.Contains(listener))
        {
            scoreListener.Add(listener);
        }
    }

    public void AddLevelListener(Action<int> listener)
    {
        if (!levelListener.Contains(listener))
        {
            levelListener.Add(listener);
        }
    }

    public void RemoveStarListener(Action<int, int> listener)
    {
        if (starListener.Contains(listener))
        {
            starListener.Remove(listener);
        }
    }

    public void RemoveScoreListener(Action<int, int> listener)
    {
        if (scoreListener.Contains(listener))
        {
            scoreListener.Remove(listener);
        }
    }

    public void RemoveLevelListener(Action<int> listener)
    {
        if (levelListener.Contains(listener))
        {
            levelListener.Remove(listener);
        }
    }
}
