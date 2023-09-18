using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapNode : MonoBehaviour
{
    public List<int> childrenNodes;
    public List<int> parentNodes;
    public MapManager mapManager;
    public int instanceId;
    private float fade = 0;
    private bool isGlowUp = true;
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        instanceId = gameObject.GetInstanceID();
        childrenNodes = new List<int>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

   private void OnMouseOver() {
        if (Input.GetMouseButtonUp (0)) {
            Debug.Log("clicked node id: " + instanceId);
            mapManager.SetMovementSelection(instanceId, this);
        }
        
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
    }

    private void OnMouseExit() {
        // remove target from STM
        // STM.ClearTarget();
        // Debug.Log("cleared target to SingleTargetManager!");
        // if(!STM.targetLocked) {
        //     STM.SetTarget(null);
        // }
 

        fade = 0f;
        spriteRenderer.material.SetFloat("_Transparency", fade);
    }
}
