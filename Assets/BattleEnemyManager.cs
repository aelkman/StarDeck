using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnemyManager : MonoBehaviour
{
    public BattleEnemyContainer prefab;
    public GameObject animatorPrefab;
    public List<string> enemyNames;
    public BattleManager battleManager;
    public HandManager handManager;
    public List<BattleEnemyContainer> battleEnemies;
    public List<BattleEnemyContainer> battleEnemiesStarting;
    public bool isInitialized = false;

    void Awake() {
        if (MainManager.Instance != null) {
            MapNode currentNode = MainManager.Instance.currentNode;
            Debug.Log("currentNode enemies: " + currentNode.enemies.Count);
            enemyNames = currentNode.enemies;
        }
        else {
            Debug.Log("Main Manager not found!");
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
            Vector3 iceStacksPos = battleEnemyContainer.iceStacks.transform.localPosition;
            battleEnemyContainer.iceStacks.transform.localPosition = new Vector3(iceStacksPos.x, iceStacksPos.y + battleEnemy.nextMoveYOffset, iceStacksPos.z);
            battleEnemies.Add(battleEnemyContainer);
            battleEnemiesStarting.Add(battleEnemyContainer);
            // battleEnemyContainer.rectTransform.localScale = new Vector3(0.21334f, 0.21334f, 0.21334f);
            // battleEnemyContainer

        }
        isInitialized = true;
    }

    // Start is called before the first frame update
    void Start()
    {

        // try to get enemyNames from GameManager, if it doesn't exist then use a default for testing
    }

    public List<BattleEnemyContainer> GetEnemies() {
        return battleEnemies;
    }

    public void EnemyDeath(BattleEnemyContainer battleEnemy) {
        battleEnemies.Remove(battleEnemy);
        if(battleEnemies.Count == 0) {
            // fix bug for no hit audio on hammer finish
            // if(handManager.lastCard.type == "Hammer") {
            //     AudioManager.Instance.PlayHammerAudio();
            // }
            StartCoroutine(DelayWin(1.5f));
        }
        battleManager.RemoveEnemyActions(battleEnemy);
        battleEnemy.nextAction.SetActive(false);
        battleEnemy.characterHUD.SetActive(false);
    }

    private IEnumerator DelayWin(float time) {
        yield return new WaitForSeconds(0.5f);
        battleManager.playerStats.playerAnimator.HipThrust();
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
