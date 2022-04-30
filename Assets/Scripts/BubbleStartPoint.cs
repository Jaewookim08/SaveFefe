using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleStartPoint : MonoBehaviour
{
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float bubbleSpeed;
    [SerializeField] private float bubbleLiveTime;
    [SerializeField] private float scalingRatio;
    [SerializeField] private int bubblesPerSecond;
    private float bubbleMakeSec;
    private float timeElapsed;
    private int bubbleNum;
    private void Start() {
        direction.Normalize();
        bubbleMakeSec = 1f / bubblesPerSecond;
        timeElapsed = 0;
        bubbleNum = 0;
    }
    private void FixedUpdate() {
        timeElapsed += Time.fixedDeltaTime;

        if (timeElapsed / bubbleMakeSec >= bubbleNum){
            var newBubble = Instantiate(bubblePrefab, this.transform.localPosition + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), transform.localRotation, this.gameObject.transform);
            BubbleObject newBubbleComp = newBubble.GetComponent<BubbleObject>();
            newBubbleComp.direction = direction;
            newBubbleComp.speed = bubbleSpeed;
            newBubbleComp.surviveTime = bubbleLiveTime;
            newBubbleComp.scalingRatio = scalingRatio;

            bubbleNum += 1;
        }
    }
}
