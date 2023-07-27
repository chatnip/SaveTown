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
    [SerializeField] public Sprite enemySprite;     // Enemy ��������Ʈ
    [SerializeField] public int enemyAmount;        // Enemy�� ����
    [SerializeField] public int enemyTimeInterval;  // Enemy�� ������ ����
    [SerializeField] public int enemySpeed;         // Enemy�� �ӵ�
    [SerializeField] public int arrowAmount;        // Arrow�� ����
    [SerializeField] public int arrowSpriteNum;     // Arrow�� ��� ����
    [SerializeField] public int arrowTextNum;       // Arrow�� ���� �ؽ�Ʈ
    [SerializeField] public int enemyDamage;        // Enemy�� �ִ� ������

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
    [SerializeField] public Sprite bossEnemySprite;     // Boss ��������Ʈ
    [SerializeField] public int bossSpeed;              // Boss�� �ӵ�
    [SerializeField] public int bossArrowAmount;        // BossArrow�� ����
    [SerializeField] public int bossArrowSpriteNum;     // BossArrow�� ��� ����
    [SerializeField] public int bossArrowTextNum;       // BossArrow�� ���� �ؽ�Ʈ

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
    [SerializeField] ScenarioBase scenarioBase;     // �ó�����
    public ScenarioBase ScenarioBase
    {
        get { return scenarioBase; }
    }
}