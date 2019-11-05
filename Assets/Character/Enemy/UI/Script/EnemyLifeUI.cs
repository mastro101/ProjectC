using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLifeUI : MonoBehaviour
{
    [SerializeField] CharacterBase enemy;
    [SerializeField] Image slider;

    private void Start()
    {
        enemy.OnDamage += FillSlider;
    }

    void FillSlider(int _damage)
    {
        slider.fillAmount = (float)enemy.currentHealth / (float)enemy.maxHealth;
    }
}