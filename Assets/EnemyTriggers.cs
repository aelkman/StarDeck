using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggers : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other)  {
        Debug.Log("enemy collision!");
    }
}
