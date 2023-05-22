using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour {

    public Transform jugador;
    public Transform head;
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;

    [Range(0f, 360f)]
    public float visionAngle = 30f;
    public float visionDistance = 10f;
    bool detected = false;

    void Start() {
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }


    void GotoNextPoint() {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }


    void Update() {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (agent.remainingDistance < 0.5f) {
            GotoNextPoint();
        }

        detected = false;
        Vector3 playerVector = jugador.position - head.position;
        if (Vector3.Angle(playerVector.normalized, head.forward) < visionAngle * 0.5) { 
            if (playerVector.magnitude < visionDistance) {
                detected = true;

                // Si detecta al jugador lo persigue
                agent.destination = jugador.position;
            }
        }
    }

    private void OnDrawGizmos() {
        if(visionAngle <= 0f) {
            return;
        }
        float halfVisionAngle = visionAngle * 0.5f;

        Vector2 p1, p2;

        p1 = PointForAngle(halfVisionAngle, visionDistance);
        p2 = PointForAngle(-halfVisionAngle, visionDistance);

        if (detected) {
            Gizmos.color = Color.green;
        } else {
            Gizmos.color = Color.red;
        }
        
        Gizmos.DrawLine(head.position, (Vector2)head.position + p1);
        Gizmos.DrawLine(head.position, (Vector2)head.position + p2);

        Gizmos.DrawRay(head.position, head.right * 4f);
    }

    Vector3 PointForAngle(float angle, float distance) {
        return transform.TransformDirection(new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),
            0, Mathf.Cos(angle * Mathf.Deg2Rad)) * distance);
    }
}