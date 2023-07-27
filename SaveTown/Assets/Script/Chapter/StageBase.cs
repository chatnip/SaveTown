using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "New Stage", menuName = "SaveTown/Create New Stage")]
public class StageBase : ScriptableObject
{
    [SerializeField] EnemySet enemySet;
    [SerializeField] BossSet bossSet;
    [SerializeField] public bool haveDone;
    [SerializeField] public bool isClear;
    [SerializeField] public bool haveScenario;

    [ShowIf("haveScenario")]
    [SerializeField] public Scenario scenario;
    
    public EnemySet EnemySet
    {
        get { return enemySet; }
    }

    public BossSet BossSet
    {
        get { return bossSet; }
    }

    public bool HaveScenario
    {
        get { return haveScenario; }
    }
    public bool HaveDone
    {
        get { return haveDone; }
    }
    public bool IsClear
    {
        get { return IsClear; }
    }
}

[System.Serializable]
public struct EnemySet
{
    [SerializeField] public Sprite enemySprite;     // Enemy 스프라이트
    [SerializeField] public int enemyAmount;        // Enemy의 갯수
    [SerializeField] public int enemyTimeInterval;  // Enemy가 나오는 간격
    [SerializeField] public int enemySpeed;         // Enemy의 속도
    [SerializeField] public int arrowAmount;        // Arrow의 갯수
    [SerializeField] public int arrowSpriteNum;     // Arrow의 모양 갯수
    [SerializeField] public int arrowTextNum;       // Arrow의 숫자 텍스트
    [SerializeField] public int enemyDamage;        // Enemy가 주는 데미지

    public Sprite EnemySprite
    {
        get { return enemySprite; }
    }
    public int EnemyAmount
    {
        get { return enemyAmount; }
    }
    public int EnemyTimeInterval
    {
        get { return enemyTimeInterval; }
    }
    public int EnemySpeed
    {
        get { return enemySpeed; }
    }
    public int ArrowAmount
    {
        get { return arrowAmount; }
    }
    public int ArrowSpriteNum
    {
        get { return arrowSpriteNum; }
    }
    public int ArrowTextNum
    {
        get { return arrowTextNum; }
    }
    public int EnemyDamage
    {
        get { return enemyDamage; }
    }
}

[System.Serializable]
public struct BossSet
{
    [SerializeField] public Sprite bossEnemySprite;     // Boss 스프라이트
    [SerializeField] public int bossSpeed;              // Boss의 속도
    [SerializeField] public int bossArrowAmount;        // BossArrow의 갯수
    [SerializeField] public int bossArrowSpriteNum;     // BossArrow의 모양 갯수
    [SerializeField] public int bossArrowTextNum;       // BossArrow의 숫자 텍스트

    public Sprite BossEnemySprite
    {
        get { return bossEnemySprite; }
    }
    public int BossSpeed
    {
        get { return bossSpeed; }
    }
    public int BossArrowAmount
    {
        get { return bossArrowAmount; }
    }
    public int BossArrowSpriteNum
    {
        get { return bossArrowSpriteNum; }
    }
    public int BossArrowTextNum
    {
        get { return bossArrowTextNum; }
    }
}

[System.Serializable]
public struct Scenario
{
    [SerializeField] ScenarioBase scenarioBase;     // 시나리오
    public ScenarioBase ScenarioBase
    {
        get { return scenarioBase; }
    }
}