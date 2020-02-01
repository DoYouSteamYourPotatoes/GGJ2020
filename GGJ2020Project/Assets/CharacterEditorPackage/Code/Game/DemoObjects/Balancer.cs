using UnityEngine;
using System.Collections;
//--------------------------------------------------------------------
//Small class to rotate an object back and forth a certain angle. Used for moving platforms in levels
//--------------------------------------------------------------------
public class Balancer: MonoBehaviour
{
    [SerializeField] float m_MovementSpeed;
    [SerializeField] float m_MaxAngle;
    [SerializeField] AnimationCurve m_RotationCurve;

    protected float m_Time;

    void FixedUpdate()
    {
        m_Time += Time.fixedDeltaTime * m_MovementSpeed;
        float angle = m_RotationCurve.Evaluate(m_Time) * m_MaxAngle * 2.0f - m_MaxAngle;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
