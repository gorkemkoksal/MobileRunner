using System.Collections;
using UnityEngine;

public class SniperFire : MonoBehaviour
{
    LineRenderer laser;
    EnemyMovement enemyMovement;
    EAnimController eAnimController;

    float laserAimCounter = 1f;
    public bool isShoot;
    public bool isFiring;
    bool safeCoroutineFlag;
    private void Start()
    {
        if (laser == null)
            laser = GetComponentInChildren<LineRenderer>();
        if (enemyMovement == null)
            enemyMovement = GetComponent<EnemyMovement>();
        if (eAnimController == null)
            eAnimController = GetComponentInParent<EAnimController>();

        StartCoroutine(LaserAiming());
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("mermi");
        if (!isShoot || other.gameObject.tag!="Player") return;
        Debug.Log("Dead");
    }
    IEnumerator LaserAiming()
    {
        if (safeCoroutineFlag)
        {
            yield break;
        }
        safeCoroutineFlag = true;
        while (true)
        {
            if (!enemyMovement.startFiring) yield return null;
            else if (enemyMovement.startFiring && laserAimCounter < 2)
            {
                yield return new WaitForSeconds(0.4f);
                laserAimCounter += 0.4f;
                laser.widthMultiplier = laserAimCounter;
            }
            else if (enemyMovement.startFiring && laserAimCounter >= 1.99f && !isShoot)
            {
                laser.material.color = Color.white;
                isShoot = true;
                //yield return new WaitForSeconds(0.3f);
                //laser.material.color = Color.red;
            }
            else if (isShoot)
            {
                yield return new WaitForSeconds(0.2f);

                isFiring = true;
                eAnimController.IsFire(isFiring);
                isFiring = false;
                laser.widthMultiplier = 1f;
                laser.material.color = Color.red;
                isShoot = false;
                enemyMovement.startFiring = false;
            }
            // else StopCoroutine(LaserAiming());
            else
            {
                laser.enabled = false;
                break;
            }
        }
    }
}
