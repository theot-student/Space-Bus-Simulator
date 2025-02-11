using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnnemyHealthBarMarker : MonoBehaviour
{
    public GameObject healthGameObject;
    public Image img;
    public Transform target;
    public Vector3 offset;
    public Camera camera;
    void Update()
    {   
        if (!camera.enabled) {
            healthGameObject.SetActive(false);
        }
        else {
            float minX = img.GetPixelAdjustedRect().width / 2;
            float maxX = Screen.width - minX;

            float minY = img.GetPixelAdjustedRect().height / 2;
            float maxY = Screen.height - minX;

            Vector2 pos = Camera.main.WorldToScreenPoint(target.position + offset);
            
            if (Vector3.Dot((target.position - transform.position), transform.forward) < 0){
                if (pos.x < Screen.width / 2){
                    pos.x = maxX;
                } else {
                    pos.x = minX;
                }
            }

            if ((pos.x >= maxX) || (pos.x <= minX) || (pos.y <= minY) || (pos.y >= maxX)){
                healthGameObject.SetActive(false);
            } else {
                healthGameObject.SetActive(true);
                pos.x = Mathf.Clamp(pos.x, minX, maxX);
                pos.y = Mathf.Clamp(pos.y, minY, maxY);
                
                img.transform.position = pos;
            }
    }
    }
}
