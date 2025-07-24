using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public GameObject TDPlayer;
    public Vector3 normalOffset;
    public Vector3 topDownOffset;

    [SerializeField] float transitionDuration = 0.5f;

    private bool isTopDownView = false;
    private bool isTransitioning = false;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Start()
    {
        player.SetActive(true);
        TDPlayer.SetActive(false);

        transform.position = player.transform.position + normalOffset;
        transform.rotation = Quaternion.Euler(15f, 0f, 0f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && !isTransitioning)
        {
            StartCoroutine(ToggleCameraView());
        }
    }

    void LateUpdate()
    {
        // Only update camera position if not transitioning
        if (!isTransitioning)
        {
            if (!isTopDownView && player.activeSelf)
            {
                transform.position = player.transform.position + normalOffset;
                transform.rotation = Quaternion.Euler(15f, transform.eulerAngles.y, transform.eulerAngles.z);
            }
            else if (isTopDownView && TDPlayer.activeSelf)
            {
                transform.position = TDPlayer.transform.position + topDownOffset;
                transform.rotation = Quaternion.Euler(74f, transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }
    }

    IEnumerator ToggleCameraView()
    {
        isTransitioning = true;

        if (player.activeSelf) player.GetComponent<PlayerMovement>().enabled = false;
        if (TDPlayer.activeSelf) TDPlayer.GetComponent<TDPlayerMovement>().enabled = false;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        if (!isTopDownView)
        {
            TDPlayer.transform.position = player.transform.position;

            targetPosition = TDPlayer.transform.position + topDownOffset;
            targetRotation = Quaternion.Euler(74f, 0f, 0f);
        }
        else
        {
            player.transform.position = TDPlayer.transform.position;

            targetPosition = player.transform.position + normalOffset;
            targetRotation = Quaternion.Euler(15f, transform.eulerAngles.y, transform.eulerAngles.z); // Maintain current Y, Z
        }
        
        if (!isTopDownView)
        {
            player.SetActive(false);
            TDPlayer.SetActive(true);
        }
        else
        {
            TDPlayer.SetActive(false);
            player.SetActive(true);
        }

        float timer = 0f;
        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float t = timer / transitionDuration; // Normalized progress (0 to 1)

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }    

        // Ensure camera is exactly at the final target position/rotation
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        
        if (!isTopDownView)
        {
            player.SetActive(false);
            TDPlayer.SetActive(true);
        }
        else
        {
            TDPlayer.SetActive(false);
            player.SetActive(true);
        }
        
        isTopDownView = !isTopDownView; // Toggle the view state

        if (player.activeSelf) player.GetComponent<PlayerMovement>().enabled = true;
        if (TDPlayer.activeSelf) TDPlayer.GetComponent<TDPlayerMovement>().enabled = true;

        isTransitioning = false;
    }
}
