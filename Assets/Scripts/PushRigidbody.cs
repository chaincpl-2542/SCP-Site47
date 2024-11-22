using UnityEngine;

public class PushRigidbody : MonoBehaviour
{
    public float pushStrength = 5.0f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        // If there is no rigidbody or it is kinematic, return
        if (rb == null || rb.isKinematic) return;

        // Calculate push direction from player
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // Apply push force to the rigidbody
        rb.AddForce(pushDir * pushStrength, ForceMode.Impulse);
    }
}
