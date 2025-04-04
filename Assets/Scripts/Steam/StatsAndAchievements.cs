using NeonLadder.Mechanics.Enums;
using Steamworks;
using UnityEngine;

namespace NeonLadder.Steam
{
    class StatsAndAchievements : MonoBehaviour
    {
        private Achievement[] achievements = new Achievement[] {
        //new Achievement(Achievements.ACH_WIN_ONE_GAME, "Winner", ""),
        //new Achievement(Achievements.ACH_WIN_100_GAMES, "Champion", ""),
        //new Achievement(Achievements.ACH_TRAVEL_FAR_ACCUM, "Interstellar", ""),
        //new Achievement(Achievements.ACH_TRAVEL_FAR_SINGLE, "Orbiter", "")
    };

        private CGameID gameID;
        private bool requestedStats;
        private bool statsValid;
        private bool storeStats;

        private float gameFeetTraveled;
        private float tickCountGameStart;
        private double gameDurationSeconds;

        private int totalGamesPlayed;
        private int totalNumWins;
        private int totalNumLosses;
        private float totalFeetTraveled;
        private float maxFeetTraveled;
        private float averageSpeed;

        protected Callback<UserStatsReceived_t> userStatsReceived;
        protected Callback<UserStatsStored_t> userStatsStored;
        protected Callback<UserAchievementStored_t> userAchievementStored;

        void OnEnable()
        {
            if (!SteamManager.Initialized) return;

            gameID = new CGameID(SteamUtils.GetAppID());

            userStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
            userStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
            userAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);

            requestedStats = false;
            statsValid = false;
        }

        void Update()
        {
            if (!SteamManager.Initialized) return;

            if (!requestedStats)
            {
                if (!SteamManager.Initialized)
                {
                    requestedStats = true;
                    return;
                }
                requestedStats = SteamUserStats.RequestCurrentStats();
            }

            if (!statsValid) return;

            EvaluateAchievements();
            StoreStatsIfNeeded();
        }

        private void EvaluateAchievements()
        {
            foreach (var achievement in achievements)
            {
                if (achievement.Achieved) continue;

                switch (achievement.AchievementID)
                {
                    case 0:
                    break;
                    //case Achievements.ACH_WIN_ONE_GAME:
                    //    if (totalNumWins != 0)
                    //    {
                    //        UnlockAchievement(achievement);
                    //    }
                    //    break;
                    //case Achievements.ACH_WIN_100_GAMES:
                    //    if (totalNumWins >= 100)
                    //    {
                    //        UnlockAchievement(achievement);
                    //    }
                    //    break;
                    //case Achievements.ACH_TRAVEL_FAR_ACCUM:
                    //    if (totalFeetTraveled >= 5280)
                    //    {
                    //        UnlockAchievement(achievement);
                    //    }
                    //    break;
                    //case Achievements.ACH_TRAVEL_FAR_SINGLE:
                    //    if (gameFeetTraveled >= 500)
                    //    {
                    //        UnlockAchievement(achievement);
                    //    }
                    //    break;
                }
            }
        }

        private void StoreStatsIfNeeded()
        {
            if (!storeStats) return;

            SteamUserStats.SetStat("NumGames", totalGamesPlayed);
            SteamUserStats.SetStat("NumWins", totalNumWins);
            SteamUserStats.SetStat("NumLosses", totalNumLosses);
            SteamUserStats.SetStat("FeetTraveled", totalFeetTraveled);
            SteamUserStats.SetStat("MaxFeetTraveled", maxFeetTraveled);
            SteamUserStats.UpdateAvgRateStat("AverageSpeed", gameFeetTraveled, gameDurationSeconds);
            SteamUserStats.GetStat("AverageSpeed", out averageSpeed);

            storeStats = !SteamUserStats.StoreStats();
        }

        public void AddDistanceTraveled(float distance)
        {
            gameFeetTraveled += distance;
        }

        public void OnGameStateChange(GameStates newState)
        {
            if (!statsValid) return;

            if (newState == GameStates.Active)
            {
                gameFeetTraveled = 0;
                tickCountGameStart = Time.time;
            }
            else if (newState == GameStates.Winner || newState == GameStates.Loser)
            {
                if (newState == GameStates.Winner)
                {
                    totalNumWins++;
                }
                else
                {
                    totalNumLosses++;
                }

                totalGamesPlayed++;
                totalFeetTraveled += gameFeetTraveled;

                if (gameFeetTraveled > maxFeetTraveled)
                    maxFeetTraveled = gameFeetTraveled;

                gameDurationSeconds = Time.time - tickCountGameStart;
                storeStats = true;
            }
        }

        private void UnlockAchievement(Achievement achievement)
        {
            achievement.Achieved = true;
            SteamUserStats.SetAchievement(achievement.AchievementID.ToString());
            storeStats = true;
        }

        private void OnUserStatsReceived(UserStatsReceived_t callback)
        {
            if (!SteamManager.Initialized) return;

            if ((ulong)gameID == callback.m_nGameID)
            {
                if (EResult.k_EResultOK == callback.m_eResult)
                {
                    Debug.Log("Received stats and achievements from Steam\n");

                    statsValid = true;
                    LoadAchievements();
                    LoadStats();
                }
                else
                {
                    Debug.Log("RequestStats - failed, " + callback.m_eResult);
                }
            }
        }

        private void LoadAchievements()
        {
            foreach (var achievement in achievements)
            {
                if (SteamUserStats.GetAchievement(achievement.AchievementID.ToString(), out achievement.Achieved))
                {
                    achievement.Name = SteamUserStats.GetAchievementDisplayAttribute(achievement.AchievementID.ToString(), "name");
                    achievement.Description = SteamUserStats.GetAchievementDisplayAttribute(achievement.AchievementID.ToString(), "desc");
                }
                else
                {
                    Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + achievement.AchievementID + "\nIs it registered in the Steam Partner site?");
                }
            }
        }

        private void LoadStats()
        {
            SteamUserStats.GetStat("NumGames", out totalGamesPlayed);
            SteamUserStats.GetStat("NumWins", out totalNumWins);
            SteamUserStats.GetStat("NumLosses", out totalNumLosses);
            SteamUserStats.GetStat("FeetTraveled", out totalFeetTraveled);
            SteamUserStats.GetStat("MaxFeetTraveled", out maxFeetTraveled);
            SteamUserStats.GetStat("AverageSpeed", out averageSpeed);
        }

        private void OnUserStatsStored(UserStatsStored_t callback)
        {
            if ((ulong)gameID == callback.m_nGameID)
            {
                if (EResult.k_EResultOK == callback.m_eResult)
                {
                    Debug.Log("StoreStats - success");
                }
                else if (EResult.k_EResultInvalidParam == callback.m_eResult)
                {
                    Debug.Log("StoreStats - some failed to validate");
                    OnUserStatsReceived(new UserStatsReceived_t { m_eResult = EResult.k_EResultOK, m_nGameID = (ulong)gameID });
                }
                else
                {
                    Debug.Log("StoreStats - failed, " + callback.m_eResult);
                }
            }
        }

        private void OnAchievementStored(UserAchievementStored_t callback)
        {
            if ((ulong)gameID == callback.m_nGameID)
            {
                if (0 == callback.m_nMaxProgress)
                {
                    Debug.Log("Achievement '" + callback.m_rgchAchievementName + "' unlocked!");
                }
                else
                {
                    Debug.Log("Achievement '" + callback.m_rgchAchievementName + "' progress callback, (" + callback.m_nCurProgress + "," + callback.m_nMaxProgress + ")");
                }
            }
        }

        public void Render()
        {
            if (!SteamManager.Initialized)
            {
                GUILayout.Label("Steamworks not Initialized");
                return;
            }

            GUILayout.Label("tickCountGameStart: " + tickCountGameStart);
            GUILayout.Label("gameDurationSeconds: " + gameDurationSeconds);
            GUILayout.Label("gameFeetTraveled: " + gameFeetTraveled);
            GUILayout.Space(10);
            GUILayout.Label("NumGames: " + totalGamesPlayed);
            GUILayout.Label("NumWins: " + totalNumWins);
            GUILayout.Label("NumLosses: " + totalNumLosses);
            GUILayout.Label("FeetTraveled: " + totalFeetTraveled);
            GUILayout.Label("MaxFeetTraveled: " + maxFeetTraveled);
            GUILayout.Label("AverageSpeed: " + averageSpeed);

            GUILayout.BeginArea(new Rect(Screen.width - 300, 0, 300, 800));
            foreach (var achievement in achievements)
            {
                GUILayout.Label(achievement.AchievementID.ToString());
                GUILayout.Label(achievement.Name + " - " + achievement.Description);
                GUILayout.Label("Achieved: " + achievement.Achieved);
                GUILayout.Space(20);
            }

            if (GUILayout.Button("RESET STATS AND ACHIEVEMENTS"))
            {
                SteamUserStats.ResetAllStats(true);
                SteamUserStats.RequestCurrentStats();
                OnGameStateChange(GameStates.Active);
            }
            GUILayout.EndArea();
        }
    }
}