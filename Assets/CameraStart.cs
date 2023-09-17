using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.transparencySortAxis = new Vector3(0,0,-1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
