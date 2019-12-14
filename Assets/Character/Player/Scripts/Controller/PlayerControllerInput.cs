using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerInput : MonoBehaviour , ICommandController
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

    public List<SetSequencesData> currentSequencesSet { get; private set; }
    SetSequencesData executedSequence;
    public List<InputData> currentInputSequence { get; private set; }

    bool sequenceStarted = false;
    int consecutiveButtonPressed = -1;
    bool buttonJustPressed = false;
    float sequenceRemainTime;

    #region Mono
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentSequencesSet = new List<SetSequencesData>();
        currentInputSequence = new List<InputData>();
        foreach (var sequence in playerData.sequences)
        {
            sequence.Init(this);
            sequence.onStartSequence    += StartSequence;
            //sequence.onCompletedSet   += OnCorrectSequence;
            sequence.onCompletedSection += OnCorrectSequence;
            foreach (var section in sequence.comboSections)
            {
                section.onCorrectInput  += OnCorrectInput;
            }
        }
    }

    private void OnDisable()
    {
        OnDestroy?.Invoke();
        foreach (var sequence in playerData.sequences)
        {
            sequence.ResetSequence();
            sequence.onStartSequence    -= StartSequence;
            //sequence.onCompletedSet   -= OnCorrectSequence;
            sequence.onCompletedSection -= OnCorrectSequence;
            foreach (var section in sequence.comboSections)
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
    }

    private void FixedUpdate()
    {
        Movement();
    }
    #endregion

    #region OtherInputHandler
    Vector3 stickAxis;
    Vector3 lookDirection;
    void Movement()
    {
        stickAxis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(stickAxis.normalized * playerData.speed * Time.deltaTime, Space.World);
        if ((stickAxis.x < -0.1f || stickAxis.x > 0.1f) || (stickAxis.z < -0.1f || stickAxis.z > 0.1f))
            lookDirection = Quaternion.LookRotation(stickAxis) * Vector3.forward;
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
                BulletPoolManager.instance.Shoot(playerData.bullet, shootPosition.position, lookDirection, this.gameObject);
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
    #endregion

    #region Sequence
    void HandleSequence()
    {
        buttonJustPressed = false;

        if (sequenceStarted == false)
        {
            HandleInput();
            foreach (var sequence in playerData.sequences)
            {
                sequence.HandleSetSequences();
            }
        }
        else
        {
            HandleInput();
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

    void StartSequence(SetSequencesData s)
    {
        if (sequenceStarted == false)
        {
            sequenceStarted = true;
        }
        currentSequencesSet.Add(s);
    }

    void OnCorrectSequence(SetSequencesData s)
    {
        executedSequence = s;
    }

    void OnCorrectInput(InputData input)
    {
        if (!buttonJustPressed)
        {
            currentInputSequence.Add(input);
            OnInputPressed?.Invoke(input);
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