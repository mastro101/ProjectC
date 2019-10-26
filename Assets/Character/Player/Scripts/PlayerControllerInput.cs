﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerInput : MonoBehaviour , ICommandController
{
    #region serialize
    [SerializeField] float speed;
    [SerializeField] float timeForSequence;
    [SerializeField] [Range(0f, 1f)] float slowMoPercent;
    [SerializeField] float timeForSlowMo;
    public BulletBase bullet;
    public CommandSequenceBase[] sequences;
    #endregion

    Rigidbody rb;

    public Vector3 aimDirection { get; private set; }

    public List<CommandSequenceBase> currentSequences { get; set; }
    CommandSequenceBase executedSequence;

    List<CommandSequenceBase> sequenceToRemove = new List<CommandSequenceBase>();

    bool sequenceStarted = false;
    int consecutiveButtonPressed = -1;
    bool buttonJustPressed = false;
    float remainTime;

    float slowMoRemainTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentSequences = new List<CommandSequenceBase>();
        foreach (var sequence in sequences)
        {
            sequence.Init(this);
            sequence.onStartSequence += s => StartSequence();
            sequence.onExecutedSequence += s => executedSequence = s;
            sequence.onCorrectInput += OnCorrectInput;
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

    void Movement()
    {
        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized * speed * Time.deltaTime, Space.World);
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

    void HandleFire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            BulletPoolManager.instance.TakeBullet(bullet).Shoot(transform.position, aimDirection);
        }
    }

    void HandleSlowMo()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Time.timeScale = slowMoPercent;
            slowMoRemainTime = Time.time + timeForSlowMo * slowMoPercent;
        }

        if (Input.GetMouseButton(1))
        {
            if (Time.time > slowMoRemainTime)
                Time.timeScale = 1;
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            Time.timeScale = 1;
        }
    }

    #region Sequence
    void HandleSequence()
    {
        buttonJustPressed = false;

        if (sequenceStarted == false)
        {
            foreach (var sequence in sequences)
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

        if (sequenceStarted == true && (Time.time > remainTime))
            ResetSequences();
    }

    void StartSequence()
    {
        if (sequenceStarted == false)
            sequenceStarted = true;
    }

    void OnCorrectInput(InputData input)
    {
        if (!buttonJustPressed)
        {
            consecutiveButtonPressed++;
            buttonJustPressed = true;
        }
        remainTime = Time.time + timeForSequence;
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
        Debug.Log("clean");
    } 
    #endregion
}