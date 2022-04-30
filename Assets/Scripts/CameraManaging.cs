using UnityEngine;

public class CameraManaging : MonoBehaviour
{
    [SerializeField] private Transform _followingTransform;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var vec = this.transform.position;
        vec = Vector3.Lerp(this.transform.position, _followingTransform.position, 0.6f);
        vec.z = -1;
        this.transform.position = vec;
    }
}
