using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CommandMegaFlare : CommandSequenceBase
{
    [SerializeField] ParticleSystem flareVFXPrefab;

    ParticleSystem flare;

    public CommandMegaFlare(ICommandController controller, params InputData[] inputDatas) : base(controller, inputDatas)
    {
    }

    public override void Execute()
    {
        base.Execute();
        flare = Instantiate(flareVFXPrefab, controller.transform);
        flare.Play();
    }

    //TODO: need a Pool Manager
    IEnumerator DestroyVFX()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Destroy(flare);
        }
    }
}