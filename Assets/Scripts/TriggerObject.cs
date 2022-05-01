using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    [SerializeField] public bool isTrigger; // Todo:
    [SerializeField] private bool directionFixed;
    [SerializeField] public Vector2 direction;
    [SerializeField] private float power;
    [SerializeField] private float stunTime;
    private MainCharacter triggerObject;
    private void OnTriggerEnter2D(Collider2D other) {
        if (!isTrigger) return;
        if (other.tag != "Player") return;

        this.triggerObject = other.GetComponent<MainCharacter>();
        if (directionFixed)
            triggerObject.CollidedWithTrigger(direction, power, stunTime, gameObject);
        else
        {
            float x = triggerObject.transform.localPosition.x - this.transform.localPosition.x;
            float y = triggerObject.transform.localPosition.y - this.transform.localPosition.y;
            direction.x = x;
            direction.y = y;
            triggerObject.CollidedWithTrigger(direction, power, stunTime, gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (!isTrigger) return;
        if (other.tag != "Player") return;

        if (directionFixed)
            triggerObject.CollidedWithTrigger(direction, power / 2, stunTime, gameObject);
        else
        {
            float x = triggerObject.transform.localPosition.x - this.transform.localPosition.x;
            float y = triggerObject.transform.localPosition.y - this.transform.localPosition.y;
            direction.x = x;
            direction.y = y;
            triggerObject.CollidedWithTrigger(direction, power / 2, stunTime, gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (isTrigger) return;

        this.triggerObject = other.collider.GetComponent<MainCharacter>();

        var point = other.GetContact(0).point;
        triggerObject.CollidedWithTriggerAtPoint(point, point - (Vector2) transform.localPosition, power, stunTime, gameObject);
    }
}
