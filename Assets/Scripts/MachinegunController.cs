using UnityEngine;
using UnityEngine.InputSystem;

public class MachinegunController : MonoBehaviour
{
    [SerializeField] Transform Rotatable;

    void Start()
    {

	}

    void Update()
    {
        var mousePos = Mouse.current.position.ReadValue();
        var mouseRay = Camera.main.ScreenPointToRay(mousePos);
        Debug.DrawRay(mouseRay.origin, mouseRay.direction);
        if (Physics.Raycast(mouseRay, out var hitInfo, float.MaxValue, (1<<6)))
        {
            var rotation = Quaternion.LookRotation(hitInfo.point - Rotatable.position, Vector3.up);
            Rotatable.rotation = rotation;
        }
    }
}
