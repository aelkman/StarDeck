using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnemyManager : MonoBehaviour
{
    public BattleEnemyContainer prefab;
    private List<BattleEnemyContainer> battleEnemies;
    private List<GameObject> enemyInstances;
    public List<string> enemyNames;
    // Start is called before the first frame update
    void Start()
    {
        battleEnemies = new List<BattleEnemyContainer>();
        
        // create enemy grid based on count of enemies
        foreach(string enemyName in enemyNames) {
            BattleEnemyContainer battleEnemyContainer = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            battleEnemyContainer.transform.SetParent(this.transform, false);
            // get size of the enemy
            BattleEnemy battleEnemy =  Resources.Load<BattleEnemy>("BattleEnemies/" + enemyName);
            battleEnemyContainer.battleEnemy = battleEnemy;
            battleEnemyContainer.Setup();
            // battleEnemyContainer.rectTransform.localScale = new Vector3(0.21334f, 0.21334f, 0.21334f);
            // battleEnemyContainer

        }

        // try to get enemyNames from GameManager, if it doesn't exist then use a default for testing
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
