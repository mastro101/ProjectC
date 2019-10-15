using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerInput : MonoBehaviour , ICommandController
{
    #region serialize
    [SerializeField] float speed;
    [SerializeField] float timeForSequence;
    public CommandSequenceBase[] sequences;
    #endregion

    Rigidbody rb;

    public List<CommandSequenceBase> currentSequences { get; set; }
    CommandSequenceBase executedSequence;

    List<CommandSequenceBase> sequenceToRemove = new List<CommandSequenceBase>();

    bool sequenceStarted = false;
    int consecutiveButtonPressed = -1;
    bool buttonJustPressed = false;
    float remainTime;

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
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized * speed * Time.deltaTime, Space.World);
    }

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
            if(sequence.GetInput(consecutiveButtonPressed) != sequence.currentInput)
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
}