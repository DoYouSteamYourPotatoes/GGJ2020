using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum eNPCType
    {
        Dash=0,
        DoubleJump,
        Hook,
        Final,


        NONE = 100
    }
    public eNPCType npcType = eNPCType.NONE;

    public enum eTalk
    {
        NONE = 0,
        FirstTalk,
        BackNotComplete,
        BackComplete,
        HubPlatform
    }
    public eTalk talkStatus = eTalk.NONE;

    [SerializeField] private string npcName = "";
    private HUB ref_hub = null;

    protected bool inRangeToTalk = false;

    public virtual void Start()
    {
        ref_hub = FindObjectOfType<HUB>();
    }

    public void MoveToHub()
    {
        ref_hub.MoveNPCToSlot(npcType);
    }

    public virtual void AbleToTalk()
    {

    }

    public virtual void NotAbleToTalk()
    {

    }

    public virtual void Talk()
    {

    }
}
