using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private int ropeLength = 5;
    [SerializeField] private GameObject ropeSegmentPrefab;
    [SerializeField] private Transform anchorTop;

    private Rigidbody previousBody;

    private void Start()
    {
        previousBody = anchorTop.GetComponent<Rigidbody>();

        for (int i = 0; i < ropeLength; i++)
        {
            GameObject segment = Instantiate(
                ropeSegmentPrefab,
                anchorTop.position + Vector3.down * (0.5f * (i + 1)),
                Quaternion.identity
            );

            HingeJoint joint = segment.AddComponent<HingeJoint>();
            joint.anchor = new Vector3(0, 0.5f, 0);
            joint.connectedBody = previousBody;

            previousBody = segment.GetComponent<Rigidbody>();
        }
    }
}
//fw