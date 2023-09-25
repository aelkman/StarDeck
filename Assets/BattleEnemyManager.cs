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
    public bool isInitialized = false;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("MainManager") != null) {
            MapNode currentNode = GameObject.Find("MainManager").GetComponent<MainManager>().currentNode;
            enemyNames = currentNode.enemies;
        }
        if(enemyNames.Count == 0) {
            enemyNames = new List<string>() {"MiniBot"};
        }
        battleEnemies = new List<BattleEnemyContainer>();
        
        // create enemy grid based on count of enemies
        for (int i = 0; i < enemyNames.Count; i++) {
            string enemyName = enemyNames[i];
            GameObject newAnimator = Instantiate(animatorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newAnimator.transform.SetParent(this.transform, false);
            // now assign the proper animator for the enemy
            Animator enemyAnimator = newAnimator.gameObject.GetComponent<Animator>();
            enemyAnimator.runtimeAnimatorController = Resources.Load("BattleEnemies/" + enemyName + " Animator") as RuntimeAnimatorController;
    
            BattleEnemyContainer battleEnemyContainer = newAnimator.GetComponentInChildren<BattleEnemyContainer>();
            BattleEnemy battleEnemy =  Resources.Load<BattleEnemy>("BattleEnemies/" + enemyName + " BE");
            battleEnemyContainer.battleEnemy = battleEnemy;
            // now, instantiate the prefab GO of the enemy sprites rig
            GameObject enemyGO = Resources.Load<GameObject>("BattleEnemies/" + enemyName + " Prefab");
            GameObject enemyGOInstance = Instantiate(enemyGO, battleEnemyContainer.transform);
            enemyGOInstance.transform.localPosition = new Vector3(enemyGOInstance.transform.localPosition.x,  enemyGOInstance.transform.localPosition.y + battleEnemy.yOffset, enemyGOInstance.transform.localPosition.z);
            battleEnemyContainer.enemyPrefabInstance = enemyGOInstance;
            // now restart the animator
            enemyAnimator.Rebind();
            enemyAnimator.Update(0f);
            if (enemyNames.Count > 1) {
                float alignResult = i / (enemyNames.Count - 1.0f);
                float newXPos = Mathf.Lerp(-battleEnemy.xOffset/2 * (enemyNames.Count-1), battleEnemy.xOffset/2 * (enemyNames.Count-1), alignResult);
                Vector3 newPos = new Vector3(newXPos, battleEnemyContainer.transform.localPosition.y, battleEnemyContainer.transform.localPosition.z);
                battleEnemyContainer.transform.localPosition = newPos;
            }
            Vector3 nextActionPos = battleEnemyContainer.nextAction.transform.localPosition;
            battleEnemyContainer.nextAction.transform.localPosition = new Vector3(nextActionPos.x, nextActionPos.y + battleEnemy.nextMoveYOffset, nextActionPos.z);
            battleEnemies.Add(battleEnemyContainer);
            // battleEnemyContainer.rectTransform.localScale = new Vector3(0.21334f, 0.21334f, 0.21334f);
            // battleEnemyContainer

        }
        isInitialized = true;
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
            battleEnemy.nextAction.SetActive(false);
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
