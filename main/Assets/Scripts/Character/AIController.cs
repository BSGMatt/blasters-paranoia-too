using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIController : MonoBehaviour
{
    public Transform target;
    public Enemy enemy;
    public bool reachedEndOfPath = false;

    private Path path;
    private int currentWaypoint = 0;
    private float nextWaypointDistance = 1f;

    Seeker seeker;

    /// <summary>
    /// Finds a path target matching the given target type. A target can be either the player 
    /// or a building. 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private Transform FindTargetOfType(int type) {
        switch (type) {
            default:
                return FindObjectOfType<BB>().transform;
            case 1:
                Building[] b = FindObjectsOfType<Building>();
                if (b.Length == 0) return FindObjectOfType<BB>().transform;
                Transform ret = null;
                float minDistance = 10000000;
                for (int i = 0; i < b.Length; i++) {
                    float dist = Vector2.Distance(transform.position, b[i].transform.position);
                    if (dist < minDistance) {
                        minDistance = dist;
                        ret = b[i].transform;
                    }
                }

                return ret;
        }
    }

    public void InitPath(int targetType) {
        seeker = GetComponent<Seeker>();
        target = FindTargetOfType(targetType);

        seeker.StartPath(enemy.GetRigidBody().position, target.position, OnPathComplete);
        reachedEndOfPath = false;
    }

    private void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    public void RunPath() {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count) {
            reachedEndOfPath = true;
            return;
        }
        else {
            reachedEndOfPath = false;
        }

        Debug.Log("Attempting to move along path...");

        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - enemy.GetRigidBody().position).normalized;

        enemy.c2d.Move(direction, enemy.GetMaxSpeed() * enemy.currentSpeedModValue);

        float distance = Vector2.Distance(enemy.GetRigidBody().position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }
    }

    public void StopPath() {
        enemy.c2d.StopMoving();
        path = null;
    }

}
