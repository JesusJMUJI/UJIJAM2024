using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraLerper : MonoBehaviour
{
    public new Camera camera;
    RaycastHit hit;
    private Ray ray;
    Transform hitObjectTransform = null;
    [SerializeField] float zoomSpeed = 6f;

    [SerializeField] private Vector3 targetBack; //Change for something else
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hitObjectTransform != null)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, hitObjectTransform.localScale.y,
                zoomSpeed * Time.deltaTime);
            camera.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(hitObjectTransform.position.x, hitObjectTransform.position.y, -10), zoomSpeed * Time.deltaTime);
        }
        else
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize,5,zoomSpeed*Time.deltaTime);
            camera.transform.position = Vector3.Lerp(camera.transform.position, targetBack, zoomSpeed * Time.deltaTime);
        }
        if (Input.GetMouseButtonDown(0))
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                
                Vector3 targetPosition = hit.collider.gameObject.transform.position;
                Vector3 newPosition = new Vector3(targetPosition.x, targetPosition.y, camera.transform.position.z); 
                // camera.transform.Translate(newPosition - camera.transform.position);

                hitObjectTransform = hit.collider.gameObject.transform;
            }
        }
        // Test
        else if (Input.GetMouseButtonDown(1))
        {
            hitObjectTransform = null;
            // Change for something
            targetBack = new Vector3(0, 0, -10);
        }
    }
    
}
