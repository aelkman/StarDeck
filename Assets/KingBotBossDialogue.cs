using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingBotBossDialogue : MonoBehaviour
{
    public GameObject playerDialogue;
    public TypewriterText playerText1;
    public TypewriterText playerText2;
    public TypewriterText bossText1;
    public TypewriterText bossText2;
    public GameObject bossDialogue;
    private BattleEnemyManager bem;
    private MusicManager musicManager;
    private float fadeInOutTime = 2.0f;
    public 
    // Start is called before the first frame update
    void Start()
    {
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
        bem = GameObject.Find("BattleEnemyManager").GetComponent<BattleEnemyManager>();
        bem.battleEnemies[0].nextAction.SetActive(false);
        StartCoroutine(TimedDialogue());
    }

    private IEnumerator TimedDialogue() {

        playerDialogue.SetActive(true);
        yield return new WaitUntil(() => playerText1.doneTyping);
        yield return new WaitForSeconds(3.0f);
        bossDialogue.SetActive(true);
        yield return new WaitUntil(() => bossText1.doneTyping);
        yield return new WaitForSeconds(4.0f);
        // boss laugh animations
        ((EnemyAnimator)bem.battleEnemies[0].characterAnimator).LaughAnimation();
        yield return new WaitForSeconds(2.5f);
        playerText1.gameObject.SetActive(false);
        playerText2.gameObject.SetActive(true);
        yield return new WaitUntil(() => playerText2.doneTyping);
        StartCoroutine(musicManager.FadeTracksInOut(fadeInOutTime, musicManager.kingbotIntro, musicManager.bossBattle));
        yield return new WaitForSeconds(fadeInOutTime + 0.01f);
        playerDialogue.SetActive(false);
        bossDialogue.SetActive(false);
        // yield return new WaitForSeconds(fadeInOutTime + 0.01f);
        bem.battleEnemies[0].nextAction.SetActive(true);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
