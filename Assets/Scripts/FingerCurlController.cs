
using UnityEngine;

public class FingerCurlController : MonoBehaviour
{
    [System.Serializable]
    public class Finger
    {
        public Transform baseBone;
        public Transform middleBone;
        public Transform tipBone;

        [HideInInspector] public Quaternion baseRest;
        [HideInInspector] public Quaternion middleRest;
        [HideInInspector] public Quaternion tipRest;
    }

    [Header("Finger Bones")]
    [SerializeField] private Finger[] fingers = new Finger[5];

    [Header("Grip")]
    [SerializeField] private float curlAngle = 55f;
    [SerializeField] private float thumbCurlAngle = 40f;
    [SerializeField] private float curlSpeed = 12f;

    private bool isGrabbing;
    private float curlAmount;

    private void Awake()
    {
        foreach (Finger finger in fingers)
        {
            if (finger.baseBone != null)
                finger.baseRest = finger.baseBone.localRotation;

            if (finger.middleBone != null)
                finger.middleRest = finger.middleBone.localRotation;

            if (finger.tipBone != null)
                finger.tipRest = finger.tipBone.localRotation;
        }
    }

    public void SetGrabbing(bool grabbing)
    {
        isGrabbing = grabbing;
        Debug.Log($"{name}: сгибание = {grabbing}");
    }

    private void LateUpdate()
    {
        float target = isGrabbing ? 1f : 0f;

        curlAmount = Mathf.MoveTowards(
            curlAmount,
            target,
            curlSpeed * Time.deltaTime
        );

        for (int i = 0; i < fingers.Length; i++)
        {
            Finger finger = fingers[i];
            if (finger == null)
                continue;

            float angle = i == 0 ? thumbCurlAngle : curlAngle;

            CurlBone(finger.baseBone, finger.baseRest, angle * 0.4f);
            CurlBone(finger.middleBone, finger.middleRest, angle * 0.6f);
            CurlBone(finger.tipBone, finger.tipRest, angle * 0.7f);
        }
    }

    private void CurlBone(
        Transform bone,
        Quaternion restRotation,
        float angle)
    {
        if (bone == null)
            return;

        Quaternion curledRotation =
            restRotation * Quaternion.Euler(-angle, 0f, 0f);

        bone.localRotation = Quaternion.Slerp(
            restRotation,
            curledRotation,
            curlAmount
        );
    }
}
