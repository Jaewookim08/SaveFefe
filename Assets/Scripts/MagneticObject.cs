using UnityEngine;

public class MagneticObject : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public float surviveTime;
    private float timeElapsed = 0;
    private void FixedUpdate() 
    {
        if(timeElapsed >= surviveTime){
            Destroy(this.gameObject);
        }
        timeElapsed += Time.fixedDeltaTime;
        this.transform.position += new Vector3(direction.x * speed * Time.fixedDeltaTime, direction.y * speed * Time.fixedDeltaTime, 0);
    }
}
