using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalFieldMovement : MonoBehaviour {
    [SerializeField]
    private Vector3 m_StartPosition;
    [SerializeField]
    private Vector3 m_Velocity;
    [SerializeField]
    private float m_TargetMass;
    [SerializeField]
    private float m_GravityConst;
    [SerializeField]
    private Vector3 m_TargetPosition;

    private const float TOLERANCE = 0.5f;
    private const float MAX_ACCELERATION_MAGTINUDE = 500f;

    private void Start() {
        transform.Translate(m_StartPosition);
    }
    
    void FixedUpdate() {
        float distance = (m_TargetPosition - transform.position).magnitude;
        if (distance < TOLERANCE) {return;}

        Vector3 accDirection = m_TargetPosition - m_StartPosition;
        float reversedMagnitude = 1 / accDirection.magnitude;
        float accMagnitude = Math.Min(
            MAX_ACCELERATION_MAGTINUDE, 
            m_GravityConst * m_TargetMass * reversedMagnitude * reversedMagnitude
            );
        Vector3 curAcc = accMagnitude * accDirection.normalized;
        
        Vector3 delta = Time.fixedDeltaTime * m_Velocity +
                        0.5f * Time.fixedDeltaTime  * Time.fixedDeltaTime * curAcc;
        m_Velocity += curAcc * Time.fixedDeltaTime;
        
        transform.Translate(delta);
    }
}