using System.Collections;
using UnityEngine;

public class NPC : Player
{

    protected override void OnEnable()
    {
        EventManager.Instance.Subscribe("GirlSinging", OnGirlSinging);
    }

    private void OnGirlSinging(object message)
    {
        StopAllCoroutines();
        StartCoroutine(MoveForSeconds((float)message));
    }

    IEnumerator MoveForSeconds(float secondsToMove)
    {
        if (IsSafe) yield break;
        
        float randomStartDelay = Random.Range(0f, 1f);
        float randomStopDelay = Random.Range(0f, 1f);

        
        yield return new WaitForSeconds(randomStartDelay);

        
        float moveDuration = secondsToMove - 1.64f - randomStopDelay; 
        if (moveDuration > 0f)
        {
            playerAnimator.SetBool("running", true);
            currentMovement = Vector3.forward; 
            yield return new WaitForSeconds(moveDuration); 
        }

        playerAnimator.SetBool("running", false);
        currentMovement = Vector3.zero;
        yield return new WaitForSeconds(1.64f + randomStopDelay);
       
    }
}