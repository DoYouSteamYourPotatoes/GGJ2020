using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//--------------------------------------------------------------------
//Hook module is a movement ability
//--------------------------------------------------------------------
public class HookModule : GroundedControllerAbilityModule
{
    [System.Serializable]
    public class HookPointData
    {
        public GameObject go;
        public float angle;
        public float distance;
    }
    List<HookPointData> list_hpd = new List<HookPointData>();

    [SerializeField] private float m_detectionRadius;
    [SerializeField] private float m_detectionSweep;
    [SerializeField] private bool m_OverridePreviousSpeed;

    private Transform directionPointer = null;
    private Transform sphereCast = null;
    //Reset all state when this module gets initialized
    protected override void ResetState()
    {
        base.ResetState();
        //m_LastDashTime = Time.fixedTime - m_DashCooldown;
        //m_HasDashedAndNotTouchedGroundYet = false;
    }

    //Called whenever this module is started (was inactive, now is active)
    protected override void StartModuleImpl()
    {
        //m_LastDashTime = Time.fixedTime;
        //m_HasDashedAndNotTouchedGroundYet = true;
        directionPointer = GameObject.FindGameObjectWithTag("DirectionPointer").transform;
        sphereCast = GameObject.FindGameObjectWithTag("SphereCast").transform;
    }

    //Execute jump (lasts one update)
    //Called for every fixedupdate that this module is active
    public override void FixedUpdateModule()
    {
        list_hpd.Clear();
        Collider[] hitColliders = Physics.OverlapSphere(sphereCast.position, m_detectionRadius);
        foreach (Collider item in hitColliders)
        {
            if (item.gameObject.tag.Equals("HookPoint"))
            {
                HookPointData hpd = new HookPointData();
                hpd.go = item.gameObject;
                hpd.angle = Vector3.Angle(directionPointer.forward, hpd.go.transform.position - directionPointer.position);
                hpd.distance = Vector3.Distance(directionPointer.position, hpd.go.transform.position);
                list_hpd.Add(hpd);
            }
        }
        if (list_hpd.Count.Equals(0))
        {
            return;
        }

        
        list_hpd.Sort((hpd1, hpd2) => hpd1.distance.CompareTo(hpd2.distance));
        HookPointData finalHPD = list_hpd[0];
        Vector3 finalDirection = (directionPointer.position - finalHPD.go.transform.position).normalized;
        finalDirection.z = 0.0f;

        Vector2 currentVel = Vector2.zero;
        Vector2 jumpVelocity = finalDirection * (-42.0f - Vector3.Distance(directionPointer.position, finalHPD.go.transform.position));
        //Vector2 jumpVelocity = finalDirection * -50.0f;
        currentVel += jumpVelocity;
        m_ControlledCollider.UpdateWithVelocity(currentVel);
    }

    //Called whenever this module is inactive and updating (implementation by child modules), useful for cooldown updating etc.
    public override void InactiveUpdateModule()
    {
        /*
        if (m_ControlledCollider.IsGrounded() ||
           (m_ControlledCollider.IsPartiallyTouchingWall() && m_ResetDashsAfterTouchingWall) ||
           (m_ControlledCollider.IsTouchingEdge() && m_ResetDashsAfterTouchingEdge))
        {
            m_HasDashedAndNotTouchedGroundYet = false;
        }
        */
    }

    public bool CanStartDash()
    {
        /*if (!GetDirInput("Aim").HasSurpassedThreshold())
        {
            return false;
        }*/
        return true;
    }
    //Query whether this module can be active, given the current state of the character controller (velocity, isGrounded etc.)
    //Called every frame when inactive (to see if it could be) and when active (to see if it should not be)
    public override bool IsApplicable()
    {
        /*
        if (Time.fixedTime - m_LastDashTime < m_DashCooldown || m_HasDashedAndNotTouchedGroundYet)
        {
            return false;
        }
        */
        if (/*!DoesInputExist("Aim") || */!DoesInputExist("Hook"))
        {
            Debug.LogError("Input for module " + GetName() + " not set up");
            return false;
        }
        if (/*GetDirInput("Aim").HasSurpassedThreshold() && */GetButtonInput("Hook").m_WasJustPressed)
        {
            return true;
        }
        return false;
    }
}
