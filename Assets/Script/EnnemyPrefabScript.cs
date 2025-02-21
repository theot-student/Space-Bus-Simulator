using UnityEngine;

public class EnnemyPrefabScript : MonoBehaviour
{
    public GameObject ennemySpaceship;

    // Update is called once per frame
    void Update()
    {  
        transform.position = ennemySpaceship.transform.position;
    }
}
