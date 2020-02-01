using UnityEngine;
using System.Collections;
//--------------------------------------------------------------------
//Spawns particles when the character is moving across the floor
//--------------------------------------------------------------------
public class OnRunParticleSpawner : MonoBehaviour {
    [SerializeField] ControlledCapsuleCollider m_Collider;
    [SerializeField] float m_LowThreshold;
    [SerializeField] float m_HighThreshold;
    [SerializeField] ParticleSystem m_ParticleSystem;
    [SerializeField] int m_LowEmissionCount;
    [SerializeField] int m_HighEmissionCount;
	void FixedUpdate () 
	{
        if (m_Collider.IsGrounded())
        {
            CGroundedInfo groundedInfo = m_Collider.GetGroundedInfo();
            Vector2 currentVel = m_Collider.GetVelocity();
            float dot = Vector3.Dot(currentVel, CState.GetDirectionAlongNormal(currentVel, groundedInfo.GetNormal()));

            if (dot >= m_LowThreshold)
            {
                int emission = m_LowEmissionCount;
                if (dot >= m_HighThreshold)
                {
                    emission = m_HighEmissionCount;
                }
                m_ParticleSystem.transform.position = groundedInfo.GetPoint();
                m_ParticleSystem.transform.LookAt(groundedInfo.GetPoint() + new Vector3(groundedInfo.GetNormal().x, groundedInfo.GetNormal().y, 0.0f), Vector3.back);
                m_ParticleSystem.Emit(emission);
            }
        }
    }
}
