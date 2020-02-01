using UnityEngine;
using System.Collections;

public class JetpackVisuals : MonoBehaviour {
    [SerializeField] CharacterControllerBase m_Character;
    [SerializeField] Transform m_JetpackModel;
    [SerializeField] ParticleSystem m_JetpackParticles;

    [SerializeField] SpriteRenderer m_UIBarFill;
    [SerializeField] SpriteRenderer m_UIBarOutline;
	[SerializeField] Gradient m_UIGradient;
    [SerializeField] Transform m_UIBarScaler;
    [SerializeField] Transform m_UITransform;
    [SerializeField] Transform m_UIAnchor;

    JetpackModule m_JetpackModule;
    float m_LastFuelFullTime;
    bool m_FuelWasFull;

    void Start()
    {
        m_UIBarFill.color = Color.clear;
        m_UIBarOutline.color = Color.clear;
        m_LastFuelFullTime = Time.time - 5.0f;
        m_FuelWasFull = true;
        m_JetpackModule = m_Character.GetAbilityModuleManager().GetModuleWithName("Jetpack") as JetpackModule;
        UpdateVisualsEnabled();
    }

	void Update () 
	{
        UpdateVisualsEnabled();
        UpdateVisuals();
	}
    void UpdateVisualsEnabled()
    {
        if (m_JetpackModule == null || m_JetpackModule.IsLocked())
        {
            EnableVisuals(false);
        }
        else
        {
            EnableVisuals(true);
        }
    }
    void UpdateVisuals()
    {
        ParticleSystem.EmissionModule emissionModule = m_JetpackParticles.emission;
        if (m_Character.GetAbilityModuleManager().GetCurrentModule() != null && m_Character.GetAbilityModuleManager().GetCurrentModule().GetName() == "Jetpack")
        {
            emissionModule.enabled = true;
        }
        else
        {
            emissionModule.enabled = false;
        }

        float currentFuelFactor = m_JetpackModule.GetFuelAs01Factor();
        float alpha = 1.0f;
        if (currentFuelFactor == 1.0f)
        {
            if (!m_FuelWasFull)
            {
                m_LastFuelFullTime = Time.time;
                m_FuelWasFull = true;
            }
            alpha = 1.0f - Mathf.Clamp01((Time.time - m_LastFuelFullTime) * 2.0f);
        }
        else
        {
            m_FuelWasFull = false;
        }

        Color fillColor = m_UIGradient.Evaluate(currentFuelFactor);
        fillColor.a = alpha;
        m_UIBarFill.color = fillColor;

        Color outlineColor = Color.white;
        outlineColor.a = alpha;
        m_UIBarOutline.color = outlineColor;

        m_UIBarScaler.transform.localScale = new Vector3(currentFuelFactor, 1.0f, 1.0f);

        m_UITransform.position = m_UIAnchor.position;
    }


    void EnableVisuals(bool a_Enable)
    {
        m_UITransform.gameObject.SetActive(a_Enable);
        m_JetpackModel.gameObject.SetActive(a_Enable);
    }

}
