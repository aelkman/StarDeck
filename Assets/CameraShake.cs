using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class CameraShake : MonoBehaviour
{
    public bool start = false;
    // public AnimationCurve curve;
    // public float duration = 1f;
    // private bool isShaking = false;
    // private Vector3 initialPos;
    // public float pendingShakeDuration;
    // [Range(0f,20f)]
    // public float intensity;
    public float magnitude = 4f;
    public float roughness = 4f;
    public float fadeInTime = 0.1f;
    public float fadeOutTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        // initialPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (start) {
            start = false;
            // CameraShaker.Instance.cameraShakeInstances.Clear();
            CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
            // StartCoroutine(Shaking());
        }
    }

    public void StartShake() {
        start = true;
    }
}
