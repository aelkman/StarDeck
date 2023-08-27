using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorFollower_Secondary : MonoBehaviour
{
    public float expandSize;
    public float timeUnit = 1f;
    public float totTime = 10f;

    private bool coroutineAllowed;
    private float xRot;
    private float yRot;
    private float zRot;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Vector3 raisedPosition;
    private Vector3 expandedScale;
    private int siblingIndexOriginal;

    Coroutine start;
    Coroutine stop;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("index: " + transform.GetSiblingIndex());
        // hard coded solution for optimization, if the expandSize changes
        // this will need to be recalculated
        expandedScale = new Vector3(4.19f, 5.26f, 0.00f);
        coroutineAllowed = true;
        originalScale = transform.localScale;
        originalRotation = transform.rotation;
        originalPosition = transform.localPosition;
        Debug.Log("original rotation: " + originalRotation);
        Debug.Log("originalScale: " + originalScale);
        siblingIndexOriginal = transform.GetSiblingIndex();
    }

    private float Sinerp(float t) {
        return Mathf.Sin(t * Mathf.PI * 0.5f);
    }

    private float Coserp(float t) {
        return Mathf.Cos(t * Mathf.PI * 0.5f);
    }

    private float EaseOutCirc(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * Mathf.Sqrt(1 - value * value) + start;
    }

    public static float EaseInCirc(float start, float end, float value)
    {
        end -= start;
        return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
    }

    public static float EaseInBack(float start, float end, float value)
    {
        end -= start;
        value /= 1;
        float s = 1.70158f;
        return end * (value) * value * ((s + 1) * value - s) + start;
    }

    void Update() {

        GameObject someObject = transform.parent.gameObject.transform.GetChild(0).gameObject;
        int childCount = transform.parent.gameObject.transform.childCount - 1;
        // Debug.Log("childCount: " + childCount);
        // Debug.Log(someObject.name);
        Vector3 positionFollow = someObject.transform.localPosition;
        // Debug.Log("followPosition: " + positionFollow);
        // Debug.Log("siblingIndex: " + transform.GetSiblingIndex());
        float fractionalPlace = ((float)transform.GetSiblingIndex() - 1) / (float)childCount;

        // float fractionalPlace = 1f / ((float)transform.GetSiblingIndex());
        // Debug.Log("fractionalPlace: " + fractionalPlace);

        transform.localPosition = new Vector3(
            // Mathf.Lerp(originalPosition.x, positionFollow.x, Coserp(fractionalPlace)),
            EaseInCirc(originalPosition.x, positionFollow.x, fractionalPlace),
            Mathf.Lerp(originalPosition.y, positionFollow.y, fractionalPlace * fractionalPlace),
            originalPosition.z
        );

        Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 translatedWorldPosition = new Vector3(screenToWorld.x, screenToWorld.y, Camera.main.nearClipPlane);
        Vector3 currentAngle = new Vector3(0f, 0f, Mathf.Lerp(-180f, 0f, originalPosition.x + translatedWorldPosition.x));
        transform.eulerAngles = currentAngle;
    }

    private static float WrapAngle(float angle)
    {
        angle%=360;
        if(angle >180)
            return angle - 360;

        return angle;
    }
}



