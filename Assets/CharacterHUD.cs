using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHUD : MonoBehaviour
{
    public GameObject blockHUD;
    public BaseCharacterInfo baseCharacterInfo;
    public GameObject blockSprite;
    public BlockText blockText;
    public GameObject vulnDisplay;
    public GameObject blindDisplay;
    public GameObject weakDisplay;
    public GameObject tauntDisplay;
    public GameObject counterDisplay;
    public GameObject attackMod;
    public GameObject attackNegative;
    public GameObject attackPositive;
    public GameObject stunDisplay;
    public GameObject iceHUD;
    public HealthBar healthBar;
    private SpriteRenderer spriteRenderer;
    public StatusHoverDescription shd;
    private bool isGlowUp = true;
    private float fade = 0f;
    public HashSet<string> statuses;
    
    // Start is called before the first frame update
    void Start()
    {
        statuses = new HashSet<string>();
        spriteRenderer = blockSprite.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.slider.value = baseCharacterInfo.health;

        if (isGlowUp) {
            fade += Time.deltaTime * 2f;
        }
        else {
            fade -= Time.deltaTime * 2f;
        }
        if (fade >= 1f) { 
            isGlowUp = false;
        }
        else if (fade <= 0f) {
            isGlowUp = true;
        }
        spriteRenderer.material.SetFloat("_Transparency", fade);

        if(baseCharacterInfo.getBlock() > 0) {
            blockText.setText(baseCharacterInfo.getBlock().ToString());
            blockHUD.SetActive(true);
            shd.blockText.gameObject.SetActive(true);
            shd.blockText.text = "Blocking the next " + baseCharacterInfo.getBlock() + " damage";
            shd.isEmpty = false;
            statuses.Add("block");
        }
        else {
            blockHUD.SetActive(false);
            shd.blockText.gameObject.SetActive(false);
            statuses.Remove("block");
        }

        if(baseCharacterInfo.atkMod != 0) {
            attackMod.SetActive(true);
            if(baseCharacterInfo.atkMod > 0) {
                attackNegative.SetActive(false);
                attackPositive.SetActive(true);
            }
            else if(baseCharacterInfo.atkMod < 0) {
                attackNegative.SetActive(true);
                attackPositive.SetActive(false);
            }
            shd.attackText.gameObject.SetActive(true);
            string sign = "";
            if(baseCharacterInfo.atkMod > 0) {
                sign = "+";
            }
            shd.attackText.text = "Attack modifier of " + sign + baseCharacterInfo.atkMod + " damage";
            statuses.Add("attackMod");
        }
        else {
            attackMod.SetActive(false);
            shd.attackText.gameObject.SetActive(false);
            statuses.Remove("attackMod");
        }

        if(baseCharacterInfo.GetVuln() > 0) {
            vulnDisplay.SetActive(true);
            shd.vulnText.gameObject.SetActive(true);
            shd.vulnText.text = "Vulnerable for " + baseCharacterInfo.GetVuln() + " turns";
            shd.vulnText.text += "<br>Vulnerable targets take " + MainManager.Instance.vulnerableModifier + "X more damage";
            statuses.Add("vuln");
        }
        else {
            vulnDisplay.SetActive(false);
            shd.vulnText.gameObject.SetActive(false);
            statuses.Remove("vuln");
        }

        if(baseCharacterInfo.blind > 0) {
            blindDisplay.SetActive(true);
            shd.blindText.gameObject.SetActive(true);
            shd.blindText.text = "Blinded for " + baseCharacterInfo.blind + " turns";
            shd.blindText.text += "<br>Blinded characters have a 50% chance to miss any attacks";
            statuses.Add("blind");
        }
        else {
            blindDisplay.SetActive(false);
            shd.blindText.gameObject.SetActive(false);
            statuses.Remove("blind");
        }

        if(baseCharacterInfo.weak > 0) {
            weakDisplay.SetActive(true);
            shd.weakText.gameObject.SetActive(true);
            shd.weakText.text = "Weakened for " + baseCharacterInfo.weak + " turns";
            shd.weakText.text += "<br>Weakened characters deal " + (MainManager.Instance.weakenedModifier * 100) + "% less damage";
            statuses.Add("weak");
        }
        else {
            weakDisplay.SetActive(false);
            shd.weakText.gameObject.SetActive(false);
            statuses.Remove("weak");
        }

        if(baseCharacterInfo.isTaunter) {
            tauntDisplay.SetActive(true);
            shd.tauntText.gameObject.SetActive(true);
            shd.tauntText.text = "Taunting player, player must attack this enemy";
            statuses.Add("taunter");
        }
        else {
            tauntDisplay.SetActive(false);
            shd.tauntText.gameObject.SetActive(false);
            statuses.Remove("taunter");
        }

        if(baseCharacterInfo.frostStacks > 0) {
            iceHUD.SetActive(true);
        }
        else {
            iceHUD.SetActive(false);
        }

        if(baseCharacterInfo.stunnedTurns > 0) {
            stunDisplay.SetActive(true);
            shd.stunText.gameObject.SetActive(true);
            shd.stunText.text = "Stunned for " + baseCharacterInfo.stunnedTurns + " turns";
            statuses.Add("stunned");
        }
        else {
            stunDisplay.SetActive(false);
            shd.stunText.gameObject.SetActive(false);
            statuses.Remove("stunned");
        }

        if(baseCharacterInfo.counterQueue.Count() > 0) {
            counterDisplay.SetActive(true);
            shd.counterText.gameObject.SetActive(true);
            shd.counterText.text = "Countering next attack:";

            for(int i = 0; i < baseCharacterInfo.counterQueue.items.Count; i++) {
                shd.counterText.text += "<br>  " + (i + 1) + ") " +  CardUtils.DescriptionParser(baseCharacterInfo.counterQueue.items[i]);
            }
            statuses.Add("counter");
        }
        else {
            counterDisplay.SetActive(false);
            shd.counterText.gameObject.SetActive(false);
            statuses.Remove("counter");
        }


    }
}
