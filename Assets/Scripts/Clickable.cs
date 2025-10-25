using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{

    // This variable is marked as `static`, which means it is
    // accessible by any script, anywhere! They can just write
    // Clickable.Clicks to read/write to this value.
    //
    // See an example in `ClickTracker.cs`, which is on a UI
    // object titled `Tracker Text (TMP)`.
    public static int Score = 0;
    private Animator animator;

    private Vector3 originalScale;

    // Movement variables
    // public float moveSpeed = 2f;          // how fast the bat moves
    public float minX = -7f, maxX = 7f;   // horizontal screen limits
    public float minY = -3f, maxY = 3f;   // vertical screen limits

    [SerializeField] private float moveSpeed = 1.5f;   // starting speed
    [SerializeField] private float speedIncreaseAmount = 0.3f; // how much to increase
    [SerializeField] private int clicksPerLevel = 25;  // how many clicks before speedup
    private int nextSpeedMilestone = 25;               // next score milestone

    private Vector2 targetPosition;       // where the bat is flying to

    private float flutterAmplitude = 0.3f;   // how far up/down the flutter goes
    private float flutterFrequency = 6f;     // how fast it oscillates
    private float directionChangeTimer = 0f; // counts down until next new target
    private float minChangeTime = 1f;
    private float maxChangeTime = 3f;
    private float flutterOffset;             // randomizes the wave for each bat

    void Start()
    {
        animator = GetComponent<Animator>();
        PickNewTarget();
        flutterOffset = Random.Range(0f, 100f); // random phase for variety
        originalScale = transform.localScale; // store initial size from the scene
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            MoveWithFlutter();
        }

        // Check if it's time to increase difficulty
        if (Score >= nextSpeedMilestone)
        {
            moveSpeed = Mathf.Min(moveSpeed + speedIncreaseAmount, 5f); // cap at 5
            nextSpeedMilestone += clicksPerLevel;
            Debug.Log($"Bat speed increased! New speed: {moveSpeed}");
        }
    }

    /// <summary>
    /// This function is called when the mouse button clicks
    /// on this object.
    /// </summary>
    private void OnMouseDown()
    {
        Score += 1;  // add one point
        animator.SetTrigger("Die");
    }

    /// <summary>
    /// Called at the end of the Death animation (via Animation Event)
    /// </summary>
    public void OnDeathAnimationEnd()
    {
        Debug.Log("Death animation finished!");

        // Hide the bat
        gameObject.SetActive(false);

        // Respawn after a short delay
        Invoke(nameof(Respawn), 1.5f);
    }

    private IEnumerator PopIn()
    {
        transform.localScale = Vector3.zero;
        float duration = 0.4f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            float bounce = Mathf.Sin(progress * Mathf.PI) * 1.1f;
            transform.localScale = originalScale * bounce;
            yield return null;
        }

        transform.localScale = originalScale;
    }

    /// <summary>
    /// Respawn the bat at a new location and return to idle
    /// </summary>
    private void Respawn()
    {
        // Move to a random position on screen
        transform.position = GetRandomPosition();

        // Show it again
        gameObject.SetActive(true);

        // Reset animation to Idle
        animator.Play("Bat_Idle");

        StartCoroutine(PopIn());
    }

    // --- Movement helper methods ---

    private void MoveWithFlutter()
    {
        // Smoothly move toward the target position
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // Apply a fluttering (sinusoidal) motion vertically
        float flutter = Mathf.Sin(Time.time * flutterFrequency + flutterOffset) * flutterAmplitude;
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + flutter * Time.deltaTime,
            transform.position.z
        );

        // Occasionally change direction even before reaching the target
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0f || Vector2.Distance(transform.position, targetPosition) < 0.2f)
        {
            PickNewTarget();
        }
    }

    private void PickNewTarget()
    {
        targetPosition = GetRandomPosition();
        directionChangeTimer = Random.Range(minChangeTime, maxChangeTime);
    }

    private Vector2 GetRandomPosition()
    {
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

}
