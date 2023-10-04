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
    private SpriteRenderer spriteRenderer;
    public GameObject damageUp;
    public VulneratibiltyHoverDescription vhd;
    private bool isGlowUp = true;
    private float fade = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = blockSprite.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
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

        if(baseCharacterInfo.getBlock() <= 0) {
            blockHUD.SetActive(false);
        }
        else {
            blockText.setText(baseCharacterInfo.getBlock().ToString());
            blockHUD.SetActive(true);
        }

        if(baseCharacterInfo.atkMod > 0) {
            damageUp.SetActive(true);
        }
        else {
            damageUp.SetActive(false);
        }

        if(baseCharacterInfo.GetVuln() > 0) {
            vulnDisplay.SetActive(true);
            vhd.SetText(baseCharacterInfo.GetVuln());
        }
        else {
            vulnDisplay.SetActive(false);
        }

        if(baseCharacterInfo.blind > 0) {
            blindDisplay.SetActive(true);
        }
        else {
            blindDisplay.SetActive(false);
        }
    }
}
