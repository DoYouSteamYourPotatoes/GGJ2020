using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUB : MonoBehaviour
{
    public enum eHubState
    {
        Default = 0,
        Repaired1,
        Repaired2,
        Repaired3,
        Final,

        None = 999
    }
    public eHubState hubState = eHubState.Default;

    public List<NPC> list_NPCs = new List<NPC>();
    public List<GameObject> list_npcSlots = new List<GameObject>();

    public List<bool> list_fragmentCompleted = new List<bool>();
    public List<GameObject> list_fragmentSlots = new List<GameObject>();
    public List<GameObject> list_playerFragments = new List<GameObject>();

    [Header("settings")]
    [SerializeField] private float fragmentToSlotDuration = 3.0f;

    private void Start()
    {
        list_fragmentCompleted = new List<bool>(new bool[4]);
    }

    public void ChangeState()
    {
        hubState = (eHubState)(((int)hubState) + 1);
        Debug.Log("changed to state: " + hubState.ToString());

        switch (hubState)
        {
            case eHubState.Default:
                break;
            case eHubState.Repaired1:
                StartCoroutine(MoveFragmentToSlot(0, list_playerFragments[0], list_fragmentSlots[0]));
                break;
            case eHubState.Repaired2:
                StartCoroutine(MoveFragmentToSlot(1, list_playerFragments[1], list_fragmentSlots[1]));
                break;
            case eHubState.Repaired3:
                StartCoroutine(MoveFragmentToSlot(2, list_playerFragments[2], list_fragmentSlots[2]));
                break;
            case eHubState.Final:
                StartCoroutine(MoveFragmentToSlot(3, list_playerFragments[3], list_fragmentSlots[3]));
                break;
            case eHubState.None:
                break;
            default:
                break;
        }
    }

    public void MoveNPCToSlot(NPC.eNPCType npcType)
    {
        Vector3 targetPos = list_npcSlots[(int)npcType].transform.position;
        GameObject goNPC = list_NPCs.Find(entry => entry.npcType.Equals(npcType)).gameObject;
        targetPos.y += goNPC.transform.localScale.y / 2.0f;
        goNPC.transform.position = targetPos;
    }

    public IEnumerator MoveFragmentToSlot(int boolIndex, GameObject obj, GameObject destination)
    {
        float timer = 0.0f;
        Vector3 startPos = obj.transform.position;
        Vector3 targetPos = destination.transform.position;
        targetPos.z = 0;
        Quaternion startRot = obj.transform.rotation;
        Quaternion targetRot = destination.transform.rotation;
        obj.GetComponent<MeshRenderer>().enabled = true;
        obj.GetComponent<Follow>().enabled = false;

        while (timer < fragmentToSlotDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fragmentToSlotDuration;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            obj.transform.position = Vector3.Lerp(startPos, targetPos, t);
            obj.transform.rotation = Quaternion.Lerp(startRot, targetRot, t);
            yield return null;
        }
        list_fragmentCompleted[boolIndex] = true;

        yield return null;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if ((int)Manager_GameLogic.Instance.crystalState > (int)hubState)
            {
                ChangeState();
            }
        }
    }
}
