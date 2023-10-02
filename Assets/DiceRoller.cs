using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 startingPos;
    // public string UpperSideTxt;
    public TextMeshProUGUI UpperSideTxt;
    public Vector3Int DirectionValues;
    private Vector3Int OpposingDirectionValues;
    private float timeElapsed;
    public TextMeshProUGUI rerollCountText;
    private int rerollRemaining = 2;
    public Button rerollButton;
    public Button continueButton;
    private bool isRollFinished;
    public float upwardForce = 20f;
    public MapManager mapManager;
    public GameObject diceContainer;
    public string eventName;
    public MeshRenderer diceRenderer;

    List<string> FaceRepresent = new List<string>() {"", "Mini-Boss", "Enemy", "Event", "Shop", "Event", "Chest"};
    // Start is called before the first frame update
    void OnEnable()
    {
        isRollFinished = false;
        if(eventName == "Unknown") {
            diceRenderer.material = Resources.Load<Material>("Dice Material Unknown Event");
        }
        else if(eventName == "Chest") {
            diceRenderer.material = Resources.Load<Material>("Dice Material Chest");
            FaceRepresent = new List<string>() {"", "Chest", "Chest", "Enemy", "Enemy", "Chest", "Chest"};
        }
        else {
            diceRenderer.material = Resources.Load<Material>("Dice Material Unknown Event");
        }
        rerollCountText.text = rerollRemaining.ToString();
        continueButton.interactable = false;
        timeElapsed = 0;
        OpposingDirectionValues = 7 * Vector3Int.one - DirectionValues;
        rb = GetComponent<Rigidbody>();
        startingPos = transform.localPosition;
        DiceRoll();
    }

    private void DiceRoll()
    {
        var vec = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        vec.Normalize();
        // Gives the Asteroid a random rotation
        rb.AddTorque(vec * 100.0f); // where 1.0 is rotation speed
        transform.rotation = Quaternion.LookRotation(Random.onUnitSphere, Random.onUnitSphere);
        rb.AddForce(new Vector3(0, 0, -1 * upwardForce));
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if  (transform.hasChanged)
        {
            if (  Vector3.Cross(-1 * Vector3.forward, transform.right).magnitude < 0.5f) //x axis a.b.sin theta <45
          //if ((int) Vector3.Cross(Vector3.up, transform.right).magnitude == 0) //Previously
            {
                if (Vector3.Dot(-1 * Vector3.forward, transform.right) > 0)
                {
                    UpperSideTxt.text = FaceRepresent[DirectionValues.x];
                }
                else
                {
                    UpperSideTxt.text = FaceRepresent[OpposingDirectionValues.x];
                }
            }
            else if ( Vector3.Cross(-1 * Vector3.forward, transform.up).magnitude <0.5f) //y axis
            {
                if (Vector3.Dot(-1 * Vector3.forward, transform.up) > 0)
                {
                    UpperSideTxt.text = FaceRepresent[DirectionValues.z];
                }
                else
                {
                    UpperSideTxt.text = FaceRepresent[OpposingDirectionValues.z];
                }
            }
            else if ( Vector3.Cross(-1 * Vector3.forward, transform.forward).magnitude <0.5f) //y axis
            {
                if (Vector3.Dot(-1 * Vector3.forward, transform.forward) > 0)
                {
                    UpperSideTxt.text = FaceRepresent[DirectionValues.y];
                }
                else
                {
                    UpperSideTxt.text = FaceRepresent[OpposingDirectionValues.y];
                }
            }

            // Debug.Log("top side: " + UpperSideTxt);
            transform.hasChanged = false;
            timeElapsed = 0;
        }
        else {
            if (timeElapsed > 0.2f && !isRollFinished) {
                Debug.Log("roll finished!");
                isRollFinished = true;
                continueButton.interactable = true;
                rerollButton.interactable = true;
            }
        }
    }

    public void ThrowDice() {
        isRollFinished = false;
        continueButton.interactable = false;
        rerollButton.interactable = false;
        rerollRemaining -= 1;
        rerollCountText.text = rerollRemaining.ToString();
        // if(rerollRemaining == 0) {
        //     rerollButton.interactable = false;
        // }
        transform.localPosition = startingPos;

        DiceRoll();
    }

    public void ContinueClick() {
        if(eventName == "Chest") {
            if(UpperSideTxt.text == "Enemy") {
                MainManager.Instance.currentNode.enemies.Add("Chest");
                foreach(string e in MainManager.Instance.currentNode.enemies) {
                    Debug.Log("enemy: " + e);
                }
            }
        }
        Debug.Log("continue click: " + UpperSideTxt.text + ", " + eventName);
        diceContainer.SetActive(false);
        mapManager.LoadNextLevel(UpperSideTxt.text);
    }
}
