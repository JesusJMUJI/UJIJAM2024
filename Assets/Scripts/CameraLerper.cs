using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class CameraLerper : MonoBehaviour
{
    public new Camera camera;
    RaycastHit hit;
    private Ray ray;
    Transform hitObjectTransform = null;
    [SerializeField] float zoomLevel;
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
                6 * Time.deltaTime);
        }
        if (Input.GetMouseButtonDown(0))
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                
                Vector3 targetPosition = hit.collider.gameObject.transform.position;
                Vector3 newPosition = new Vector3(targetPosition.x, targetPosition.y, camera.transform.position.z); 
                camera.transform.Translate(newPosition - camera.transform.position); 
                hitObjectTransform = hit.collider.gameObject.transform;
            }
        }
    }
    
}
