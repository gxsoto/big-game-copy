using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodPumpkin : MonoBehaviour
{

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log("PumpkinBehavior started on " + gameObject.name);
        // Randomly pick which idle animation to trigger
        if (Random.value < 0.5f)
        {
            animator.SetTrigger("ChooseIdle01");
        }
        else
        {
            animator.SetTrigger("ChooseIdle02");
        }

        // Start the pop-in animation
        StartCoroutine(PopInEffect());
    }

    IEnumerator PopInEffect()
    {
        Vector3 targetScale = transform.localScale; // final scale from spawner
        transform.localScale = Vector3.zero;        // start invisible

        float duration = 0.5f;   // how long the effect takes
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Ease-out spring formula
            float scaleFactor = Mathf.Sin(t * Mathf.PI * 0.5f);
            transform.localScale = targetScale * scaleFactor;

            yield return null;
        }

        // Add a little overshoot bounce
        float bounce = 1.1f;
        transform.localScale = targetScale * bounce;
        yield return new WaitForSeconds(0.05f);
        transform.localScale = targetScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
