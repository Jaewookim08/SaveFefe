using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] 
    public struct MoveRoute{
        public Vector2 direction;
        public float speed;
        public float time;
    }
public class MovingObject : MonoBehaviour
{
    [SerializeField] private int isreversedImage;
    [SerializeField] private bool isReversable;
    [SerializeField] private List<MoveRoute> moveRouteList;
    private int currentIndex = -1;
    private float leftTime = 0;
    private void FixedUpdate() 
    {
        if(leftTime <= 0){
            if(currentIndex + 1 >= moveRouteList.Count)
                currentIndex = 0;
            else
                currentIndex += 1;
            
            leftTime = moveRouteList[currentIndex].time;
        }
        leftTime -= Time.fixedDeltaTime;
        MoveRoute mv = moveRouteList[currentIndex];
        this.transform.localPosition += new Vector3(mv.direction.x * mv.speed * Time.fixedDeltaTime, mv.direction.y * mv.speed * Time.fixedDeltaTime, 0);
        if (!isReversable) return;
        if (mv.direction.x > 0 && this.transform.localScale.x > 0){
            this.transform.localScale = new Vector3(-this.transform.localScale.x * isreversedImage, this.transform.localScale.y, this.transform.localScale.z);
        }
        if (mv.direction.x < 0 && this.transform.localScale.x < 0){
            this.transform.localScale = new Vector3(-this.transform.localScale.x * isreversedImage, this.transform.localScale.y, this.transform.localScale.z);
        }
    }
}
