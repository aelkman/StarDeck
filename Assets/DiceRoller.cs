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
    private bool isFirstRun = true;
    public AudioSource diceHit;

    List<string> FaceRepresent;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(isFirstRun) {
            startingPos = transform.localPosition;
            isFirstRun = false;
        }
        isRollFinished = false;
        if(eventName == "Unknown") {
            diceRenderer.material = Resources.Load<Material>("Dice Material Unknown Event");
            FaceRepresent = new List<string>() {"", "Mini-Boss", "Enemy", "Event", "Shop", "Event", "Chest"};
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
        rerollButton.interactable = false;
        timeElapsed = 0;
        OpposingDirectionValues = 7 * Vector3Int.one - DirectionValues;
        rb = GetComponent<Rigidbody>();
        DiceRoll();
    }

    void Start() {
    }

    private void DiceRoll()
    {
        transform.localPosition = startingPos;
        var vec = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        vec.Normalize();
        // Gives the Asteroid a random rotation
        rb.AddTorque(vec * 100.0f); // where 1.0 is rotation speed

        // commented below for possible bug fix
        // transform.rotation = Quaternion.LookRotation(Random.onUnitSphere, Random.onUnitSphere);
        rb.AddForce(new Vector3(0, 0, -1 * upwardForce));
        timeElapsed = 0;
        isRollFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if  (transform.hasChanged)
        {
            if (  Vector3.Cross(-1 * Vector3.forward, transform.right).magnitude < 0.5f) //x axis a.b.sin theta <45
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
        }
        else {
            if (timeElapsed > 0.05f && (rb.velocity.magnitude < 0.1) && (rb.angularVelocity.magnitude < 0.1)) {
                // Debug.Log("roll finished!");
                isRollFinished = true;
                continueButton.interactable = true;
                if(UpperSideTxt.text == "Chest" && eventName == "Unknown") {
                    continueButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Chest?";
                }
                if(rerollRemaining > 0) {
                    rerollButton.interactable = true;
                }
            }
            // else {
            //     Debug.Log("not moving but! not finished, time elapsed: " + timeElapsed + " isRolleFinished: " + isRollFinished);
            // }
        }
    }

    public void ThrowDice() {
        continueButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
        AudioManager.Instance.PlayButtonPress();
        continueButton.interactable = false;
        rerollButton.interactable = false;
        rerollRemaining -= 1;
        rerollCountText.text = rerollRemaining.ToString();
        if(rerollRemaining <= 0) {
            rerollButton.interactable = false;
        }

        DiceRoll();
    }

    public void ContinueClick() {
        continueButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
        string nextLevel = UpperSideTxt.text;
        AudioManager.Instance.PlayButtonPress();
        if(eventName == "Chest") {
            if(UpperSideTxt.text == "Enemy") {
                MainManager.Instance.currentNode.enemies.Add("Chest");
                MainManager.Instance.currentNode.destinationName = "Enemy";
                foreach(string e in MainManager.Instance.currentNode.enemies) {
                    Debug.Log("enemy: " + e);
                }
            }
            else {
                nextLevel = "Chest";
            }
        }
        else if(eventName == "Unknown") {
            if(UpperSideTxt.text == "Enemy") {
                MainManager.Instance.currentNode.destinationName = "Enemy";
                MainManager.Instance.currentNode.GenerateEnemyGroup();
                foreach(string e in MainManager.Instance.currentNode.enemies) {
                    Debug.Log("enemy: " + e);
                }
            }
            else if(UpperSideTxt.text == "Mini-Boss") {
                MainManager.Instance.currentNode.destinationName = "Mini-Boss";
                MainManager.Instance.currentNode.GenerateBossGroup("MiniBoss");
                foreach(string e in MainManager.Instance.currentNode.enemies) {
                    Debug.Log("enemy: " + e);
                }
            }
        }
        Debug.Log("continue click: " + UpperSideTxt.text + ", " + eventName);
        diceContainer.SetActive(false);
        mapManager.destinationsClickable = true;
        mapManager.LoadNextLevel(nextLevel);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("collision dice!");
        diceHit.Stop();
        diceHit.volume = 1;
        diceHit.Play();
    }

    void OnCollisionStay(Collision collision)
    {
        // Debug.Log("collision stay dice!");
        var totalImpulse = Mathf.Abs(collision.impulse.x) + Mathf.Abs(collision.impulse.y) + Mathf.Abs(collision.impulse.z);
        var force = totalImpulse/Time.deltaTime;
        // Debug.Log("force is " + force);
        if(force > 8) {
            // Debug.Log("sitting force: " + force);
            diceHit.Stop();
            diceHit.volume = Mathf.Lerp(0, 1, force/33);
            diceHit.Play();
        }

    }
}
