using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] List<Enemy> EnemyPrefabs = new List<Enemy>();
    [SerializeField] Queue<Enemy> EnemyObjectesQueue = new Queue<Enemy>();

    [SerializeField] List<Arrow> ArrowPrefabs = new List<Arrow>();
    [SerializeField] Queue<Arrow> ArrowObjectesQueue = new Queue<Arrow>();

    private void Awake()
    {
        SetupQueue();
    }

    private void SetupQueue() // ����Ʈ�� �������� ť�� ���� �� ��Ȱ��ȭ
    {
        for (int i = 0; i < EnemyPrefabs.Count; i++)
        {
            EnemyObjectesQueue.Enqueue(EnemyPrefabs[i]);
            EnemyPrefabs[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < ArrowPrefabs.Count; i++)
        {
            ArrowObjectesQueue.Enqueue(ArrowPrefabs[i]);
            ArrowPrefabs[i].gameObject.SetActive(false);
        }
    }

    public Enemy EnemyObjectPool() // Enemy ������Ʈ�� ����
    {
        var enemyObject = EnemyObjectesQueue.Dequeue();
        return enemyObject;
    }
    public void EnmeyObjectPick(Enemy enemyObject) // Enemy ������Ʈ�� ����
    {
        EnemyObjectesQueue.Enqueue(enemyObject);
        enemyObject.gameObject.SetActive(false);
    }

    public Arrow ArrowObjectPool() // Arrow ������Ʈ�� ����
    {
        var arrowObject = ArrowObjectesQueue.Dequeue();
        return arrowObject;
    }

    public void ArrowObjectPick(Arrow arrowObject) // Arrow ������Ʈ�� ����
    {
        ArrowObjectesQueue.Enqueue(arrowObject);
        arrowObject.gameObject.SetActive(false);
    }
}
