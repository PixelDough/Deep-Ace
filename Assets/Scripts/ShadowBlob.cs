using System;
using UnityEngine;

public class ShadowBlob : MonoBehaviour
{

    [SerializeField] private Transform fromTransform;
    [SerializeField] private Transform quad;
    [SerializeField] private float maxDistance = 4f;

    private Vector2 _maxScale;

    private void Start()
    {
        _maxScale = transform.localScale;
        transform.localScale = Vector3.one;
    }

    private void LateUpdate()
    {
        if (!fromTransform) fromTransform = transform;
        if (Physics.Raycast(fromTransform.position, Vector3.down, out var hit, maxDistance, LayerMask.GetMask("Solid"), QueryTriggerInteraction.Ignore))
        {
            quad.gameObject.SetActive(true);
            quad.position = hit.point + Vector3.up * 0.01f;
            
            float percentToMaxDistance = 1 - (hit.distance / maxDistance);
            quad.localScale = Vector3.Lerp(Vector3.zero, _maxScale, percentToMaxDistance);
            quad.forward = -hit.normal;
        }
        else
        {
            quad.gameObject.SetActive(false);
        }
    }

}
