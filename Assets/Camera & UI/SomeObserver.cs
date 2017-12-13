using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeObserver : MonoBehaviour {

    CameraRaycaster cameraRaycaster;
    // Use this for initialization
    void Start()
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.layerChangeObservers += HandleIt;
    }

    void HandleIt()
    {
        print("IT WORKED");
    }
    public void test()
    {

    }
	// Update is called once per frame
	void Update () {
		
	}
}
