using UnityEngine;
using System.Collections;
//--------------------------------------------------------------------
//This class spawns particles as long as its ControlledCapsuleCollider is touching a wall
//--------------------------------------------------------------------
public class OnWallSlideParticleSpawner : MonoBehaviour {
    [SerializeField] ControlledCapsuleCollider m_Collider;
    [SerializeField] float m_Threshold;
    [SerializeField] ParticleSystem m_ParticleSystem;
    [SerializeField] int m_EmissionCount;
	void FixedUpdate () 
	{
        CSideCastInfo sideCastInfo = m_Collider.GetSideCastInfo();
        if (sideCastInfo.m_WallCastCount >= 2)
        {
            Vector2 currentVel = m_Collider.GetVelocity();
            float dot = Vector3.Dot(currentVel, CState.GetDirectionAlongNormal(currentVel, sideCastInfo.GetSideNormal()));
            if (dot >= m_Threshold)
            {
                m_ParticleSystem.transform.position = sideCastInfo.GetSidePoint();
                m_ParticleSystem.transform.LookAt(sideCastInfo.GetSidePoint() + new Vector3(sideCastInfo.GetSideNormal().x, sideCastInfo.GetSideNormal().y, 0.0f), Vector3.back);
                m_ParticleSystem.Emit(m_EmissionCount);
            }
        }
    }
}
