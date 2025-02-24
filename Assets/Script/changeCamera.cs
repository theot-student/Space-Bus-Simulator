using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    public Camera innerCamera;
    public Camera outerCamera;
    public Player player;
    public GameObject healthGameObject;
    void Start()
    {
        outerCamera.enabled=true;
        innerCamera.enabled=false;
    }

    // Update is called once per frame
    void Update()
    {
    if (!PauseGameScript.gameIsPaused) {
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
                healthGameObject.SetActive(true);
                
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
                healthGameObject.SetActive(false);
            }
        }
    }
    }
}
