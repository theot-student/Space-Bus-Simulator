using UnityEngine;
using System.Collections.Generic;
public class SpaceStationsScript : MonoBehaviour
{
    public List<SpaceStationScript> spaceStations = new List<SpaceStationScript>();
    public int nbSpaceStations;

    void Start()
    {
        AddChildrenToList();
    }

    void AddChildrenToList()
    {
        spaceStations.Clear(); // Clear the list before adding to avoid duplicates

        foreach (Transform child in transform)
        {
            SpaceStationScript station = child.GetComponent<SpaceStationScript>();
            if (station != null)
            {
                spaceStations.Add(station);
            }
        }
        nbSpaceStations = spaceStations.Count;
    }
}
