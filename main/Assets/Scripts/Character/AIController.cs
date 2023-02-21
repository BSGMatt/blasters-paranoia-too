using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIController : MonoBehaviour
{
    public Transform target; //The target the enemy will move towards. 
    public Character character; //The character that this controller belongs to. 
    public bool reachedEndOfPath = false;
    public float minDistance; //The minimum distance the enemy will be from the target before stopping. 
    public float maxDistance; //The maximum distance the enemy will be from the target before it starts moving again.

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
                if (b.Length == 0) return FindObjectOfType<BB>().transform; //default to player if no buildings are present.
                return BestBuildingToKill(b);

            case 2:
                return FindClosestCrystal();
        }
    }

    private Transform BestBuildingToKill(Building[] buildings) {

        /**
         * Rank the list of enemies based on their "moment importance",
         * which is equal to:
         * building's importance - building's distance from character. 
         */
        int maxImportance = -10000000;
        Building best = null;
        foreach (Building b in buildings) {
            int bImportance = b.card.importance - 
                (int) Vector2.Distance(b.transform.position, character.transform.position);

            if (bImportance > maxImportance) {
                maxImportance = bImportance;
                best = b;
            }
        }

        return best.transform;
    }

    private Transform FindClosestCrystal() {
        GameManager gm = FindObjectOfType<GameManager>();

        float minDistance = 1000000;
        Transform ret = gm.crystals[0].transform; //Use first crystal by default. 
        foreach (Crystal c in gm.crystals) {
            if (!c.dead) {
                float distance = Vector2.Distance(c.transform.position, 
                    character.transform.position);

                //Debug.Log("Distance from Crystal " + c + ": " + distance);

                if (distance < minDistance) {
                    minDistance = distance;
                    ret = c.transform;
                }
            }
        }

        return ret;
    }

    public void InitPath(TargetType type) {
        seeker = GetComponent<Seeker>();
        target = FindTargetOfType((int) type);

        seeker.StartPath(character.GetRigidBody().position, target.position, OnPathComplete);
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
            character.c2d.StopMoving();
            return;
        }
        else {
            reachedEndOfPath = false;
        }

        //Debug.Log("Attempting to move along path...");

        MoveAlongPath();

    }

    private void MoveAlongPath() {
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - character.GetRigidBody().position).normalized;

        character.c2d.Move(direction, character.GetMaxSpeed() * character.currentSpeedModValue);

        //Check if minDistance is reached. if so, set current waypoint to the last waypoint of the path. 
        if (Vector2.Distance(character.GetRigidBody().position, target.position) <= minDistance) {
            currentWaypoint = path.vectorPath.Count;
        }
        else {
            float distance = Vector2.Distance(character.GetRigidBody().position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance) {
                currentWaypoint++;
            }
        }
    }

    public void StopPath() {
        character.c2d.StopMoving();
        path = null;
    }

    public void ResetPath() {
        StopPath();
        InitPath(character.targetType);
    }

}
