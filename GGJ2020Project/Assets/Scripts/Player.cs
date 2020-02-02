using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 3.0f;
    private List<NPC> list_npcs = new List<NPC>();

    private Coroutine cor_CheckNPCs = null;
    private Coroutine cor_CheckBrokenPiece = null;

    private DashModule ref_dashModule = null;

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (list_npcs.Count > 0)
            {
                list_npcs.Sort((entry1, entry2) => Vector3.Distance(entry1.gameObject.transform.position, transform.position)
                .CompareTo(Vector3.Distance(entry2.gameObject.transform.position, transform.position)));
                list_npcs[0].Talk();
            }
        }
    }

    private void Start()
    {
        ref_dashModule = FindObjectOfType<DashModule>();
        cor_CheckNPCs = StartCoroutine(CheckForNPCsNearby());
        cor_CheckBrokenPiece = StartCoroutine(CheckForNPCsNearby());
    }

    private IEnumerator CheckForNPCsNearby()
    {
        while (true)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
            foreach (Collider item in hitColliders)
            {
                if (item.gameObject.tag.Equals("NPC"))
                {
                    NPC npcRef = item.gameObject.GetComponent<NPC>();
                    npcRef.AbleToTalk();
                    if (!list_npcs.Contains(npcRef))
                    {
                        list_npcs.Add(npcRef);
                    }
                }
            }

            List<NPC> toRemove = new List<NPC>();
            foreach (NPC item in list_npcs)
            {
                bool found = false;
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].gameObject.tag.Equals("NPC"))
                    {
                        if (item.Equals(hitColliders[i].gameObject.GetComponent<NPC>()))
                        {
                            found = true;
                            break;
                        }
                    }

                }
                if (!found)
                {
                    item.NotAbleToTalk();
                    toRemove.Add(item);
                }
            }

            foreach (NPC item in toRemove)
            {
                list_npcs.Remove(item);
            }
            yield return new WaitForSeconds(0.35f);
        }
    }

    public void LookForBrokenPiece()
    {
        Debug.Log("LookForBrokenPiece");
        if (cor_CheckBrokenPiece != null)
        {
            StopCoroutine(cor_CheckBrokenPiece);
        }
        cor_CheckBrokenPiece = StartCoroutine(CheckForBrokenPieceNearby());
    }

    private IEnumerator CheckForBrokenPieceNearby()
    {
        while (true)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
            foreach (Collider item in hitColliders)
            {
                BrokenPiece refPiece = item.gameObject.GetComponent<BrokenPiece>();
                if (refPiece)
                {
                    Debug.Log("PICKING UP PIECE");
                    refPiece.Pickup(BrokenPieceFound);
                    yield break;
                }
            }
            yield return null;
        }
    }

    private void BrokenPieceFound()
    {
        NPC_1 npc = FindObjectOfType<NPC_1>();
        npc.QuestComplete();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Breakable"))
        {
            Debug.Log("Breakable: " + collision.gameObject);
            if (ref_dashModule.isDashing)
            {
                ref_dashModule.m_HasDashedAndNotTouchedGroundYet = false;
                Destroy(collision.gameObject);
            }
        }
    }
}
