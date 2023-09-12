using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHUD : MonoBehaviour
{
    public GameObject blockHUD;
    public BattleEnemyContainer battleEnemy;
    public GameObject blockSprite;
    public BlockText blockText;
    private SpriteRenderer spriteRenderer;
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

        if(battleEnemy.getBlock() <= 0) {
            blockHUD.SetActive(false);
        }
        else {
            blockText.setText(battleEnemy.getBlock().ToString());
            blockHUD.SetActive(true);
        }
    }

    public void ActivateBlockUI() {
        blockText.setText(battleEnemy.getBlock().ToString());
        blockHUD.SetActive(true);
    }
}

