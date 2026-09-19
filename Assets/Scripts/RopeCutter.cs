using UnityEngine;

public class RopeCutter : MonoBehaviour
{
    [SerializeField] private HingeJoint ropeJoint;

    void Start()
    {
        Destroy(ropeJoint);
    }
}