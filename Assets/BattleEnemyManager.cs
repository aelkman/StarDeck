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
            // now assign the proper animator for the enemy
            Animator enemyAnimator = newAnimator.gameObject.GetComponent<Animator>();
            switch(enemyName) {
                case "Rob":
                    enemyAnimator.runtimeAnimatorController = Resources.Load("BattleEnemies/Rob Animator") as RuntimeAnimatorController;
                    break;
                case "GoldBot":
                    enemyAnimator.runtimeAnimatorController = Resources.Load("BattleEnemies/GoldBot Animator") as RuntimeAnimatorController;
                    break;
                default:
                    break;
            }
            BattleEnemyContainer battleEnemyContainer = newAnimator.GetComponentInChildren<BattleEnemyContainer>();
            BattleEnemy battleEnemy =  Resources.Load<BattleEnemy>("BattleEnemies/" + enemyName + " BE");
            battleEnemyContainer.battleEnemy = battleEnemy;
            // now, instantiate the prefab GO of the enemy sprites rig
            GameObject enemyGO = Resources.Load<GameObject>("BattleEnemies/" + enemyName + " Prefab");
            GameObject enemyGOInstance = Instantiate(enemyGO, battleEnemyContainer.transform);
            // now restart the animator
            enemyAnimator.Rebind();
            enemyAnimator.Update(0f);
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
            StartCoroutine(DelayWin(1.5f));
        }
        else {
            // if battle isn't over, remove the pending actions from the dead enemy
            battleManager.RemoveEnemyActions(battleEnemy);
        }
    }

    private IEnumerator DelayWin(float time) {
        battleManager.playerStats.playerAnimator.ClapAnimation();
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
