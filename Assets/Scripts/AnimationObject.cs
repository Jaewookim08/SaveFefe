using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationObject : MonoBehaviour
{
    [SerializeField] Sprite InitSprite;
    [SerializeField] Sprite ChangeSprite;
    [SerializeField] float animateTime;
    [SerializeField] bool whenCollide;
    private bool isAnimating;
    private float time;
    private void Start() {
        isAnimating = false;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (!whenCollide) return;
        isAnimating = true;
        time = 0;
        this.GetComponent<SpriteRenderer>().sprite = ChangeSprite;
    }

    private void FixedUpdate() {
        if (!isAnimating) return;
        if (time >= animateTime){
            isAnimating = false;
            time = 0;
            this.GetComponent<SpriteRenderer>().sprite = InitSprite;
            return;
        }
        time += Time.fixedDeltaTime;
    }
}
