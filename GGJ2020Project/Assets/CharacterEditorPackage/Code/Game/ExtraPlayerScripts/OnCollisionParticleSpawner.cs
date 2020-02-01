using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//--------------------------------------------------------------------
//Spawns particles when a character hits a surface with enough velocity
//--------------------------------------------------------------------
public class OnCollisionParticleSpawner : MonoBehaviour {
    [SerializeField] ControlledCapsuleCollider m_Collider;
	[SerializeField] float m_Threshold;
    [SerializeField] ParticleSystem m_ParticleSystem;
    [SerializeField] int m_EmissionCount;
	void FixedUpdate () 
	{
        List<CapsuleCollisionOccurrance> cols = m_Collider.GetCollisionInfo();
        for (int i = 0; i < cols.Count; i ++)
        {       
            if (cols[i].m_VelocityLossPure.magnitude >= m_Threshold)
            {
                m_ParticleSystem.transform.position = cols[i].m_Point;
                m_ParticleSystem.transform.LookAt(cols[i].m_Point + cols[i].m_Normal, Vector3.back);
                m_ParticleSystem.Emit(m_EmissionCount);
            }
        }
	}
}
