using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float globalMaxX;
    public float globalMaxY;
    public float globalMinX;
    public float globalMinY;

    [SerializeField] float shakeTimeStandardStrong = 0.5f;
    [SerializeField] float strengthStandardStrong = 0.5f;
    [SerializeField] float shakeTimeStandardLight = 0.2f;
    [SerializeField] float strengthStandardLight = 0.2f;
    float screenShakeStrength = 0;
    float screenShakeTimer = 0;

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 start = transform.position;
            Vector3 goal = target.position + new Vector3(0.0f, 0.0f, -10);
            float t = Time.deltaTime * speed;
            Vector3 newPosition = Vector3.Lerp(start, goal, t);
            float maxX = globalMaxX - Camera.main.orthographicSize * Camera.main.aspect;
            float maxY = globalMaxY - Camera.main.orthographicSize;
            float minX = globalMinX + Camera.main.orthographicSize * Camera.main.aspect;
            float minY = globalMinY + Camera.main.orthographicSize;
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            if (screenShakeTimer > 0)
            {
                newPosition += Random.onUnitSphere * screenShakeStrength;
                screenShakeTimer -= Time.deltaTime;
            }

            transform.position = newPosition;
        }
    }

    //This draws the camera border in the scene view for easier viewing of the border
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(globalMinX, globalMinY, 0.0f), new Vector3(globalMaxX, globalMinY, 0.0f));
        Gizmos.DrawLine(new Vector3(globalMinX, globalMaxY, 0.0f), new Vector3(globalMaxX, globalMaxY, 0.0f));
        Gizmos.DrawLine(new Vector3(globalMinX, globalMinY, 0.0f), new Vector3(globalMinX, globalMaxY, 0.0f));
        Gizmos.DrawLine(new Vector3(globalMaxX, globalMinY, 0.0f), new Vector3(globalMaxX, globalMaxY, 0.0f));
    }

    public void ScreenShakeLight()
    {
        screenShakeStrength = strengthStandardLight;
        screenShakeTimer = shakeTimeStandardLight;
    }

    public void ScreenShakeStrong()
    {
        screenShakeStrength = strengthStandardStrong;
        screenShakeTimer = shakeTimeStandardStrong;
    }

    public void ScreenShake(float strength, float shakeTime)
    {
        screenShakeStrength = strength;
        screenShakeTimer = shakeTime;
    }

    public void zoomOut()
    {
        StartCoroutine(Cutscene());
    }

    IEnumerator Cutscene()
    {
        Camera cam = GetComponent<Camera>();
        Vector3 target = new Vector3(0, 30, -10);
        Vector3.Lerp(transform.position, target, 1);
        for (int i = 0; i < 60; i++)
        {
            cam.orthographicSize = 5 + i/2;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(6);
        target.y = -3;
        Vector3.Lerp(transform.position, target, 1);
        for (int i = 0; i < 61; i++)
        {
            cam.orthographicSize = 35 - i/2;
            yield return new WaitForEndOfFrame();
        }     
    }

}
