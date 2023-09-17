using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapNode : OutlineHoverer
{
    public List<int> childrenNodes;
    public List<int> parentNodes;
    public MapManager mapManager;
    public int instanceId;
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
    }
}
