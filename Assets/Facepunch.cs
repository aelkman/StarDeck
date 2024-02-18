using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Steamworks.Data;
using UnityEngine;

public class Facepunch : MonoBehaviour
{
    public static Facepunch Instance;

    // Start is called before the first frame update
    void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);

        try
        {
            Steamworks.SteamClient.Init( 2687800 );
            Steamworks.SteamUserStats.RequestCurrentStats();
            var killCount = Steamworks.SteamUserStats.GetStatInt( "BATTLE_WINS" );
            Debug.Log("battle wins: " + killCount);
            Debug.Log("steam client initialized, user stats received!");

        }
        catch ( System.Exception e )
        {
            Debug.Log("error getting steamworks data");
            // Something went wrong - it's one of these:
            //
            //     Steam is closed?
            //     Can't find steam_api dll?
            //     Don't have permission to play app?
            //
        }
    }

    // Update is called once per frame
    void Update()
    {
        Steamworks.SteamClient.RunCallbacks();
    }

    void OnApplicationQuit() {
        Steamworks.SteamClient.Shutdown();
    }

    public void TriggerAchievement(string identifier) {
        // test identifier: "ACH_TEST"
        Achievement a = new Achievement(identifier);
        a.Trigger();
        // foreach(Steamworks.Data.Achievement a in Steamworks.SteamUserStats.Achievements) {
        //     Debug.Log(a.Identifier);
        // }
        // // "ACH_TEST" for test
        // var achievement = (Steamworks.Data.Achievement)Steamworks.SteamUserStats.Achievements.Where(a => a.Identifier == identifier).First();
        // achievement.Trigger();
    }
}
