using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class SeeMe : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        SeeObject sobj = other.GetComponent<SeeObject>();

        sobj.AddSeeTransform(this.transform);
    }

    private void OnTriggerExit2D(Collider2D other) {
        SeeObject sobj = other.GetComponent<SeeObject>();

        sobj.RemoveSeeTransform(this.transform);
    }
}
