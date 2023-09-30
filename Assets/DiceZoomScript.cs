using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceZoomScript : MonoBehaviour
{
    private Camera cam;
    private float minFov = 34f;
    private float maxFov = 56f;
    private float sensitivity = 20f;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        var fov = cam.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * -sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        cam.fieldOfView = fov;
    }
}
