using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingBotBossWinDialogue : MonoBehaviour
{
    public GameObject playerDialogue;
    public TypewriterText playerText1;
    public TypewriterText playerText2;
    public TypewriterText playerText3;
    public TypewriterText bossText1;
    public TypewriterText bossText2;
    public TypewriterText bossText3;
    public TypewriterText bossText4;
    public GameObject bossDialogue;
    private BattleEnemyManager bem;
    private MusicManager musicManager;
    public GameObject clickContinuePlayer;
    public GameObject clickContinueBoss;
    public bool isFinished = false;
    private bool clicked = false;
    private bool waitForClick = false;
    // Start is called before the first frame update
    void Start()
    {
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
        bem = GameObject.Find("BattleEnemyManager").GetComponent<BattleEnemyManager>();
        bem.battleEnemies[0].nextAction.SetActive(false);
        StartCoroutine(TimedDialogue());
    }

    void Update() {
        if(waitForClick) {
            if(Input.GetMouseButtonDown(0)) {
                clicked = true;
            }
            else if(Input.GetMouseButtonUp(0)) {
                clicked = false;
            }
        }
    }

    private IEnumerator TimedDialogue() {

        StartCoroutine(musicManager.FadeTracksInOut(2.0f, musicManager.bossBattle, musicManager.kingbotWin));
        
        bossDialogue.SetActive(true);
        yield return new WaitUntil(() => bossText1.doneTyping);
        yield return new WaitForSeconds(2.5f);

        playerDialogue.SetActive(true);
        yield return new WaitUntil(() => playerText1.doneTyping);
        yield return new WaitForSeconds(1.0f);

        // wait click
        clickContinuePlayer.SetActive(true);
        waitForClick = true;
        yield return new WaitUntil(() => clicked);
        waitForClick = false;
        clickContinuePlayer.SetActive(false);


        bossText1.gameObject.SetActive(false);
        bossText2.gameObject.SetActive(true);
        yield return new WaitUntil(() => bossText2.doneTyping);
        yield return new WaitForSeconds(1.0f);

        // boss laugh animations
        ((EnemyAnimator)bem.battleEnemies[0].characterAnimator).LaughAnimation();
        yield return new WaitForSeconds(1.0f);
        
        playerText1.gameObject.SetActive(false);
        playerText2.gameObject.SetActive(true);
        yield return new WaitUntil(() => playerText2.doneTyping);

        // wait click
        clickContinuePlayer.SetActive(true);
        
        waitForClick = true;
        yield return new WaitUntil(() => clicked);
        waitForClick = false;
        
        clickContinuePlayer.SetActive(false);
        
        bossText2.gameObject.SetActive(false);
        bossText3.gameObject.SetActive(true);
        yield return new WaitUntil(() => bossText3.doneTyping);
        yield return new WaitForSeconds(4.0f);

        playerText2.gameObject.SetActive(false);
        playerText3.gameObject.SetActive(true);
        yield return new WaitUntil(() => playerText3.doneTyping);

        // wait click
        clickContinuePlayer.SetActive(true);
        
        waitForClick = true;
        yield return new WaitUntil(() => clicked);
        waitForClick = false;
        
        clickContinuePlayer.SetActive(false);

        bossText3.gameObject.SetActive(false);
        bossText4.gameObject.SetActive(true);
        yield return new WaitUntil(() => bossText4.doneTyping);
        ((EnemyAnimator)bem.battleEnemies[0].characterAnimator).animator.SetTrigger("Wave");

        // wait click
        clickContinueBoss.SetActive(true);
        
        waitForClick = true;
        yield return new WaitUntil(() => clicked);
        waitForClick = false;
        
        clickContinueBoss.SetActive(false);
        
        playerDialogue.SetActive(false);
        bossDialogue.SetActive(false);
        isFinished = true;
    }
}
