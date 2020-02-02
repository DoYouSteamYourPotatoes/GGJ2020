using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_1 : NPC
{
    [SerializeField] private GameObject go_brokenEntity = null;
    [SerializeField] private GameObject go_repairedEntity = null;
    [SerializeField] private GameObject go_panTargetDash = null;
    [SerializeField] private SpriteRenderer npcGraphics = null;
    [SerializeField] private float shakeDuration = 1.0f;
    [SerializeField] private float shakeSpeed = 1.0f;
    [SerializeField] private float shakeAmount = 1.0f;
    [SerializeField] private Color col_canTalk = Color.white;
    [SerializeField] private GameObject go_fragmentDash = null;
    [SerializeField] private GameObject go_playerCrystalDash = null;
    [SerializeField] private AiCompanion_visual ref_companion = null;

    private Color col_original = Color.white;

    private Coroutine cor_ableToTalk = null;

    public override void Start()
    {
        base.Start();
        col_original = npcGraphics.color;
        talkStatus = eTalk.FirstTalk;
    }

    public override void AbleToTalk()
    {
        base.AbleToTalk();

        if (inRangeToTalk)
        {
            return;
        }

        if (cor_ableToTalk != null)
        {
            StopCoroutine(cor_ableToTalk);
        }
        cor_ableToTalk = StartCoroutine(AbleToTalkCoroutine());
        inRangeToTalk = true;
    }

    public override void NotAbleToTalk()
    {
        base.NotAbleToTalk();
        if (cor_ableToTalk != null)
        {
            StopCoroutine(cor_ableToTalk);
        }
        npcGraphics.color = col_original;
        inRangeToTalk = false;
    }

    public IEnumerator AbleToTalkCoroutine()
    {
        float timer = 0.0f;
        Color startColor = npcGraphics.color;
        Color endColor = col_canTalk;

        while (true)
        {
            timer = 0.0f;
            startColor = npcGraphics.color;
            endColor = col_canTalk;

            while (timer < 0.5f)
            {
                timer += Time.deltaTime;
                float t = timer / 0.5f;
                t = t * t * t * (t * (6f * t - 15f) + 10f);
                npcGraphics.color = Color.Lerp(startColor, endColor, t);
                yield return null;
            }

            timer = 0.0f;
            startColor = npcGraphics.color;
            endColor = col_original;
            while (timer < 0.5f)
            {
                timer += Time.deltaTime;
                float t = timer / 0.5f;
                t = t * t * t * (t * (6f * t - 15f) + 10f);
                npcGraphics.color = Color.Lerp(startColor, endColor, t);
                yield return null;
            }
        }
        yield return null;
    }

    public void QuestComplete()
    {
        talkStatus = eTalk.BackComplete;
        Debug.Log("Quest COmplete");
    }

    public override void Talk()
    {
        base.Talk();
        if (cor_ableToTalk != null)
        {
            StopCoroutine(cor_ableToTalk);
        }
        npcGraphics.color = col_original;

        switch (talkStatus)
        {
            case eTalk.NONE:
                break;
            case eTalk.FirstTalk:
                Manager_GameLogic.Instance.NPC1TalkSequence_1(NPCTalkSequence1);
                break;
            case eTalk.BackNotComplete:
                Manager_GameLogic.Instance.NPC1TalkSequence_2(NPCTalkSequence2);
                break;
            case eTalk.BackComplete:
                Manager_GameLogic.Instance.NPC1TalkSequence_3(NPCTalkSequence3,
                    delegate { go_brokenEntity.SetActive(false); go_repairedEntity.SetActive(true); });
                break;
            case eTalk.HubPlatform:
                Manager_GameLogic.Instance.NPC1TalkSequence_4();
                break;
            default:
                break;
        }
    }

    public void NPCTalkSequence1()
    {
        go_brokenEntity.SetActive(true);
        StartCoroutine(ShakeBrokenEntity(done =>
        {
            if (done)
            {
                Manager_GameLogic.Instance.NPC1TalkSequence_1_End();
                talkStatus = eTalk.BackNotComplete;
            }
        }));
    }

    public void NPCTalkSequence2()
    {
        go_brokenEntity.SetActive(true);
        StartCoroutine(ShakeBrokenEntity(done =>
        {
            if (done)
            {
                Manager_GameLogic.Instance.NPC1TalkSequence_1_End();
            }
        }));
    }

    public void NPCTalkSequence3()
    {
        StartCoroutine(GiveCrystalToPlayer(done =>
        {
            if (done)
            {
                StartCoroutine(PanCameraThenComeBack(value =>
                {
                    if (value)
                    {
                        Manager_GameLogic.Instance.NPC1TalkSequence_3_End();
                    }
                }, 1.0f, go_panTargetDash.transform));
            }
        }));

    }

    public IEnumerator ShakeBrokenEntity(Action<bool> done)
    {
        Vector3 startPos = go_brokenEntity.transform.position;
        Vector3 endPos = new Vector3(startPos.x + 1, startPos.y, startPos.z);
        float duration = 0.1f;

        yield return new WaitForSeconds(0.5f);
        go_brokenEntity.transform.position = endPos;
        yield return new WaitForSeconds(duration);
        go_brokenEntity.transform.position = startPos;
        yield return new WaitForSeconds(duration);
        go_brokenEntity.transform.position = endPos;
        yield return new WaitForSeconds(duration);
        go_brokenEntity.transform.position = startPos;
        yield return new WaitForSeconds(duration);
        go_brokenEntity.transform.position = endPos;
        yield return new WaitForSeconds(duration);
        go_brokenEntity.transform.position = startPos;
        yield return new WaitForSeconds(duration);
        go_brokenEntity.transform.position = endPos;
        yield return new WaitForSeconds(duration);
        go_brokenEntity.transform.position = startPos;
        yield return new WaitForSeconds(duration);
        go_brokenEntity.transform.position = endPos;
        yield return new WaitForSeconds(duration);
        go_brokenEntity.transform.position = startPos;
        yield return new WaitForSeconds(duration);
        go_brokenEntity.transform.position = endPos;
        yield return new WaitForSeconds(duration);
        go_brokenEntity.transform.position = startPos;
        yield return new WaitForSeconds(0.5f);

        done(true);
        yield return null;
    }

    private IEnumerator PanCameraThenComeBack(Action<bool> done, float pause, Transform target)
    {
        Camera ref_main = Camera.main;
        ref_main.GetComponent<BasicCameraTracker>().enabled = false;

        float timer = 0.0f;
        Vector3 startPos = ref_main.gameObject.transform.position;
        Vector3 endPos = target.position;

        while (timer < 1.25f)
        {
            timer += Time.deltaTime;
            float t = timer / 0.5f;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            ref_main.gameObject.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        yield return new WaitForSeconds(pause);
        timer = 0.0f;
        startPos = target.position;
        endPos = ref_main.gameObject.transform.position;
        while (timer < 1.25f)
        {
            timer += Time.deltaTime;
            float t = timer / 0.5f;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            ref_main.gameObject.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        ref_main.GetComponent<BasicCameraTracker>().enabled = true;
        done(true);
        yield return null;
    }

    private IEnumerator GiveCrystalToPlayer(Action<bool> done)
    {
        yield return new WaitForSeconds(0.25f);
        go_fragmentDash.SetActive(true);
        float timer = 0.0f;
        Vector3 startPos = go_fragmentDash.transform.position;
        Vector3 endPos = startPos;

        endPos.y += 5.0f;
        while (timer < 2.5f)
        {
            timer += Time.deltaTime;
            float t = timer / 2.5f;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            go_fragmentDash.transform.position = Vector3.Lerp(startPos, endPos, t);
            go_fragmentDash.transform.Rotate(new Vector3(0.0f, t, 0.0f));
            yield return null;
        }
        ref_companion.enabled = false;
        yield return new WaitForSeconds(0.25f);

        timer = 0.0f;
        startPos = go_fragmentDash.transform.position;
        endPos = ref_companion.gameObject.transform.position;
        endPos.z = 0;
        Quaternion startRot = go_fragmentDash.transform.rotation;
        Quaternion targetRot = ref_companion.gameObject.transform.rotation;

        while (timer < 1.0f)
        {
            timer += Time.deltaTime;
            float t = timer / 1.0f;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            go_fragmentDash.transform.position = Vector3.Lerp(startPos, endPos, t);
            go_fragmentDash.transform.rotation = Quaternion.Lerp(startRot, targetRot, t);
            yield return null;
        }

        go_playerCrystalDash.SetActive(true);
        Destroy(go_fragmentDash);
        yield return new WaitForSeconds(1.0f);
        ref_companion.enabled = true;
        done(true);

        yield return null;
    }
}
