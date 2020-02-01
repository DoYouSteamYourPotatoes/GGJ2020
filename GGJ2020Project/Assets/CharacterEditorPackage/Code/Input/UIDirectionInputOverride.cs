using UnityEngine;
using System.Collections;

public class UIDirectionInputOverride : MonoBehaviour {
    [SerializeField] PlayerInput m_PlayerInput;
    [SerializeField] string m_InputOverrideName;
	Vector2 m_CurrentInput;

    public void LeftGuiButtonDown()
    {
		Debug.Log (m_CurrentInput);
		m_CurrentInput.x -= 1.0f;
		m_PlayerInput.GetDirectionInput(m_InputOverrideName).SetOverride(true, m_CurrentInput);
    }
    public void LeftGuiButtonUp()
    {
		m_CurrentInput.x += 1.0f;
		m_PlayerInput.GetDirectionInput(m_InputOverrideName).SetOverride(true, m_CurrentInput);
    }
	public void RightGuiButtonDown()
	{
		m_CurrentInput.x += 1.0f;
		m_PlayerInput.GetDirectionInput(m_InputOverrideName).SetOverride(true, m_CurrentInput);
	}
	public void RightGuiButtonUp()
	{
		m_CurrentInput.x -= 1.0f;
		m_PlayerInput.GetDirectionInput(m_InputOverrideName).SetOverride(true, m_CurrentInput);
	}
}
