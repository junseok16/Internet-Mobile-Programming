using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPViewer : MonoBehaviour
{
    private MonsterHP mMonsterHP;
    private Slider mSlider;

    public void SetUp(MonsterHP monsterHP)
    {
        this.mMonsterHP = monsterHP;
        mSlider = GetComponent<Slider>();
    }

    private void Update()
    {
        mSlider.value = mMonsterHP.curHP / mMonsterHP.maxHP;
    }
}
