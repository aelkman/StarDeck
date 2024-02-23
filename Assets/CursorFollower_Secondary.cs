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
    public float sizeMultiplier = 0.005f;

    // private float xRot;
    // private float yRot;
    // private float zRot;
    // private Quaternion originalRotation;
    // private Vector3 originalScale;
    public Vector3 actualOriginal;
    private Vector3 originalPosition;
    // private Vector3 originalWorldPosition;
    // private Vector3 raisedPosition;
    // private Vector3 expandedScale;
    // private int siblingIndexOriginal;
    private bool isScaled = false;

    Coroutine start;
    Coroutine stop;

    // Start is called before the first frame update
    void Start()
    {
        // // Debug.Log("index: " + transform.GetSiblingIndex());
        // hard coded solution for optimization, if the expandSize changes
        // this will need to be recalculated
        // expandedScale = new Vector3(4.19f, 5.26f, 0.00f);
        // originalScale = transform.localScale;
        // originalRotation = transform.rotation;
        originalPosition = transform.localPosition;
        // originalWorldPosition = transform.position;
        // // Debug.Log("original postiion: " + originalPosition);
        // // Debug.Log("original rotation: " + originalRotation);
        // // Debug.Log("originalScale: " + originalScale);
        // siblingIndexOriginal = transform.GetSiblingIndex();
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

    public static float EaseInCubic(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value + start;
    }

    public static float EaseInQuint(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value * value * value + start;
    }

    public static float EaseInExpo(float start, float end, float value)
    {
        end -= start;
        return end * Mathf.Pow(2, 10 * (value - 1)) + start;
    }

    void Update() {

        GameObject someObject = transform.parent.gameObject.transform.GetChild(0).gameObject;
        int childCount = transform.parent.gameObject.transform.childCount - 1;
        // // Debug.Log("childCount: " + childCount);
        // // Debug.Log(someObject.name);
        Vector3 positionFollow = someObject.transform.localPosition;
        // // Debug.Log("followPosition: " + positionFollow);
        // // Debug.Log("siblingIndex: " + transform.GetSiblingIndex());
        float fractionalPlace = ((float)transform.GetSiblingIndex() - 1) / (float)childCount;
        

        // float fractionalPlace = 1f / ((float)transform.GetSiblingIndex());
        // // Debug.Log("fractionalPlace: " + fractionalPlace);
        float fractionalPlace2 = ((float)transform.GetSiblingIndex()) / (float)childCount;

        if(!isScaled) {
            transform.localScale = new Vector3(
                transform.localScale.x + transform.GetSiblingIndex() * sizeMultiplier, 
                transform.localScale.y + transform.GetSiblingIndex() * sizeMultiplier, 
                transform.localScale.z + transform.GetSiblingIndex() * sizeMultiplier
            );

            isScaled = true;
        }

        transform.localPosition = new Vector3(
            // Mathf.Lerp(originalPosition.x, positionFollow.x, Coserp(fractionalPlace)),
            EaseInCubic(originalPosition.x, positionFollow.x, fractionalPlace),
            Mathf.Lerp(originalPosition.y, positionFollow.y, fractionalPlace),
            originalPosition.z
        );

        Vector3 mousePos = Mouse.current.position.ReadValue();   
        mousePos.z=Camera.main.nearClipPlane;
        Vector3 Worldpos=Camera.main.ScreenToWorldPoint(mousePos);
        // Debug.Log(Worldpos);
        // Vector3 currentAngle = new Vector3(0f, 0f, Mathf.Lerp(180f, 0f, (Worldpos.x+1.778f)/3.556f));

        float angle = Mathf.Rad2Deg * Mathf.Atan((actualOriginal.y - Worldpos.y) / (actualOriginal.x - Worldpos.x));
        if(angle < 0) {
            angle = 180 + angle;
        }

        float newAngle = Mathf.Lerp(90, angle, ((float)transform.GetSiblingIndex())/21f);
        if(newAngle > 90) {
            // newAngle = newAngle + 10 * Mathf.Pow(Mathf.Lerp(0, 1, Mathf.Abs( actualOriginal.x - Worldpos.x)), 3);
            newAngle = newAngle + 100 * Mathf.Tan(Mathf.Lerp(0, 1, Mathf.Abs( actualOriginal.x - Worldpos.x))/4);
            if(newAngle > 180) {
                newAngle = 180;
            }
        }
        else {
            // newAngle -= 10 * Mathf.Pow(Mathf.Lerp(0, 1, Mathf.Abs( actualOriginal.x - Worldpos.x)), 3);
            newAngle -= 100 * Mathf.Tan(Mathf.Lerp(0, 1, Mathf.Abs( actualOriginal.x - Worldpos.x))/4);
            if(newAngle < 0) {
                newAngle = 0;
            }
        }
        // Debug.Log(angle);
        Vector3 currentAngle = new Vector3(0f, 0f, newAngle);

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



