using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MonoBehaviour {
    public MapCreator map_creator = null;
    // Use this for initialization
    void Start () {
        map_creator = GameObject.Find("GameRoot").GetComponent<MapCreator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (this.map_creator.isDelete(this.gameObject))
        { // 카메라에게 나 안보이냐고 물어보고 안 보인다고 대답하면,
            GameObject.Destroy(this.gameObject); // 자기 자신을 삭제.
        }
    }
}
