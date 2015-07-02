using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;

public class AchievementHandler : MonoBehaviour
{
    private List<F8Achievement> m_vAchievements;
    public List<F8Achievement> Achievements
    {
        get { return m_vAchievements; }
    }
	// Use this for initialization
	void Start ()
    {
        m_vAchievements = new List<F8Achievement>();

        F8Achievement highScore10 = new F8Achievement
        {
            Name = "Oh! I got this - Highscore 10",
            Code = "CgkIzs-alcMYEAIQAg"
        };
        F8Achievement highScore25 = new F8Achievement
        {
            Name = "Getting the hang of it",
            Code = "CgkIzs-alcMYEAIQAw"
        };
        F8Achievement highScore75 = new F8Achievement
        {
            Name = "Ambitious",
            Code = "CgkIzs-alcMYEAIQBA"
        };
        F8Achievement highScore150 = new F8Achievement
        {
            Name = "Tier 0",
            Code = "CgkIzs-alcMYEAIQBQ"
        };
        F8Achievement highScore250 = new F8Achievement
        {
            Name = "Superior Kind",
            Code = "CgkIzs-alcMYEAIQBg"
        };
        F8Achievement highScore1000 = new F8Achievement
        {
            Name = "Freak",
            Code = "CgkIzs-alcMYEAIQBw"
        };

        F8Achievement played5 = new F8Achievement
        {
            Name = "Newcomer",
            Code = "CgkIzs-alcMYEAIQCA"
        };
        F8Achievement played20 = new F8Achievement
        {
            Name = "Rookie ",
            Code = "CgkIzs-alcMYEAIQCQ"
        };
        F8Achievement played50 = new F8Achievement
        {
            Name = "Regular",
            Code = "CgkIzs-alcMYEAIQCg"
        };
        F8Achievement played100 = new F8Achievement
        {
            Name = "Citizen",
            Code = "CgkIzs-alcMYEAIQCw"
        };
        F8Achievement played250 = new F8Achievement
        {
            Name = "Leecher",
            Code = "CgkIzs-alcMYEAIQDA"
        };
        F8Achievement dieFirstTime = new F8Achievement
        {
            Name = "WTH",
            Code = "CgkIzs-alcMYEAIQDQ"
        };
        F8Achievement womboCombo = new F8Achievement
        {
            Name = "Leecher",
            Code = "CgkIzs-alcMYEAIQDg"
        };

        m_vAchievements.Add(highScore10);
        m_vAchievements.Add(highScore25);
        m_vAchievements.Add(highScore75);
        m_vAchievements.Add(highScore150);
        m_vAchievements.Add(highScore250);
        m_vAchievements.Add(highScore1000);
        m_vAchievements.Add(played5);
        m_vAchievements.Add(played20);
        m_vAchievements.Add(played50);
        m_vAchievements.Add(played100);
        m_vAchievements.Add(played250);
        m_vAchievements.Add(dieFirstTime);
        m_vAchievements.Add(womboCombo);

        Social.LoadAchievements(achv =>
            {
                if(achv.Length > 0)
                {
                    foreach(IAchievement achievement in achv)
                    {
                        for( int k=0; k < m_vAchievements.Count;k++)
                        {
                            if(m_vAchievements[k].Code == achievement.id)
                            {
                                m_vAchievements[k].Achieved = achievement.completed;
                            }
                        }
                    }
                }
            });
	}
	public void Update ()
    {
	
	}
    public void Process(F8Achievement achv)
    {
        if(!achv.Achieved)
        {
            Social.ReportProgress(achv.Code, achv.Progress, (bool success) =>
            {
                
            });
        }
        
    }
}
