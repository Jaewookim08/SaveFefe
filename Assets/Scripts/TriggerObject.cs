using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    [SerializeField] private bool directionFixed;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float power;
    [SerializeField] private float stunTime;
    private MainCharacter triggerObject;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player"){
            Debug.Log("Player Collided");
            this.triggerObject = other.GetComponent<MainCharacter>();
            if (directionFixed)
                triggerObject.CollidedWithTrigger(direction, power, stunTime);
            else{
                float x = triggerObject.transform.localPosition.x - this.transform.localPosition.x;
                float y = triggerObject.transform.localPosition.y - this.transform.localPosition.y;
                direction.x = x;
                direction.y = y;
                triggerObject.CollidedWithTrigger(direction, power, stunTime);
            }
            return;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player"){
            Debug.Log("Player Collided");
            if (directionFixed)
                triggerObject.CollidedWithTrigger(direction, power / 2, stunTime);
            else{
                float x = triggerObject.transform.localPosition.x - this.transform.localPosition.x;
                float y = triggerObject.transform.localPosition.y - this.transform.localPosition.y;
                direction.x = x;
                direction.y = y;
                triggerObject.CollidedWithTrigger(direction, power / 2, stunTime);
            }
            return;
        }
    }
}
