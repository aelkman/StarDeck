using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnemyManager : MonoBehaviour
{
    public BattleEnemyContainer prefab;
    public List<string> enemyNames;
    private List<BattleEnemyContainer> battleEnemies;
    private List<GameObject> enemyInstances;
    // Start is called before the first frame update
    void Start()
    {
        battleEnemies = new List<BattleEnemyContainer>();
        
        // create enemy grid based on count of enemies
        for (int i = 0; i < enemyNames.Count; i++) {
            string enemyName = enemyNames[i];
            BattleEnemyContainer battleEnemyContainer = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            battleEnemyContainer.transform.SetParent(this.transform, false);
            // get size of the enemy
            BattleEnemy battleEnemy =  Resources.Load<BattleEnemy>("BattleEnemies/" + enemyName);
            battleEnemyContainer.battleEnemy = battleEnemy;
            if (enemyNames.Count > 1) {
                float alignResult = i / (enemyNames.Count - 1.0f);
                float newXPos = Mathf.Lerp(-battleEnemy.xOffset/2 * (enemyNames.Count-1), battleEnemy.xOffset/2 * (enemyNames.Count-1), alignResult);
                battleEnemyContainer.transform.localPosition = new Vector3(newXPos, battleEnemyContainer.transform.localPosition.y, battleEnemyContainer.transform.localPosition.z);
            }
            battleEnemyContainer.Setup();
            battleEnemies.Add(battleEnemyContainer);
            // battleEnemyContainer.rectTransform.localScale = new Vector3(0.21334f, 0.21334f, 0.21334f);
            // battleEnemyContainer

        }

        // try to get enemyNames from GameManager, if it doesn't exist then use a default for testing
    }

    public List<BattleEnemyContainer> GetEnemies() {
        return battleEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
