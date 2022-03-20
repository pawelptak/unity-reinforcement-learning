using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform targetObject;
	
	public Vector3 cameraOffset;
	
    // Start is called before the first frame update
    void Start()
    {
		cameraOffset = transform.position - targetObject.transform.position;
        
    }

    void LateUpdate()
    {
        Vector3 newPos = targetObject.transform.position + cameraOffset;
		transform.position = newPos;
    }
}
