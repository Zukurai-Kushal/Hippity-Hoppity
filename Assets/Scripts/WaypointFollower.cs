using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private bool followBackAndForth = true;
    [SerializeField] private float speed = 2f;
    private int currWaypointIndex = 0;
    private int indexIncrementValue = 1; 
    // Update is called once per frame
    void Update()
    {

        if(waypoints.Length > 1 && Vector2.Distance(waypoints[currWaypointIndex].transform.position, this.transform.position) < 0.1f)
        {
            if(followBackAndForth)  // In a back and fourth loop
            {
                if(currWaypointIndex == 0)
                {
                    indexIncrementValue = 1;
                }
                else if(currWaypointIndex >= waypoints.Length - 1)
                {
                    indexIncrementValue = -1;
                }
                currWaypointIndex += indexIncrementValue;
            }
            else // In a circuit loop
            {
                if(currWaypointIndex >= waypoints.Length - 1)
                {
                    currWaypointIndex = 0;
                }
                else
                {
                    currWaypointIndex++;
                }
            }
        }

        if(waypoints.Length > 0)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, waypoints[currWaypointIndex].transform.position, Time.deltaTime * speed);
        }
    }
}
