using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapNode : MonoBehaviour
{
    public List<MapNode> childrenNodes;
    public List<MapNode> parentNodes;
    public MapManager mapManager;
    public GameObject destination;
    public List<string> enemies;
    public DestinationsController destinationsController;
    public int instanceId;
    private float fade = 0;
    private bool isGlowUp = true;
    public int level;
    public SpriteRenderer spriteRenderer;
    public string destinationName;
    public Animator destinationAnimator;
    public bool isBoss = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForLoad());
        instanceId = gameObject.GetInstanceID();
        childrenNodes = new List<MapNode>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator WaitForLoad() {
        yield return new WaitForSeconds(0.27f);
        level = transform.parent.GetComponent<TreeTraversal>().level;
        GameObject prefabInstance;

        if (transform.parent.GetComponent<TreeTraversal>().nextLevel == null || isBoss) {
                // this is a boss
                destinationName = "Boss";
                GenerateBossGroup("Boss");
                prefabInstance = Resources.Load<GameObject>("Map Destinations/Boss Icon Animator");
        }
        else {
            if (transform.GetComponent<MapNode>().parentNodes.Count != 0) {
                destinationName = destinationsController.AssignPopDestination();
            }
            else {
                // if it's the child node
                destinationName = "Enemy";
            }
            if (destinationName == "Enemy")
            {
                GenerateEnemyGroup();

                prefabInstance = Resources.Load<GameObject>("Map Destinations/Enemy Icon Animator");
            }
            else if (destinationName == "Mini-Boss")
            {
                GenerateBossGroup("MiniBoss");
                prefabInstance = Resources.Load<GameObject>("Map Destinations/Super Enemy Icon Animator");
            }
            else if (destinationName == "Unknown") {
                prefabInstance = Resources.Load<GameObject>("Map Destinations/Unknown Icon Animator");
            }
            else if (destinationName == "Shop") {
                prefabInstance = Resources.Load<GameObject>("Map Destinations/Shop Icon Animator");
            }
            else if (destinationName == "Event") {
                prefabInstance = Resources.Load<GameObject>("Map Destinations/Event Icon Animator");
            }
            else if (destinationName == "Chest") {
                prefabInstance = Resources.Load<GameObject>("Map Destinations/Chest Icon Animator");
            }
            else {
                prefabInstance = null;
            }
        }

        // dont instantiate a prefab for the root node, we're already there
        if (transform.GetComponent<MapNode>().parentNodes.Count != 0) {
            destination = Instantiate(prefabInstance, transform);
            var animator = destination.GetComponent<Animator>();
            destinationAnimator = animator;
            var currentNode = mapManager.mapArrow.GetComponent<MapArrow>().currentNode;
            if(currentNode.childrenNodes.Contains(this)) {
                animator.enabled = true;
            }
            else {
                animator.enabled = false;
            }
            prefabInstance.transform.localScale = new Vector3(1,1,1);
        }        
    }

    public void GenerateBossGroup(string folder)
    {
        enemies = new List<string>();
        EnemyGroup[] group = Resources.LoadAll<EnemyGroup>("BattleEnemies/Groups/Level 1/" + folder);

        // pick a random group
        int index = Random.Range(0, group.Length);
        var enemiesEnum = group[index].enemies;
        foreach (EnemiesEnum enemy in enemiesEnum)
        {
            enemies.Add(enemy.ToString());
        }
    }

    public void GenerateEnemyGroup()
    {
        enemies = new List<string>();
        // For now, these are hard-coded, will need to change later
        EnemyGroup[] group;
        if (level >= 0 && level <= 1)
        {
            // Easy groups
            group = Resources.LoadAll<EnemyGroup>("BattleEnemies/Groups/Level 1/Easy");
        }
        else if (level >= 2 && level <= 3)
        {
            // Easy groups
            group = Resources.LoadAll<EnemyGroup>("BattleEnemies/Groups/Level 1/Easy-2");
        }
        else if (level >= 4 && level <= 8)
        {
            // Medium groups
            group = Resources.LoadAll<EnemyGroup>("BattleEnemies/Groups/Level 1/Medium");
        }
        else
        {
            // Hard groups
            group = Resources.LoadAll<EnemyGroup>("BattleEnemies/Groups/Level 1/Hard");
        }

        // pick a random group
        int index = Random.Range(0, group.Length);
        var enemiesEnum = group[index].enemies;
        foreach (EnemiesEnum enemy in enemiesEnum)
        {
            enemies.Add(enemy.ToString());
        }
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonUp (0)) {
            Debug.Log("clicked node id: " + instanceId);
            mapManager.SetMovementSelection(this);
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
