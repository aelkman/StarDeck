using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

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
    public Volume ppVolume;
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    public float vignetteFlashTime = 0.4f;
    public float vignetteMinIntensity = 0f;
    public float vignetteMaxIntensity = 0.48f;
    public float chromaticAbberationTime = 0.4f;
    public float chromaticAbberationMin = 0f;
    public float chromaticAbberationMax = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        // initialPos = transform.localPosition;
        ppVolume.profile.TryGet<Vignette>(out vignette);
        ppVolume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
    }

    // Update is called once per frame
    void Update()
    {
        if (start) {
            start = false;
            // CameraShaker.Instance.cameraShakeInstances.Clear();
            StartCoroutine(VignetteFlash(vignetteFlashTime));
            StartCoroutine(ChromaticAbberation(vignetteFlashTime));
            CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
            // StartCoroutine(Shaking());
        }
    }

    private IEnumerator VignetteFlash(float flashTime) {
        // vignette.intensity.value = vignetteMinIntensity;
        // // 30 frames per second
        // for(float i = vignetteMinIntensity; i < vignetteFlashTime/2; i += 0.033f) {
        //     vignette.intensity.value = Mathf.Lerp(vignetteMinIntensity, vignetteMaxIntensity, i/vignetteFlashTime/2);
        //     yield return new WaitForSeconds(0.033f);
        // }
        // for(float i = vignetteMinIntensity; i < vignetteFlashTime/2; i += 0.033f) {
        //     vignette.intensity.value = Mathf.Lerp(vignetteMaxIntensity, vignetteMinIntensity, i/vignetteFlashTime/2);
        //     yield return new WaitForSeconds(0.033f);
        // }
        vignette.intensity.value = vignetteMaxIntensity;
        // 30 frames per second
        for(float i = vignetteMinIntensity; i < vignetteFlashTime; i += 0.033f) {
            vignette.intensity.value = Mathf.Lerp(vignetteMaxIntensity, vignetteMinIntensity, i/vignetteFlashTime);
            yield return new WaitForSeconds(0.033f);
        }
    }

    private IEnumerator ChromaticAbberation(float flashTime) {
        chromaticAberration.intensity.value = chromaticAbberationMin;
        // 30 frames per second
        // for(float i = vignetteMinIntensity; i < vignetteFlashTime/2; i += 0.033f) {
        //     chromaticAberration.intensity.value = Mathf.Lerp(vignetteMinIntensity, vignetteMaxIntensity, i/vignetteFlashTime/2);
        //     yield return new WaitForSeconds(0.033f);
        // }
        for(float i = chromaticAbberationMin; i < chromaticAbberationTime; i += 0.033f) {
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAbberationMax, chromaticAbberationMin, i/chromaticAbberationTime);
            yield return new WaitForSeconds(0.033f);
        }
        // chromaticAberration.intensity.value = vignetteMaxIntensity;
        // // 30 frames per second
        // for(float i = vignetteMinIntensity; i < vignetteFlashTime; i += 0.033f) {
        //     vignette.intensity.value = Mathf.Lerp(vignetteMaxIntensity, vignetteMinIntensity, i/vignetteFlashTime);
        //     yield return new WaitForSeconds(0.033f);
        // }
    }

    public void StartShake() {
        start = true;
    }
}
