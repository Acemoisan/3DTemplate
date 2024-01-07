using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmo : MonoBehaviour
{
    [SerializeField] UnityEngine.GameObject positionCenter;
    [SerializeField] float diameter;
    [SerializeField] Color gizmoColor;


    void OnDrawGizmos()
    {
        // Display the radius of given position
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(positionCenter.transform.position, diameter);
    }
}
