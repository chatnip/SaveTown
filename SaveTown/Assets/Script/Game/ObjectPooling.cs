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

    private void SetupQueue() // 리스트의 프리팹을 큐에 넣은 후 비활성화
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

    public Enemy EnemyObjectPool() // Enemy 오브젝트를 꺼냄
    {
        var enemyObject = EnemyObjectesQueue.Dequeue();
        return enemyObject;
    }
    public void EnmeyObjectPick(Enemy enemyObject) // Enemy 오브젝트를 넣음
    {
        EnemyObjectesQueue.Enqueue(enemyObject);
        enemyObject.gameObject.SetActive(false);
    }

    public Arrow ArrowObjectPool() // Arrow 오브젝트를 꺼냄
    {
        var arrowObject = ArrowObjectesQueue.Dequeue();
        return arrowObject;
    }

    public void ArrowObjectPick(Arrow arrowObject) // Arrow 오브젝트를 넣음
    {
        ArrowObjectesQueue.Enqueue(arrowObject);
        arrowObject.gameObject.SetActive(false);
    }
}
