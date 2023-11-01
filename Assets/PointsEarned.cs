using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

public class PointsEarned : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool mouse_over;
    // public int coinsEarned;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI pointsText;
    public BattleManager battleManager;
    public BattleEnemyManager BEM;
    public GameObject cardsRewards;
    public GameObject coinsReward;
    public GameObject items;
    public PotionRewardButton potionRewardButton;
    public ArtifactDisplay artifactDisplay;
    public CoinsEarned coinsEarned;
    public List<GameObject> rewards;
    private float noDamageMultiplier = 1.5f;
    private int enemyCount;
    private int miniBossCount;
    private int bossCount;
    private int enemyValue = 10;
    private int miniBossValue = 50;
    private int bossValue = 100;
    public float itemDisplacement = 200f;

    // Start is called before the first frame update
    void Start()
    {
        rewards = new List<GameObject>();
        rewards.Add(coinsReward);
        if(WillRewardHavePotion()) {
            potionRewardButton.gameObject.SetActive(true);
            rewards.Add(potionRewardButton.gameObject);
        }
        else {
            potionRewardButton.gameObject.SetActive(false);
        }

        if(MainManager.Instance.currentNode.destinationName == "Mini-Boss") {
            artifactDisplay.gameObject.SetActive(true);
            rewards.Add(artifactDisplay.gameObject);
        }
        else {
            artifactDisplay.gameObject.SetActive(false);
        }

        PlaceRewards();
        cardsRewards.SetActive(false);
    }

    private void PlaceRewards() {
        for(int i = 0; i < rewards.Count; i++) {
            var reward = rewards[i];
            reward.transform.localPosition = new Vector3(i*itemDisplacement, 0f, 0f);
        }
        var itemsPos = items.transform.localPosition;
        items.transform.localPosition = new Vector3((rewards.Count-1) * -itemDisplacement/2, itemsPos.y, 0f);
    }

    private bool WillRewardHavePotion() {
        int chance = 0;
        if(MainManager.Instance.currentNode.destinationName == "Enemy") {
            chance += 1;
        }
        else if(MainManager.Instance.currentNode.destinationName == "Mini-Boss"){
            chance += 5;
        }
        else if(MainManager.Instance.currentNode.destinationName == "Boss"){
            chance += 10;
        }
        return Random.Range(0, 10) < chance;
    }

    // Update is called once per frame
    void Update()
    {
        // if (mouse_over) {
        //     if (Input.GetMouseButtonUp(0)) {
        //         gameObject.SetActive(false);
        //         MainManager.Instance.coinCount += coinsEarned;
        //     }
        // }
    }

    public void SetData() {
        enemyCount = 0;
        miniBossCount = 0;
        bossCount = 0;

        foreach(BattleEnemyContainer bec in BEM.battleEnemiesStarting) {
            if(bec.battleEnemy.isBoss) {
                bossCount += 1;
            }
            else if(bec.battleEnemy.isMiniBoss) {
                miniBossCount += 1;
            }
            else {
                enemyCount += 1;
            }
        }

        pointsText.text = "";

        if (enemyCount > 0) {
            string descriptor = "Enemies";
            if (enemyCount == 1) {
                descriptor = "Enemy";
            }
            pointsText.text += enemyCount + " " + descriptor + "<br>";
        }
        if (miniBossCount > 0) {
            string descriptor = "Mini-Bosses";
            if (miniBossCount == 1) {
                descriptor = "Mini-Boss";
            }
            pointsText.text += miniBossCount + " "  + descriptor + "<br>";
        }
        if (bossCount > 0) {
            string descriptor = "Bosses";
            if (bossCount == 1) {
                descriptor = "Boss";
            }
            pointsText.text += bossCount + " "  + descriptor + "<br>";
        }

        int enemyCoins = (enemyCount * enemyValue) + (miniBossCount * miniBossValue) + (bossCount * bossValue);

        if(battleManager.noDamageTaken) {
            coinsEarned.coinsEarned = (int)(enemyCoins * noDamageMultiplier);
            pointsText.text += "Perfect Bonus " + noDamageMultiplier + "x<br>";
        }
        else {
            coinsEarned.coinsEarned = enemyCoins;
        }
        pointsText.text += "<br>" + coinsEarned.coinsEarned + " gold earned";
        coinsText.text = coinsEarned.coinsEarned.ToString();
    }

    public void OnPointerEnter(PointerEventData pointerEventData) {
        mouse_over = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        mouse_over = false;
    }

    public void ClickContinue() {
        AudioManager.Instance.PlayButtonPress();
        gameObject.SetActive(false);
        cardsRewards.SetActive(true);
    }

}
