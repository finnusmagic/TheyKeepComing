using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentExample : MonoBehaviour
{

    public AIWaypointNetwork waypointNetwork = null;
    public int currentIndex = 0;
    public bool HasPath = false;
    public bool PathPendig = false;
    public bool PathStale = false;
    public NavMeshPathStatus pathStatus = NavMeshPathStatus.PathInvalid;

    private NavMeshAgent _navAgent = null;

    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();

        if (waypointNetwork == null) return;


        SetNextDestination(false);
    }

    void Update()
    {
        HasPath = _navAgent.hasPath;
        PathPendig = _navAgent.pathPending;
        PathStale = _navAgent.isPathStale;
        pathStatus = _navAgent.pathStatus;

        if ((!HasPath && !PathPendig) || pathStatus == NavMeshPathStatus.PathInvalid)
        {
            SetNextDestination(true);
        }
        else if (_navAgent.isPathStale)
        {
            SetNextDestination(false);
        }
    }

    void SetNextDestination(bool increment)
    {
        if (!waypointNetwork) return;

        int incStep = increment ? 1 : 0;

        int nextWaypoint = (currentIndex + incStep >= waypointNetwork.Waypoints.Count) ? 0 : currentIndex + incStep;
        Transform nextWaypointTransform = waypointNetwork.Waypoints[nextWaypoint];

        if (nextWaypointTransform != null)
        {
            currentIndex = nextWaypoint;
            _navAgent.destination = nextWaypointTransform.position;
            return;
        }
        currentIndex++;
    }
}
