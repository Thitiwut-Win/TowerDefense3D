using System.Collections;
using UnityEngine;

public class BlackSheep : BaseEnemy
{
    private bool isCharging = false;
    protected override void Update()
    {
        base.Update();
        if (!isCharging)
        {
            StartCoroutine(Charge());
        }
    }
    private IEnumerator Charge()
    {
        isCharging = true;
        agent.speed = 6;
        yield return new WaitForSeconds(4);
        agent.speed = 2;
        yield return new WaitForSeconds(2);
        isCharging = false;
    }
}
