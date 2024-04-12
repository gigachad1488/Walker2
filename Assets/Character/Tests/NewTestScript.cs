using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    /*
    // A Test behaves as an ordinary method
    [Test]
    public void MovementTest()
    {
    }

    [UnityTest]
    public IEnumerator  MovementTestEnumerator()
    {
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));        
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.rb.velocity = new Vector3(1, 0, 0);
        float initialXPos = controller.transform.position.x;
        yield return new WaitForSeconds(1f);
        Assert.AreNotEqual(controller.transform.position.x, initialXPos);
        Object.Destroy(player);
        yield return null;
    }

    [UnityTest]
    public IEnumerator SwitchTestEnumerator()
    {
        GameObject player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        yield return new WaitForSeconds(1);
        PlayerAction action = player.GetComponent<PlayerAction>();
        string currentWeapon = action.gunSelector.gunType.ToString();
        action.SwitchinWeapon(1);
        yield return new WaitForSeconds(1f);
        Assert.AreNotEqual(action.gunSelector.gunType.ToString(), currentWeapon);
        Object.Destroy(player);
        yield return null;
    }

    [UnityTest]
    public IEnumerator PhysicsTestEnumerator()
    {
        GameObject enemy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy1"));
        EnemyHealth enemyHp = enemy.GetComponent<EnemyHealth>();
        Vector3 currentPos = enemy.transform.position;
        enemyHp.Death(new Vector3(50, 50, 0), 500);
        yield return new WaitForSeconds(1f);
        Assert.AreNotEqual(enemy.transform.position, currentPos);
        Object.Destroy(enemy);
        yield return null;
    }
    */
}
