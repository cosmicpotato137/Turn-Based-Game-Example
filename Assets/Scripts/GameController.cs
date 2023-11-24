using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    PlayerController player;
    public int spawnFrequency = 15;
    public GameObject enemyPrefab;
    public List<EnemyController> enemies;
    public int whoseTurn;
    int numPlayerTurns = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        enemies = new List<EnemyController>(FindObjectsOfType<EnemyController>());
        whoseTurn = 0;
        SetTurns();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // everything that happens when the turn changes
    public void ChangeTurn()
    {
        // add to whoseturn while keeping it in the range of # of enemies
        whoseTurn = (whoseTurn + 1) % (enemies.Count + 1);
        SetTurns();

        // spawn enemies before the player's turn
        if (numPlayerTurns % spawnFrequency == 0 && whoseTurn == 0)
        {
            GameObject e = Instantiate(enemyPrefab, new Vector3(7, 1, 0), Quaternion.identity);
            enemies.Add(e.GetComponent<EnemyController>());
        }
    }

    void SetTurns()
    {   
        // delete enemies that aren't alive
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].hp == 0)
            {
                Destroy(enemies[i].gameObject);
                enemies.RemoveAt(i);
                if (i < whoseTurn - 1)
                    whoseTurn -= 1;
            }
        }
        // update whoseturn
        whoseTurn = whoseTurn % (enemies.Count + 1);

        // change turns
        if (whoseTurn == 0)
        {
            player.SetTurn(true);
            numPlayerTurns += 1;

            foreach (EnemyController enemy in enemies)
            {
                enemy.SetTurn(false);
            }
        }
        else
        {
            player.SetTurn(false);
            foreach (EnemyController enemy in enemies)
            {
                enemy.SetTurn(false);
            }
            enemies[whoseTurn - 1].SetTurn(true);
        }

    }
}
