using UnityEngine;

public class changeCamera : MonoBehaviour
{
    public Camera innerCamera;
    public Camera outerCamera;
    
    void Start()
    {
        outerCamera.enabled=true;
        innerCamera.enabled=false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("c")){
        if (outerCamera.enabled){
        	innerCamera.enabled=true;
		outerCamera.enabled=false;	
	}
        else {
        	outerCamera.enabled=true;
        	innerCamera.enabled=false;
        	
        }
        }
        
    }
}
