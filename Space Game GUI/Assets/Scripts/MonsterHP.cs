using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHP : MonoBehaviour
{
    [SerializeField] private float mMaxHP = 0.0f;
    private float mCurHP = 0.0f;
    private Monster mMonster;
    private SpriteRenderer mSpriteRenderer;

    public float maxHP => mMaxHP;
    public float curHP => mCurHP;

    private void Awake()
    {
        mCurHP = mMaxHP;
        mMonster = GetComponent<Monster>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        mCurHP -= damage;
        StopCoroutine("coroutineColor");
        StartCoroutine("coroutineColor");

        if (mCurHP <= 0)
        {
            mMonster.MonsterDie();
        }
    }

    private IEnumerator coroutineColor()
    {
        mSpriteRenderer.color = Color.gray;
        yield return new WaitForSeconds(0.05f);
        mSpriteRenderer.color = Color.white;
    }
}
