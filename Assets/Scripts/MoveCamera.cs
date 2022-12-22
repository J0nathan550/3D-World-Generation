using TMPro;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float cameraSpeed = 4f;
    public float mouseSensitivity = 4f;
    [SerializeField] private CharacterController character;
    [SerializeField] private UISystem ui;

    private float x = 0f;
    private float y = 0f;

    public static bool fullscreen = false;

    private void Update()
    {
        Modifications();
        if (fullscreen)
        {
            MovePosCam();
            MoveAround();
        }
    }

    private void MoveAround()
    {
        x += mouseSensitivity * Input.GetAxis("Mouse X");
        y -= mouseSensitivity * Input.GetAxis("Mouse Y");
        y = Mathf.Clamp(y, -90, 90); 

        transform.eulerAngles = new Vector3(y, x, 0.0f);
    }

    private void MovePosCam()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Jump");
        float yCrouch = Input.GetAxis("Crouch");
        float z = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftAlt))
        {
           character.Move(transform.forward * z * cameraSpeed * Time.deltaTime + transform.right * x * cameraSpeed * Time.deltaTime + transform.up * -yCrouch * cameraSpeed * Time.deltaTime);
        }
        else
        {
           character.Move(transform.forward * z * cameraSpeed * Time.deltaTime + transform.right * x * cameraSpeed * Time.deltaTime + transform.up * y * cameraSpeed * Time.deltaTime);
        }
    }

    private void Modifications()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            fullscreen = !fullscreen;
        }
        if (fullscreen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            for (int i = 0; i < ui.allUIElements.Length; i++)
            {
                ui.allUIElements[i].SetActive(false);
            }
            for (int i = 0; i < ui.panels.Length; i++)
            {
                ui.panels[i].SetActive(false);
            }
            for (int i = 0; i < ui.colorPanels.Length; i++)
            {
                ui.colorPanels[i].SetActive(false);
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            for (int i = 0; i < ui.allUIElements.Length; i++)
            {
                ui.allUIElements[i].SetActive(true);
            }
        }
    }

}