using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//--------------------------------------------------------------------
//Dash module is a movement ability
//--------------------------------------------------------------------
public class DashModule : GroundedControllerAbilityModule
{
    public static event Action<bool> Dashing = null;

    [SerializeField] float m_DashStrength;
    [SerializeField] float m_DashCooldown;

    [SerializeField] bool m_ResetDashsAfterTouchingWall;
    [SerializeField] bool m_ResetDashsAfterTouchingEdge;
    [SerializeField] bool m_OverridePreviousSpeed;

    private Transform directionPointer = null;
    public bool isDashing = false;
    public float breakingWindow = 0.35f;
    public float breakingWindowCounter = 0.0f;

    float m_LastDashTime;
    public bool m_HasDashedAndNotTouchedGroundYet;
    //Reset all state when this module gets initialized
    protected override void ResetState()
    {
        base.ResetState();
        m_LastDashTime = Time.fixedTime - m_DashCooldown;
        m_HasDashedAndNotTouchedGroundYet = false;
    }

    //Called whenever this module is started (was inactive, now is active)
    protected override void StartModuleImpl()
    {
        directionPointer = GameObject.FindGameObjectWithTag("DirectionPointer").transform;
        m_LastDashTime = Time.fixedTime;
        m_HasDashedAndNotTouchedGroundYet = true;
    }

    private void Update()
    {
        if (isDashing)
        {
            breakingWindowCounter += Time.deltaTime;
            if (breakingWindowCounter >= breakingWindow)
            {
                isDashing = false;
                Dashing?.Invoke(false);
            }
        }
    }

    //Execute jump (lasts one update)
    //Called for every fixedupdate that this module is active
    public override void FixedUpdateModule()
    {
        isDashing = true;
        Dashing?.Invoke(true);
        breakingWindowCounter = 0.0f;

        Vector2 direction = GetDirInput("Aim").m_ClampedInput.normalized;
        //Vector2 direction = directionPointer.forward;

        /*Vector2 currentVel = m_ControlledCollider.GetVelocity();
        if (m_OverridePreviousSpeed)
        {
            currentVel = Vector2.zero;
        }
        Vector2 jumpVelocity = direction * m_DashStrength;

        currentVel += jumpVelocity;

        m_ControlledCollider.UpdateWithVelocity(currentVel);*/
        m_ControlledCollider.SetVelocity(new Vector2(m_DashStrength * direction.x, 10.0f));
    }

    //Called whenever this module is inactive and updating (implementation by child modules), useful for cooldown updating etc.
    public override void InactiveUpdateModule()
    {
        if (m_ControlledCollider.IsGrounded() ||
           (m_ControlledCollider.IsPartiallyTouchingWall() && m_ResetDashsAfterTouchingWall) ||
           (m_ControlledCollider.IsTouchingEdge() && m_ResetDashsAfterTouchingEdge))
        {
            m_HasDashedAndNotTouchedGroundYet = false;
        }
    }

    public bool CanStartDash()
    {
        if (Time.fixedTime - m_LastDashTime < m_DashCooldown || m_HasDashedAndNotTouchedGroundYet /*|| !GetDirInput("Aim").HasSurpassedThreshold()*/)
        {
            return false;
        }
        return true;
    }
    //Query whether this module can be active, given the current state of the character controller (velocity, isGrounded etc.)
    //Called every frame when inactive (to see if it could be) and when active (to see if it should not be)
    public override bool IsApplicable()
    {
        if (Time.fixedTime - m_LastDashTime < m_DashCooldown || m_HasDashedAndNotTouchedGroundYet)
        {
            return false;
        }
        if (/*!DoesInputExist("Aim") ||*/ !DoesInputExist("Dash"))
        {
            Debug.LogError("Input for module " + GetName() + " not set up");
            return false;
        }
        if (/*GetDirInput("Aim").HasSurpassedThreshold() && */GetButtonInput("Dash").m_WasJustPressed)
        {
            return true;
        }
        return false;
    }
}
