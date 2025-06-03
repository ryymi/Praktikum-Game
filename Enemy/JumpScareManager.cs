using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JumpScareManager : MonoBehaviour
{
    public GameObject jumpScareMonster; // Model monster
    public Transform jumpScarePosition; // Posisi di depan kamera
    public AudioClip jumpScareSound;    // Efek suara jumpscare
    public Image fadeScreen;            // Layar untuk efek fade (UI Image)

    public float scareDuration = 1f;    // Durasi jumpscare
    public float fadeDuration = 1f;     // Durasi fade
    public float fadeSpeed = 1.5f;      // Kecepatan fade

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        jumpScareMonster.SetActive(false);

        // Pastikan layar mulai transparan dan tidak memblokir interaksi
        fadeScreen.color = new Color(0, 0, 0, 0);
        fadeScreen.raycastTarget = false;
    }

    public void TriggerJumpScare()
    {
        StartCoroutine(PlayJumpScare());
    }

    private IEnumerator PlayJumpScare()
    {
        // Pindahkan monster ke posisi yang benar
        jumpScareMonster.transform.position = jumpScarePosition.position;
        jumpScareMonster.transform.rotation = jumpScarePosition.rotation;
        jumpScareMonster.SetActive(true);

        // Fade to black
        fadeScreen.raycastTarget = true; // Mengaktifkan raycast target untuk memblokir interaksi
        yield return StartCoroutine(FadeToBlack());

        // Mainkan efek suara jumpscare
        if (jumpScareSound != null)
        {
            audioSource.PlayOneShot(jumpScareSound);
        }
        // Tampilkan monster selama scareDuration
        yield return new WaitForSeconds(scareDuration);

        // Hilangkan monster
        jumpScareMonster.SetActive(false);

        // Pastikan raycastTarget pada layar hitam dimatikan sebelum pindah scene
        fadeScreen.raycastTarget = false;

        // Pindah ke menu "You're Dead"
UnityEngine.SceneManagement.SceneManager.LoadScene("YouAreDead");
    }
    void Update()
    {
        Debug.Log("Monster active: " + jumpScareMonster.activeSelf);
    }
    private IEnumerator FadeToBlack()
    {
        for (float t = 0; t < fadeDuration; t += Time.deltaTime * fadeSpeed)
        {
            float fadeAmount = t / fadeDuration;
            fadeScreen.color = new Color(0, 0, 0, fadeAmount);
            yield return null;
        }
        fadeScreen.color = Color.black;
    }
}
