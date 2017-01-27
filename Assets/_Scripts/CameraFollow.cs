using UnityEngine;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {

    public Transform target;                // camera target

    private bool smooth = true;
    private float smoothSpeed = 0.125f;

    //Offsets the camera
    Vector3 offset;
    private int offsetX = 0;
    private int offsetY = 27;
    private int offsetZ = -23;

    //Zoom
    public bool allowZoom = true;
    private float zoom = 1f;
    
    void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
        offset = new Vector3(offsetX, offsetY, offsetZ);
    }


    private void LateUpdate()
    {
        //Position the camera behind my player
        Vector3 desiredPosition = target.transform.position + offset;
        if (smooth)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        }
        else
        {
            transform.position = desiredPosition;
        }

        //Zoom the camera
        if (allowZoom)
        {
            
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && GetComponent<Camera>().fieldOfView < 22)
            {
                //Gets away from the player
                //GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, zoom, Time.deltaTime * zoomSpeed);
                //GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z - .2f);
                GetComponent<Camera>().fieldOfView += zoom; // Mathf.Lerp(GetComponent<Camera>().fieldOfView, zoom, Time.deltaTime * zoomSpeed);
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && GetComponent<Camera>().fieldOfView > 18)
            {
                //Gets closer to the player
                //GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, zoom, Time.deltaTime * zoomSpeed);
                GetComponent<Camera>().fieldOfView -= zoom;// Mathf.Lerp(GetComponent<Camera>().fieldOfView, zoom, zoomSpeed);
            }
        }

    }
}
