using UnityEngine;

public class PNJ : MonoBehaviour
{
    public int id;
    private static int PNJCounter = 0;


    void Start()
    {
        PNJCounter += 1;
        id = PNJCounter;
    }
}