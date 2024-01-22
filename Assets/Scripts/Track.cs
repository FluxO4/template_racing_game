using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public List<WayPointGroup> wayPointGroups;

    public List<Transform> startSpots; //A collection of starting points in order, where cars are placed upon starting the game
    public GameObject startRegion; //A collider region that has an exit surface at the starting line of the track
    public GameObject wayPointGroupRootObject;

    public List<Transform> checkPoints;

    //Collects the waypoints
    public void Initialise()
    {
        //Check if wayPointGroupGroup exists

        if (!wayPointGroupRootObject)
        {
            Debug.Log("No way points found");
            return;
        }

        //Collecting positions of all waypoints in each group, and setting each of their cumulative distances
        wayPointGroups = new List<WayPointGroup>();
        for (int i = 0; i < wayPointGroupRootObject.transform.childCount; i++)
        {
            WayPointGroup t = new WayPointGroup();
            Transform wayPointGroupTransform = wayPointGroupRootObject.transform.GetChild(i);
            t.wayPoints = new List<Vector3>();
            t.distanceFromStart = new List<float>();
            float cumulativeDistance = 0;
            for(int ii = 0; ii < wayPointGroupTransform.childCount; ii++)
            {
                Vector3 pos = wayPointGroupTransform.GetChild(ii).position;

                if (t.wayPoints.Count > 0)
                {
                    cumulativeDistance+=(pos - t.wayPoints[t.wayPoints.Count - 1]).magnitude; 
                }
                
                t.wayPoints.Add(pos);
                
                t.distanceFromStart.Add(cumulativeDistance);
                
            }

            t.totalLength = cumulativeDistance;
            wayPointGroups.Add(t);
        }

        //Setting distances to each waypoint from the starting waypoint
    }


    public float distanceFromStart(Vector3 position, int startSearchFromWaypointIndex, int wayPointGroupIndex = 0)
    {
        startSearchFromWaypointIndex = Mathf.Max(0, Mathf.Min(startSearchFromWaypointIndex, wayPointGroups[wayPointGroupIndex].wayPoints.Count - 1));

        for(int i = startSearchFromWaypointIndex; i < wayPointGroups[wayPointGroupIndex].wayPoints.Count - 1; i++)
        {
            float invLerp = InverseLerp(wayPointGroups[wayPointGroupIndex].wayPoints[i], wayPointGroups[wayPointGroupIndex].wayPoints[i], position);
            if (invLerp < 1 && invLerp > 0)
            {
                float range = -wayPointGroups[wayPointGroupIndex].distanceFromStart[i] + wayPointGroups[wayPointGroupIndex].distanceFromStart[i + 1];
                return wayPointGroups[wayPointGroupIndex].distanceFromStart[i] + range * invLerp;
            }
        }
        return 0;
    }

    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }


    #region Singleton stuff
    public static Track i
    {
        get
        {
            return instance;
        }
    }
    public static Track instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Two tracks in one scene, destroying one");
            Destroy(gameObject);
        }
    }

    #endregion
}

public class WayPointGroup
{
    public float totalLength = 0;
    public float startGroupDistance = 0;
    public List<Vector3> wayPoints;
    public List<float> distanceFromStart;
}