using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager_GameLogic : MonoBehaviour
{
    #region Singleton
    public static Manager_GameLogic Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(Manager_GameLogic)) as Manager_GameLogic;

            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static Manager_GameLogic instance;
    #endregion

    public enum eAbilities
    {
        WallJump = 0,
        WallSlide,
        NetClimb,
        DropDown,
        Swim,

        Dash = 100,
        DoubleJump,
        Hook,

        None
    }
    public enum eCrystals
    {
        None = 0,
        Initial,
        Second,
        Third,
        Fourth
    }
    public eCrystals crystalState = eCrystals.None;

    [SerializeField] private AbilityModuleManager moduleManager = null;
    [SerializeField] private Image img_fadeBlack = null;


    [Header("settings")]
    [SerializeField] private float zoomInOrthoSize = 8.0f;
    [SerializeField] private float regularOrthoSize = 12.0f;
    [SerializeField] private float zoomOutOrthoSize = 15.0f;
    [SerializeField] private float zoomDuration = 15.0f;

    private Camera ref_cam = null;
    private Player ref_player = null;

    private void Start()
    {
        moduleManager = FindObjectOfType<AbilityModuleManager>();
        LockAbility(eAbilities.Dash);
        LockAbility(eAbilities.DoubleJump);
        LockAbility(eAbilities.Hook);
        ref_cam = Camera.main;
        ref_player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ObtainAbility1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ObtainAbility2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ObtainAbility3();
        }
    }

    public void ObtainAbility1()
    {
        crystalState = crystalState + 1;
        UnlockAbility(eAbilities.Dash);
        Debug.Log("Dash Unlocked");
    }

    public void ObtainAbility2()
    {
        crystalState = crystalState + 1;
        UnlockAbility(eAbilities.DoubleJump);
        Debug.Log("DoubleJump Unlocked");
    }

    public void ObtainAbility3()
    {
        crystalState = crystalState + 1;
        UnlockAbility(eAbilities.Hook);
        Debug.Log("Hook Unlocked");
    }

    public void ObtainAbility4()
    {
        crystalState = crystalState + 1;
    }

    public void LockAbility(eAbilities toUnlock)
    {
        moduleManager.GetModuleWithID(toUnlock).SetLocked(true);
    }
    public void UnlockAbility(eAbilities toUnlock)
    {
        moduleManager.GetModuleWithID(toUnlock).SetLocked(false);
    }

    public IEnumerator EnterZoominMode(Action<bool> callback)
    {
        float timer = 0.0f;
        float startSize = ref_cam.orthographicSize;
        float targetSize = zoomInOrthoSize;

        while (timer < zoomDuration)
        {
            timer += Time.deltaTime;
            float t = timer / zoomDuration;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            ref_cam.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
            yield return null;
        }
        callback(true);

        yield return null;
    }

    public IEnumerator EnterZoomoutMode(Action<bool> callback)
    {
        float timer = 0.0f;
        float startSize = ref_cam.orthographicSize;
        float targetSize = regularOrthoSize;

        while (timer < zoomDuration)
        {
            timer += Time.deltaTime;
            float t = timer / zoomDuration;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            ref_cam.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
            yield return null;
        }
        callback(true);

        yield return null;
    }

    public void NPC1TalkSequence_1(Action callback)
    {
        StartCoroutine(EnterZoominMode(value =>
        {
            if (value)
            {
                //do stuff...
                callback();
            }
        }));
    }

    public void NPC1TalkSequence_1_End()
    {
        StartCoroutine(EnterZoomoutMode(value =>
        {
            if (value)
            {
                //do stuff...
                ref_player.LookForBrokenPiece();
            }
        }));
    }

    public void NPC1TalkSequence_2(Action callback)
    {
        StartCoroutine(EnterZoominMode(value =>
        {
            if (value)
            {
                //do stuff...
                callback();
            }
        }));
    }
    public void NPC1TalkSequence_3(Action doneCallback, Action midCallback)
    {
        StartCoroutine(EnterZoominMode(value =>
        {
            if (value)
            {
                //do stuff...
                StartCoroutine(FadeOutThenIn(paused =>
                {
                    if (paused)
                    {
                        //play the repair sounds here
                        midCallback();
                    }
                }, done =>
                {
                    if (done)
                    {
                        doneCallback();
                    }
                }, 2.0f));
            }
        }));
    }

    public void NPC1TalkSequence_3_End()
    {
        StartCoroutine(EnterZoomoutMode(value =>
        {
            if(value)
            {
                ObtainAbility1();
            }
        }));
    }

    public void NPC1TalkSequence_4()
    {
        //At HUB
    }

    private IEnumerator FadeToBlack(Action<bool> done)
    {
        float timer = 0.0f;
        float startAlpha = 0.0f;
        float endAlpha = 1.0f;

        while (timer < 0.35f)
        {
            timer += Time.deltaTime;
            float t = timer / 0.35f;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            Color targetColor = new Color(0.0f, 0.0f, 0.0f, Mathf.Lerp(startAlpha, endAlpha, t));
            img_fadeBlack.color = targetColor;
            yield return null;
        }

        done(true);
        yield return null;
    }

    private IEnumerator FadeIn(Action<bool> done)
    {
        float timer = 0.0f;
        float startAlpha = 1.0f;
        float endAlpha = 0.0f;

        while (timer < 0.35f)
        {
            timer += Time.deltaTime;
            float t = timer / 0.35f;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            Color targetColor = new Color(0.0f, 0.0f, 0.0f, Mathf.Lerp(startAlpha, endAlpha, t));
            img_fadeBlack.color = targetColor;
            yield return null;
        }

        done(true);
        yield return null;
    }

    private IEnumerator FadeOutThenIn(Action<bool> callBack, Action<bool> done, float pause)
    {
        yield return StartCoroutine(FadeToBlack(value => { }));
        callBack(true);
        yield return new WaitForSeconds(pause);
        yield return StartCoroutine(FadeIn(value => { }));
        done(true);
    }
}
