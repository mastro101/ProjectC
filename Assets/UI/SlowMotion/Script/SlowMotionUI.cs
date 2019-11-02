using UnityEngine;
using UnityEngine.UI;

public class SlowMotionUI : MonoBehaviour
{
    [SerializeField] PlayerData player;
    [SerializeField] Image slider;

    float remainTime;

    private void Awake()
    {
        player.OnSlowMoUsed += UpdateSlowMotionTimer;
    }

    public void UpdateSlowMotionTimer(float _remainTime)
    {
        slider.fillAmount = (float)player.slowMoRemainTime / ((float)player.timeForSlowMo * player.slowMoPercent);
    }
}