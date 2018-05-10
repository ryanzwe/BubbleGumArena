using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraZoomer : MonoBehaviour
{
    [SerializeField] List<Transform> targetsS;
    [SerializeField] Transform zeroPoint;
    [SerializeField] float ignoreDistance = 20;
    [SerializeField] float ignoreHeight = 5;
    [SerializeField] float ignoreDeapth = 0;
    [SerializeField] float extraZoomOut;

    [SerializeField] Vector3 offset;
    [SerializeField] float smoothTime = 0.5f;

    [SerializeField] float minZoom = 40f;
    [SerializeField] float maxZoom = 10f;
    [SerializeField] float zoomLimiter = 50f;

    private Vector3 velocity;
    private Camera cam;
    private Transform[] targets = new Transform[4];

    void Start()
    {
        offset = transform.position;
        cam = GetComponent<Camera>();
        targets[0] = targetsS[0];
        targets[1] = targetsS[1];
        targets[2] = targetsS[2];
        targets[3] = targetsS[3];
    }

    private void LateUpdate()
    {
        if (targets.Length == 0)
            return;
        for (int i = 0; i < targetsS.Count; i++)
        {
            if (targetsS[i].position.x > ignoreDistance || targetsS[i].position.z > ignoreDistance || targetsS[i].position.x < -ignoreDistance || targetsS[i].position.z < -ignoreDistance || targetsS[i].position.y > ignoreHeight || targetsS[i].position.y < ignoreDeapth)
                targets[i] = zeroPoint;
            else
                targets[i] = targetsS[i];
        }

        Move();
        Zoom();
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;//(offset + new Vector3(offset.x,offset.y * extraZoomOut,offset.z * extraZoomOut));
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Length; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }
    Vector3 GetCenterPoint()
    {
        if(targets.Length == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Length; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;

    }

}
