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

    void Start()
    {
        animator = GetComponent<Animator>();
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

    /// <summary>
    /// Respawn the bat at a new location and return to idle
    /// </summary>
    private void Respawn()
    {
        // Move to a random position on screen
        Vector2 newPos = new Vector2(Random.Range(-7f, 7f), Random.Range(-3f, 3f));
        transform.position = newPos;

        // Show it again
        gameObject.SetActive(true);

        // Reset animation to Idle
        animator.Play("Idle");
    }

}
