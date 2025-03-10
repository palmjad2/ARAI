using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class ArmGraspAgent : Agent
{
    [Header("References")]

    [Tooltip("Finger segments that will each have their own collider and can rotate.")]
    public Transform pointerBase;
    public Transform pointerEnd;
    public Transform middleBase;
    public Transform middleEnd;
    public Transform ringBase;
    public Transform ringEnd;
    public Transform pinkyBase;
    public Transform pinkyEnd;
    public Transform thumb;

    [Tooltip("Finger segments that will each have their own collider and can rotate.")]
    public Transform finger1;
    public Transform finger2;
    // Add more fingers here if needed

    [Tooltip("Palm transform (with collider).")]
    public Transform palm;

    [Header("Finger Rotation Settings")]
    [Tooltip("How many degrees per step the agent can rotate each finger joint.")]
    public float rotationSpeed = 30f;

    // Internal contact booleans
    private bool InConta = false;
    private bool finger1InContact = false;
    private bool finger2InContact = false;

    private bool palmInContact = false;

    // Track how long each collider remains in contact

    private float pointerBaseContactTime = 0f;
    private float pointerEndContactTime = 0f;

    private float finger1ContactTime = 0f;
    private float finger2ContactTime = 0f;
    private float palmContactTime = 0f;

    // How long must a finger or palm be in contact to gain reward
    private const float contactThreshold = 1.0f;

    // Called once at the beginning
    public override void Initialize()
    {
        // Optionally do any setup here
    }

    // Reset logic when each new episode begins
    public override void OnEpisodeBegin()
    {
        // Reset all finger/palm rotations and contact timers
        finger1.localRotation = Quaternion.identity;
        finger2.localRotation = Quaternion.identity;
        // Reset more fingers here if needed
        palm.localRotation = Quaternion.identity;

        pointerBaseContactTime = 0f;
        pointerEndContactTime = 0f;
        finger1ContactTime = 0f;
        finger2ContactTime = 0f;
        finger1ContactTime = 0f;
        finger2ContactTime = 0f;
        finger1ContactTime = 0f;
        finger2ContactTime = 0f;
        finger1ContactTime = 0f;
        finger2ContactTime = 0f;
        palmContactTime = 0f;

        finger1InContact = false;
        finger2InContact = false;
        finger1InContact = false;
        finger2InContact = false;
        finger1InContact = false;
        finger2InContact = false;
        finger1InContact = false;
        finger2InContact = false;
        finger1InContact = false;
        finger2InContact = false;
        palmInContact = false;


    }

    // Collect observations
    public override void CollectObservations(VectorSensor sensor)
    {
        // 1) Which finger/palm is in contact (as booleans -> 0 or 1).
        sensor.AddObservation(finger1InContact ? 1f : 0f);
        sensor.AddObservation(finger2InContact ? 1f : 0f);
        sensor.AddObservation(finger1InContact ? 1f : 0f);
        sensor.AddObservation(finger2InContact ? 1f : 0f);
        sensor.AddObservation(finger1InContact ? 1f : 0f);
        sensor.AddObservation(finger2InContact ? 1f : 0f);
        sensor.AddObservation(finger1InContact ? 1f : 0f);
        sensor.AddObservation(finger2InContact ? 1f : 0f);
        sensor.AddObservation(finger1InContact ? 1f : 0f);
        sensor.AddObservation(finger2InContact ? 1f : 0f);
        sensor.AddObservation(palmInContact ? 1f : 0f);


        // That’s 6 total observations:
        // [finger1Contact, finger2Contact, palmContact, topPos.x, topPos.y, topPos.z]
    }

    // Define how many actions we expect: for example,
    // each finger might have 1 DOF (one rotation axis).
    // If you have 2 fingers + 1 palm each with 1 DOF, that’s 3 actions total.
    // Adjust as needed (e.g., multiple joints per finger).
    public override void OnActionReceived(ActionBuffers actions)
    {
        // Continuous actions for rotating finger1, finger2, etc.
        float rotateFinger1 = actions.ContinuousActions[0]; // range ~[-1,1]
        float rotateFinger2 = actions.ContinuousActions[1]; // range ~[-1,1]
        float rotatePalm = actions.ContinuousActions[2]; // range ~[-1,1]

        // Apply local rotations around (for example) the X-axis
        finger1.localRotation *= Quaternion.Euler(rotateFinger1 * rotationSpeed * Time.deltaTime, 0f, 0f);
        finger2.localRotation *= Quaternion.Euler(rotateFinger2 * rotationSpeed * Time.deltaTime, 0f, 0f);
        palm.localRotation *= Quaternion.Euler(rotatePalm * rotationSpeed * Time.deltaTime, 0f, 0f);

        // --- Reward Logic ---
        // 1) Check if each finger/palm has been in contact for >= 1 second
        //    If so, add reward (normalized as you see fit) and reset the timer
        if (finger1ContactTime >= contactThreshold)
        {
            AddReward(0.1f);  // small reward
            finger1ContactTime = 0f; // reset so we don't repeatedly reward
        }
        if (finger2ContactTime >= contactThreshold)
        {
            AddReward(0.1f);
            finger2ContactTime = 0f;
        }
        if (palmContactTime >= contactThreshold)
        {
            AddReward(0.2f);  // bigger reward for palm contact
            palmContactTime = 0f;
        }

    }


    // Track collisions to set booleans and measure contact time
    private void OnCollisionStay(Collision collision)
    {
        // If the collision is with the cylinder
        if (collision.gameObject.CompareTag("Cylinder"))
        {
            // Check which collider of ours is involved
            foreach (ContactPoint contact in collision.contacts)
            {
                // If the contact point is from finger1
                if (contact.thisCollider != null && contact.thisCollider.gameObject == finger1.gameObject)
                {
                    finger1InContact = true;
                    finger1ContactTime += Time.fixedDeltaTime;
                }
                if (contact.thisCollider != null && contact.thisCollider.gameObject == finger2.gameObject)
                {
                    finger2InContact = true;
                    finger2ContactTime += Time.fixedDeltaTime;
                }
                if (contact.thisCollider != null && contact.thisCollider.gameObject == palm.gameObject)
                {
                    palmInContact = true;
                    palmContactTime += Time.fixedDeltaTime;
                }
            }
        }
    }

    // Reset booleans if not in contact this frame
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cylinder"))
        {
            // If finger/palm leaves the cylinder, reset contact
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.thisCollider != null && contact.thisCollider.gameObject == finger1.gameObject)
                {
                    finger1InContact = false;
                    finger1ContactTime = 0f;
                }
                if (contact.thisCollider != null && contact.thisCollider.gameObject == finger2.gameObject)
                {
                    finger2InContact = false;
                    finger2ContactTime = 0f;
                }
                if (contact.thisCollider != null && contact.thisCollider.gameObject == palm.gameObject)
                {
                    palmInContact = false;
                    palmContactTime = 0f;
                }
            }
        }
    }
}
