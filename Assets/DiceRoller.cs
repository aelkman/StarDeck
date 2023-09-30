using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 startingPos;
    public string UpperSideTxt;
    public Vector3Int DirectionValues;
    private Vector3Int OpposingDirectionValues;

    readonly List<string> FaceRepresent = new List<string>() {"", "Enemy", "Enemy", "Event", "Shop", "Event", "Chest"};
    // Start is called before the first frame update
    void Start()
    {
        OpposingDirectionValues = 7 * Vector3Int.one - DirectionValues;
        rb = GetComponent<Rigidbody>();
        startingPos = transform.localPosition;
        var vec = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        vec.Normalize();
        // Gives the Asteroid a random rotation
        rb.AddTorque(vec * 100.0f); // where 1.0 is rotation speed
        transform.rotation = Quaternion.LookRotation(Random.onUnitSphere, Random.onUnitSphere);
    }

    // Update is called once per frame
    void Update()
    {
        if  (transform.hasChanged)
        {
            if (  Vector3.Cross(-1 * Vector3.forward, transform.right).magnitude < 0.5f) //x axis a.b.sin theta <45
          //if ((int) Vector3.Cross(Vector3.up, transform.right).magnitude == 0) //Previously
            {
                if (Vector3.Dot(-1 * Vector3.forward, transform.right) > 0)
                {
                    UpperSideTxt = FaceRepresent[DirectionValues.x];
                }
                else
                {
                    UpperSideTxt = FaceRepresent[OpposingDirectionValues.x];
                }
            }
            else if ( Vector3.Cross(-1 * Vector3.forward, transform.up).magnitude <0.5f) //y axis
            {
                if (Vector3.Dot(-1 * Vector3.forward, transform.up) > 0)
                {
                    UpperSideTxt = FaceRepresent[DirectionValues.z];
                }
                else
                {
                    UpperSideTxt = FaceRepresent[OpposingDirectionValues.z];
                }
            }
            else if ( Vector3.Cross(-1 * Vector3.forward, transform.forward).magnitude <0.5f) //y axis
            {
                if (Vector3.Dot(-1 * Vector3.forward, transform.forward) > 0)
                {
                    UpperSideTxt = FaceRepresent[DirectionValues.y];
                }
                else
                {
                    UpperSideTxt = FaceRepresent[OpposingDirectionValues.y];
                }
            }

            Debug.Log("top side: " + UpperSideTxt);
            transform.hasChanged = false;
        }
    }

    public void ThrowDice() {
        transform.localPosition = startingPos;
        var vec = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        vec.Normalize();
        // Gives the Asteroid a random rotation
        rb.AddTorque(vec * 100.0f); // where 1.0 is rotation speed
        transform.rotation = Quaternion.LookRotation(Random.onUnitSphere, Random.onUnitSphere);
    }
}
