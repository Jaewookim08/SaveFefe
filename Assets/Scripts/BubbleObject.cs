using System;
using UnityEngine;

public class BubbleObject : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public float scalingRatio;
    public float surviveTime;
    [SerializeField] private Sprite explodedBubble;
    private float timeElapsed = 0;
    private void FixedUpdate() 
    {
        if(timeElapsed >= surviveTime){
            this.GetComponent<SpriteRenderer>().sprite = explodedBubble;
            if (timeElapsed - 0.2 >= surviveTime) Destroy(this.gameObject);
        }
        else{
            this.transform.localScale = new Vector3(this.transform.localScale.x * scalingRatio, this.transform.localScale.y * scalingRatio, this.transform.localScale.z);
        }
        timeElapsed += Time.fixedDeltaTime;
        this.transform.localPosition += new Vector3(direction.x * speed * Time.fixedDeltaTime, direction.y * speed * Time.fixedDeltaTime, 0);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        timeElapsed = surviveTime;
    }
}
