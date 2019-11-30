using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerData))]
public class SlowMotionController : MonoBehaviour
{
    [SerializeField] bool EnableOnCoolDown;

    PlayerData playerData;

    bool canSlow;
    bool timeSlowed;
    float timerToPressAndRelease = .2f;
    bool fakedHold;

    IEnumerator buttonCorutine;
    IEnumerator slowMoCourutine;
    IEnumerator refillSlowMoCorutine;

    private void Awake()
    {
        playerData = GetComponent<PlayerData>();
        if (!EnableOnCoolDown)
        {
            playerData.OnRefilled += EnableSlowMo;
            playerData.OnSlowMoStarted += DisableSlowMo;
        }
        EnableSlowMo();
    }

    private void OnDisable()
    {
        if (!EnableOnCoolDown)
        {
            playerData.OnRefilled -= EnableSlowMo;
            playerData.OnSlowMoStarted -= DisableSlowMo;
        }
    }

    private void Start()
    {
        buttonCorutine = HoldButtonCorutine();
        slowMoCourutine = SlowMo();
        refillSlowMoCorutine = RefillSlowMo();
        playerData.slowMoRemainTime = playerData.timeForSlowMo * playerData.slowMoPercent;
    }

    private void Update()
    {
        HandleSlowMo();
    }

    void ResetSlowMo()
    {
        Time.timeScale = 1;
        playerData.slowMoRemainTime = playerData.timeForSlowMo * playerData.slowMoPercent;
        playerData.slowMoRemainTime = playerData.timeForSlowMo * playerData.slowMoPercent;
        timeSlowed = false;
    }

    void EnableSlowMo()
    {
        canSlow = true;
    }
    
    void DisableSlowMo()
    {
        canSlow = false;
    }

    void HandleSlowMo()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.Joystick1Button5))
        {
            if (fakedHold == false)
            {
                if (canSlow == true)
                {
                    fakedHold = true;
                    Time.timeScale = playerData.slowMoPercent;
                    timeSlowed = true;
                    StartCoroutine(buttonCorutine);
                    StartCoroutine(slowMoCourutine);
                    StopCoroutine(refillSlowMoCorutine);
                    refillSlowMoCorutine = RefillSlowMo();
                }
            }
            else
            {
                fakedHold = false;
                if (timeSlowed)
                {
                    StopCoroutine(slowMoCourutine);
                    slowMoCourutine = SlowMo();
                    StartCoroutine(refillSlowMoCorutine);
                }
            }
        }

        if (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.Joystick1Button4) || Input.GetKeyUp(KeyCode.Joystick1Button5))
        {
            if (fakedHold == true)
            {
                StopCoroutine(buttonCorutine);
            }
            else
            {
                if (timeSlowed)
                {
                    StopCoroutine(slowMoCourutine);
                    slowMoCourutine = SlowMo();
                    StartCoroutine(refillSlowMoCorutine);
                }
            }
            buttonCorutine = HoldButtonCorutine();
        }
    }

    #region Corutine
    IEnumerator SlowMo()
    {
        playerData.OnSlowMoStarted?.Invoke();
        while (playerData.slowMoRemainTime > 0)
        {
            if (timeSlowed == true)
                playerData.slowMoRemainTime -= Time.deltaTime;
            yield return null;
        }
        StartCoroutine(refillSlowMoCorutine);
        slowMoCourutine = SlowMo();
    }

    IEnumerator HoldButtonCorutine()
    {
        yield return new WaitForSeconds(timerToPressAndRelease);
        fakedHold = false;
        buttonCorutine = HoldButtonCorutine();
    }

    IEnumerator RefillSlowMo()
    {
        Time.timeScale = 1;
        timeSlowed = false;
        while (playerData.slowMoRemainTime < playerData.timeForSlowMo * playerData.slowMoPercent)
        {
            if (timeSlowed == false)
                playerData.slowMoRemainTime += Time.deltaTime;
            yield return null;
        }
        refillSlowMoCorutine = RefillSlowMo();
        playerData.OnRefilled?.Invoke();
    } 
    #endregion
}
