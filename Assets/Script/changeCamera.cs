using UnityEngine;

public class changeCamera : MonoBehaviour
{
    public Camera innerCamera;
    public Camera outerCamera;
    public Player player;
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
                player.isFirstPerson=true;
                Renderer[] renderers = player.GetComponentsInChildren<Renderer>();
                foreach (Renderer rend in renderers)
                {
                    rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;   
                }

                
        }
            else {
                outerCamera.enabled=true;
                innerCamera.enabled=false;
                Renderer[] renderers = player.GetComponentsInChildren<Renderer>();
                player.isFirstPerson=false;
                foreach (Renderer rend in renderers)
                {
                    rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
                
            }
        }
        
    }
}
