using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RectTransformTester : MonoBehaviour
{
    public RectTransform RectTransform
    {
        get { return GetComponent<RectTransform>(); }
    }
}