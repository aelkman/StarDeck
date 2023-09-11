using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnemyManager : MonoBehaviour
{
    public BattleEnemyContainer prefab;
    public GameObject animatorPrefab;
    public List<string> enemyNames;
    public BattleManager battleManager;
    private List<BattleEnemyContainer> battleEnemies;
    private List<GameObject> enemyInstances;
    // Start is called before the first frame update
    void Start()
    {
        battleEnemies = new List<BattleEnemyContainer>();
        
        // create enemy grid based on count of enemies
        for (int i = 0; i < enemyNames.Count; i++) {
            string enemyName = enemyNames[i];
            GameObject newAnimator = Instantiate(animatorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newAnimator.transform.SetParent(this.transform, false);
            // BattleEnemyContainer battleEnemyContainer = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            BattleEnemyContainer battleEnemyContainer = newAnimator.GetComponentInChildren<BattleEnemyContainer>();
            // battleEnemyContainer.transform.SetParent(this.transform, false);
            // get size of the enemy
            BattleEnemy battleEnemy =  Resources.Load<BattleEnemy>("BattleEnemies/" + enemyName);
            battleEnemyContainer.battleEnemy = battleEnemy;
            if (enemyNames.Count > 1) {
                float alignResult = i / (enemyNames.Count - 1.0f);
                float newXPos = Mathf.Lerp(-battleEnemy.xOffset/2 * (enemyNames.Count-1), battleEnemy.xOffset/2 * (enemyNames.Count-1), alignResult);
                Vector3 newPos = new Vector3(newXPos, battleEnemyContainer.transform.localPosition.y, battleEnemyContainer.transform.localPosition.z);
                battleEnemyContainer.transform.localPosition = newPos;
            }
            battleEnemies.Add(battleEnemyContainer);
            // battleEnemyContainer.rectTransform.localScale = new Vector3(0.21334f, 0.21334f, 0.21334f);
            // battleEnemyContainer

        }

        // try to get enemyNames from GameManager, if it doesn't exist then use a default for testing
    }

    public List<BattleEnemyContainer> GetEnemies() {
        return battleEnemies;
    }

    public void EnemyDeath(BattleEnemyContainer battleEnemy) {
        battleEnemies.Remove(battleEnemy);
        if(battleEnemies.Count == 0) {
            StartCoroutine(DelayWin(2.0f));
        }
        else {
            // if battle isn't over, remove the pending actions from the dead enemy
            battleManager.RemoveEnemyActions(battleEnemy);
        }
    }

    private IEnumerator DelayWin(float time) {
        yield return new WaitForSeconds(time);
        battleManager.BattleWin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<BattleEnemyContainer> GetBattleEnemies() {
        return battleEnemies;
    }
}
