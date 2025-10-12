using UnityEngine;

public class LightMovement : MonoBehaviour
{
    [SerializeField] private Transform _lightTransform;
    [SerializeField] private Transform _cameraTransform;

    private Vector3 _lightPosition;

    // Update is called once per frame
    void FixedUpdate()
    {
        _lightPosition = new Vector3(_cameraTransform.position.x, _lightTransform.position.y);
        _lightTransform.position = _lightPosition;
    }
}
