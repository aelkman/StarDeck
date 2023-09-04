using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    public GameObject blockHUD;
    public PlayerStats playerStats;
    public BlockText blockText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerStats.getBlock() <= 0) {
            blockHUD.SetActive(false);
        }
        else {
            blockText.setText(playerStats.getBlock().ToString());
            blockHUD.SetActive(true);
        }
    }
}
