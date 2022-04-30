using UnityEngine;
using System.Collections.Generic;
public class SeeObject : MonoBehaviour
{
    [SerializeField] private List<Transform> seeTransformList;

    public void AddSeeTransform(Transform obj){
        if(seeTransformList.Exists(x => x == obj)){
            return;
        }
        seeTransformList.Add(obj);
    }

    public void RemoveSeeTransform(Transform obj){
        seeTransformList.Remove(obj);
    }

    public Vector3? GetClosestSeeObjTransform(){
        if(seeTransformList.Count == 0) return null;
        else{
            Vector3 targetVector = seeTransformList[0].transform.position;
            float maxDistance = Vector3.Distance(targetVector, this.transform.position);
            foreach(Transform obj in seeTransformList){
                Vector3 vec = obj.transform.position;
                float distance = Vector3.Distance(vec, this.transform.position);
                if (distance < maxDistance){
                    targetVector = vec;
                    maxDistance = distance;
                }

            }
            return targetVector;
        }
    }
}
