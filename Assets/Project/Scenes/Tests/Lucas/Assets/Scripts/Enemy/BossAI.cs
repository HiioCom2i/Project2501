using UnityEngine;
using UnityEngine.SceneManagement;

public class BossAI : EnemyAI
{
    protected override void Die()
    {
        SceneManager.LoadScene("GameWin");
    }

}