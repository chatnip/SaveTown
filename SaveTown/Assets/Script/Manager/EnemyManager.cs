using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyManager : MonoBehaviour
{
    [Header("*Component")]
    [SerializeField] GameManager GameManager;
    [SerializeField] KeyPadManager KeyPadManager;
    [SerializeField] ObjectPooling ObjectPooling;
    [SerializeField] GameSystem GameSystem;

    [Header("*Arrow")]
    [SerializeField] public List<Sprite> arrowSprite = new List<Sprite>();

    [HideInInspector] public Queue<Enemy> EnemyQueue = new Queue<Enemy>();
    [HideInInspector] public bool isEndGame;

    private void Awake()
    {
        isEndGame = false;
    }

    public IEnumerator EnemyAmount() // 활성화 될 Enemy 수
    {
        Debug.Log("실행");
        if (isEndGame == true)
        {
            yield break;
        }
        for (int i = 0; i < GameSystem.i_enemyAmount; i++)
        {
            StartCoroutine(CreateEnemy());
            yield return new WaitForSeconds(GameSystem.i_enemyTimeInterval);
        }
    }

    private IEnumerator CreateEnemy() // Enemy 활성화
    {
        if (isEndGame == true)
        {
            yield break;
        }
        Debug.Log("적 활성화");
        yield return new WaitForSeconds(0.2f);
        Enemy enemy = ObjectPooling.EnemyObjectPool();
        EnemyQueue.Enqueue(enemy);
        EnemySetting(enemy);
        enemy.gameObject.SetActive(true);
    }

    private void EnemySetting(Enemy enemy) // Enemy 설정
    {
        enemy.type = Type.Nomal;
        int randomPosition = Random.Range(-200, 200);
        enemy.transform.localPosition = new Vector3(randomPosition, 710, 0);
        enemy.enemyImage.sprite = GameSystem.s_enemySprite;
    }

    public IEnumerator CreateBoss()
    {
        yield return new WaitForSeconds(2);
        Enemy enemy = ObjectPooling.EnemyObjectPool();
        EnemyQueue.Enqueue(enemy);
        BossSetting(enemy);
        enemy.gameObject.SetActive(true);
    }
   
    private void BossSetting(Enemy enemy) // Boss 설정
    {
        enemy.type = Type.Boss;
        enemy.transform.localPosition = new Vector3(0, 710, 0);
        enemy.enemyImage.sprite = GameSystem.s_bossEnemySprite;
    }
}

[System.Serializable]
public enum Type
{
    Nomal,
    Boss
}