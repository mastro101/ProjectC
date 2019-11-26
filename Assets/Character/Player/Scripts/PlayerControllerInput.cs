using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerInput : MonoBehaviour , ICommandController
{
    [SerializeField] PlayerData playerData;
    [SerializeField] Transform shootPosition;

    Rigidbody rb;

    public Vector3 aimDirection { get; private set; }

    public List<CommandSequenceBase> currentSequences { get; set; }
    CommandSequenceBase executedSequence;

    List<CommandSequenceBase> sequenceToRemove = new List<CommandSequenceBase>();

    bool sequenceStarted = false;
    int consecutiveButtonPressed = -1;
    bool buttonJustPressed = false;
    float sequenceRemainTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentSequences = new List<CommandSequenceBase>();
        foreach (var sequence in playerData.sequences)
        {
            sequence.Init(this);
            sequence.onStartSequence += StartSequence;
            sequence.onExecutedSequence += OnCorrectSequence;
            sequence.onCorrectInput += OnCorrectInput;
        }
    }

    private void OnDisable()
    {
        foreach (var sequence in playerData.sequences)
        {
            sequence.onStartSequence -= StartSequence;
            sequence.onExecutedSequence -= OnCorrectSequence;
            sequence.onCorrectInput -= OnCorrectInput;
        }
    }

    private void Update()
    {
        HandleSequence();
        Aim();
        HandleFire();
        HandleSlowMo();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    Vector3 stickAxis;
    void Movement()
    {
        stickAxis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(stickAxis.normalized * playerData.speed * Time.deltaTime, Space.World);
        if ((stickAxis.x < -0.1f || stickAxis.x > 0.1f) || (stickAxis.z < -0.1f || stickAxis.z > 0.1f))
            transform.rotation = Quaternion.LookRotation(stickAxis);
    }

    void Aim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
        {
            Vector3 raycastPoint = raycastHit.point;
            Vector3 pointOnPlayerHight = new Vector3(raycastPoint.x, transform.position.y, raycastPoint.z);
            Vector3 _direction = pointOnPlayerHight - transform.position;
            aimDirection = _direction.normalized;
        }
    }

    bool shooted;
    void HandleFire()
    {
        if (BulletPoolManager.instance != null)
        {
            if (Input.GetAxis("Fire1") > 0 && shooted == false)
            {
                shooted = true;
                BulletPoolManager.instance.Shoot(playerData.bullet, shootPosition.position, transform.forward, this.gameObject);
            }
            else if (Input.GetAxis("Fire1") <= 0 && shooted == true)
            {
                shooted = false;
            }

            if (Input.GetButtonDown("Fire2"))
            {
                BulletPoolManager.instance.Shoot(playerData.bullet, shootPosition.position, aimDirection, this.gameObject);
            }
        }
    }

    bool timeSlowed;
    void HandleSlowMo()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.Joystick1Button5))
        {
            Time.timeScale = playerData.slowMoPercent;
            timeSlowed = true;
            playerData.slowMoRemainTime = playerData.timeForSlowMo * playerData.slowMoPercent;
        }

        if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Joystick1Button4) || Input.GetKey(KeyCode.Joystick1Button5))
        {
            if (timeSlowed == true)
                playerData.slowMoRemainTime -= Time.deltaTime;
            
            if (playerData.slowMoRemainTime < 0 && timeSlowed == true)
            {
                ResetSlowMo();
            }
        }
        
        if (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.Joystick1Button4) || Input.GetKeyUp(KeyCode.Joystick1Button5))
        {
            ResetSlowMo();
        }
    }

    void ResetSlowMo()
    {
        Time.timeScale = 1;
        playerData.slowMoRemainTime = playerData.timeForSlowMo * playerData.slowMoPercent;
        playerData.slowMoRemainTime = playerData.timeForSlowMo * playerData.slowMoPercent;
        timeSlowed = false;
    }

    #region Sequence
    void HandleSequence()
    {
        buttonJustPressed = false;

        if (sequenceStarted == false)
        {
            foreach (var sequence in playerData.sequences)
            {
                sequence.HandleInputSequence();
            }
        }
        else
        {
            foreach (var sequence in currentSequences)
            {
                sequence.HandleInputSequence();
            }

            if (executedSequence != null)
                if (currentSequences.Contains(executedSequence))
                {
                    currentSequences.Remove(executedSequence);
                    executedSequence = null;
                }

            if (sequenceToRemove.Count > 0)
            {
                int l = sequenceToRemove.Count;
                for (int i = 0; i < l; i++)
                {
                    if (currentSequences.Contains(sequenceToRemove[i]))
                    {
                        sequenceToRemove[i].ResetSequence();
                        currentSequences.Remove(sequenceToRemove[i]);
                    }
                }
                sequenceToRemove.Clear();
            }

            if (currentSequences.Count == 0)
                ResetSequences();
        }

        if (sequenceStarted == true && (Time.time > sequenceRemainTime))
            ResetSequences();
    }

    void StartSequence(CommandSequenceBase s)
    {
        if (sequenceStarted == false)
            sequenceStarted = true;
    }

    void OnCorrectSequence(CommandSequenceBase s)
    {
        executedSequence = s;
    }

    void OnCorrectInput(InputData input)
    {
        if (!buttonJustPressed)
        {
            consecutiveButtonPressed++;
            buttonJustPressed = true;
        }
        sequenceRemainTime = Time.time + playerData.timeForSequence;
        foreach (var sequence in currentSequences)
        {
            if (sequence.GetInput(consecutiveButtonPressed) != sequence.currentInput)
            {
                sequenceToRemove.Add(sequence);
            }
        }
    }

    void ResetSequences()
    {
        consecutiveButtonPressed = -1;
        foreach (var sequence in currentSequences)
        {
            sequence.ResetSequence();
        }
        currentSequences.Clear();
        sequenceStarted = false;
    } 
    #endregion
}