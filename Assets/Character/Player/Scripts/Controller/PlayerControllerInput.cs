using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerInput : MonoBehaviour , IShooter
{
    [SerializeField] InputContainer inputsData;
    [SerializeField] PlayerData playerData;
    [SerializeField] Transform shootPosition;
    [SerializeField] SpriteRenderer spriteCharacter;

    Rigidbody rb;

    public Vector3 aimDirection { get; private set; }

    public System.Action OnDestroy { get; set; }
    public System.Action<InputData> OnInputPressed { get; set; }
    public System.Action OnInputReset { get; set; }

    public List<SetSequences> sequences { get; private set; }
    public List<SetSequences> currentSequencesSet { get; private set; }
    public List<SetSequences> sequencesToRemove;
    SetSequences executedSequence;
    public List<InputData> currentInputSequence { get; private set; }

    public InputData currentInput { get; private set; }

    bool sequenceStarted = false;
    int consecutiveButtonPressed = -1;
    bool buttonJustPressed = false;
    float sequenceRemainTime;

    #region Mono
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        sequences = new List<SetSequences>(); 
        currentSequencesSet = new List<SetSequences>();
        currentInputSequence = new List<InputData>();
        sequencesToRemove = new List<SetSequences>();
        foreach (var sequenceData in playerData.sequences)
        {
            SetSequences sequence = new SetSequences(sequenceData, this, this);
            sequence.onStartSequence    += StartSequence;
            //sequence.onCompletedSet   += OnCorrectSequence;
            sequence.onCompletedSection += OnCorrectSequence;
            foreach (var section in sequence.commands)
            {
                section.onCorrectInput  += OnCorrectInput;
            }
            sequences.Add(sequence);
        }
    }

    private void OnDisable()
    {
        OnDestroy?.Invoke();
        foreach (var sequence in sequences)
        {
            sequence.ResetSequence();
            sequence.onStartSequence    -= StartSequence;
            //sequence.onCompletedSet   -= OnCorrectSequence;
            sequence.onCompletedSection -= OnCorrectSequence;
            foreach (var section in sequence.commands)
            {
                section.onCorrectInput  -= OnCorrectInput;
            }
        }
    }

    private void Update()
    {
        HandleSequence();
        Aim();
        HandleFire();
        HandleDash();
    }

    private void FixedUpdate()
    {
        Movement();
    }
    #endregion

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            playerData.TempInvulnerability(5f);
    }

    #region OtherInputHandler
    bool usingJoypad = false;

    Vector3 stickAxis;
    Vector3 keyAxis;
    Vector3 stickDirection;
    Vector3 transformVelocity;
    void Movement()
    {
        stickAxis = new Vector3(Input.GetAxis("HorizontalStick"), 0, Input.GetAxis("VerticalStick"));
        keyAxis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (usingJoypad)
            transformVelocity = stickAxis.normalized * playerData.speed * Time.deltaTime;
        else
            transformVelocity = keyAxis.normalized * playerData.speed * Time.deltaTime;

        transform.Translate(transformVelocity, Space.World);
        if ((stickAxis.x < -0.1f || stickAxis.x > 0.1f) || (stickAxis.z < -0.1f || stickAxis.z > 0.1f))
            stickDirection = Quaternion.LookRotation(stickAxis) * Vector3.forward;
        if (spriteCharacter)
        {
            if (stickAxis.x < 0 && spriteCharacter.flipX == true)
            {
                spriteCharacter.flipX = false;
            }
            else if (stickAxis.x > 0 && spriteCharacter.flipX == false)
            {
                spriteCharacter.flipX = true;
            }
        }
    }

    Vector2 mouseVelocity;
    void Aim()
    {
        mouseVelocity = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if ((stickAxis.x < -0.1f || stickAxis.x > 0.1f) || (stickAxis.z < -0.1f || stickAxis.z > 0.1f))
        {
            aimDirection = stickDirection;
            usingJoypad = true;
            Debug.Log("joypad");
        }

        if ((mouseVelocity.x < -0.1f || mouseVelocity.x > 0.1f || mouseVelocity.y < -0.1f || mouseVelocity.y > 0.1f ||
            keyAxis.x < -0.1f || keyAxis.x > 0.1f || keyAxis.y < -0.1f || keyAxis.y > 0.1f || Input.GetKeyDown(KeyCode.Mouse0)) && usingJoypad == true)
        {
            usingJoypad = false;
            Debug.Log("mouse");
        }

        if (usingJoypad == false)
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
    }

    bool shooted;
    void HandleFire()
    {
        if (BulletPoolManager.instance != null)
        {
            if (Input.GetAxis("Fire1") > 0 && shooted == false)
            {
                shooted = true;
                BulletPoolManager.instance.Shoot(playerData.bullet, shootPosition.position, aimDirection, this);
            }
            else if (Input.GetAxis("Fire1") <= 0 && shooted == true)
            {
                shooted = false;
            }

            if (Input.GetButtonDown("Fire2"))
            {
                BulletPoolManager.instance.Shoot(playerData.bullet, shootPosition.position, aimDirection, this);
            }
        }
    } 
    #endregion

    #region Sequence
    void HandleSequence()
    {
        buttonJustPressed = false;

        if (sequenceStarted == false)
        {
            HandleInput();
            foreach (var sequence in sequences)
            {
                sequence.HandleSetSequences();
            }
        }
        else
        {
            HandleInput();
            foreach (var sequence in sequencesToRemove)
            {
                currentSequencesSet.Remove(sequence);
            }
            sequencesToRemove.Clear();

            foreach (var sequence in currentSequencesSet)
            {
                sequence.HandleSetSequences();
            }
        }

        if (sequenceStarted == true && (Time.time > sequenceRemainTime))
        {
             ExecuteSequence();
        }
    }

    void HandleInput()
    {
        if (Input.anyKeyDown)
        {
            foreach (var input in inputsData.inputs)
            {
                if (input.CheckInputPressed())
                {
                    if (sequenceStarted == false)
                    {
                        sequenceStarted = true;
                    }
                    currentInputSequence.Add(input);
                    OnInputPressed?.Invoke(input);
                    buttonJustPressed = true;
                    sequenceRemainTime = Time.time + playerData.timeForSequence;
                }
            }
        }
    }

    void StartSequence(SetSequences s)
    {
        if (sequenceStarted == false)
        {
            sequenceStarted = true;
        }
        currentSequencesSet.Add(s);
    }

    void OnCorrectSequence(SetSequences s)
    {
        executedSequence = s;
    }

    void OnCorrectInput(InputData input)
    {
        if (!buttonJustPressed)
        {
            currentInputSequence.Add(input);
            //OnInputPressed?.Invoke(input);
            consecutiveButtonPressed++;
            buttonJustPressed = true;
        }
        sequenceRemainTime = Time.time + playerData.timeForSequence;
    }

    void ResetSequences()
    {
        consecutiveButtonPressed = -1;
        foreach (var sequence in currentSequencesSet)
        {
            sequence.ResetSequence();
        }
        currentSequencesSet.Clear();
        currentInputSequence.Clear();
        OnInputReset?.Invoke();
        sequenceStarted = false;
    }

    void ExecuteSequence()
    {
        if (executedSequence != null)
        {
            if (currentSequencesSet.Contains(executedSequence))
            {
                executedSequence.Execute();
                executedSequence = null;
            }
        }
        ResetSequences();
    }
    #endregion
}