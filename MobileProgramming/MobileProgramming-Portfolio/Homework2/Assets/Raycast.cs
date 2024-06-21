using UnityEngine;
using System.Collections;

public class Raycast : MonoBehaviour {
    private Camera mCamera;
    Camera kCamera; 

    // Use this for initialization
    void Start () {
        this.mCamera = Camera.main;
        kCamera= GetComponent<Camera>();

    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetMouseButtonDown(0))
        {
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;
            Ray mRay = this.mCamera.ScreenPointToRay(Input.mousePosition);
            //Debug.Log(Input.mousePosition.x);
            Vector3 point = kCamera.ScreenToWorldPoint(new Vector3(x, y,20));
            Debug.Log(point);
            //transform.position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            //Debug.Log(transform.position);
            RaycastHit hit;

            if(Physics.Raycast(mRay,out hit))
            {
                //Debug.Log(hit.collider.gameObject.transform.position);
            }
        }
	}
}
