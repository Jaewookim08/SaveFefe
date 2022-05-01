using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetStartPoint : MonoBehaviour
{
    [SerializeField] private GameObject magneticPrefab;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float magneticSpeed;
    [SerializeField] private float magneticLiveTime;
    [SerializeField] private int magneticsPerSecond;
    [SerializeField] private Vector2 direction;
    private float magneticMakeSec;
    private float timeElapsed;
    private int magneticNum;
    private void Start() {
        magneticMakeSec = 1f / magneticsPerSecond;
        timeElapsed = 0;
        magneticNum = 0;
    }
    private void FixedUpdate() {
        timeElapsed += Time.fixedDeltaTime;

        if (timeElapsed / magneticMakeSec >= magneticNum){
            var newmagnetic = Instantiate(magneticPrefab, new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), transform.rotation, targetTransform);
            MagneticObject newmagneticComp = newmagnetic.GetComponent<MagneticObject>();
            direction = targetTransform.localPosition - newmagnetic.transform.localPosition;
            direction.Normalize();
            newmagneticComp.direction = direction;
            newmagneticComp.speed = magneticSpeed;
            newmagneticComp.surviveTime = magneticLiveTime;
            
            newmagnetic.GetComponent<TriggerObject>().direction = direction;

            magneticNum += 1;
        }
    }
}
