using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Stats stats;
    public GameDataManager gdm;
    // Start is called before the first frame update
    void Start()
    {
        // for now, gameData is ONLY the Stats object
        stats = gdm.gameData;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
