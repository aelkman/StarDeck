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

    // IEnumerator Shaking() {
    //     isShaking = true;

    //     var startTime = Time.realtimeSinceStartup;
    //     var randomPoint = new Vector3(Random.Range(-1f, 1f) * intensity, Random.Range(-1f, 1f) * intensity, initialPos.z);
    //     while(Time.realtimeSinceStartup < startTime + pendingShakeDuration/2) {
    //         float fract = Time.realtimeSinceStartup/startTime + pendingShakeDuration/2;
    //         transform.localPosition = new Vector3(Mathf.Lerp(initialPos.x, randomPoint.x, Mathf.SmoothStep(0,1,fract)),
    //                                             Mathf.Lerp(initialPos.y, randomPoint.y, Mathf.SmoothStep(0,1,fract)),
    //                                             initialPos.z)      ;
    //         yield return null;
    //     }
    //     startTime = Time.realtimeSinceStartup;
    //     while(Time.realtimeSinceStartup < startTime + pendingShakeDuration/2) {
    //         float fract = Time.realtimeSinceStartup/startTime + pendingShakeDuration/2;
    //         transform.localPosition = new Vector3(Mathf.Lerp(randomPoint.x, initialPos.x, Mathf.SmoothStep(0,1,fract)),
    //                                             Mathf.Lerp(randomPoint.y, initialPos.y, Mathf.SmoothStep(0,1,fract)),
    //                                             initialPos.z)      ;
    //         yield return null;
    //     }

    //     // pendingShakeDuration = 0f;
    //     transform.localPosition = initialPos;
    //     isShaking = false;
    // }

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
