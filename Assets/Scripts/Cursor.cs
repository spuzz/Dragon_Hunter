using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {

    Camera m_MainCamera;
    // Use this for initialization
    void Start () {
        m_MainCamera = Camera.main;

    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0))
        {
            print(GetComponent<CameraRaycaster>().layerHit);
        }
	}
}
