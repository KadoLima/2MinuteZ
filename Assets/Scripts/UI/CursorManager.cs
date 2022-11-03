using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] Texture2D crosshairTexture;
    [SerializeField] Texture2D normalTexture;

    public static CursorManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating(nameof(ChangeCursor), 0f, 0.3f);

        Cursor.lockState = CursorLockMode.Confined;

        NormalCursor();
    }

    private void OnEnable()
    {
        EventsManager.OnMissionIsPlayable += ChangeCursor;
    }

    private void OnDisable()
    {
        EventsManager.OnMissionIsPlayable -= ChangeCursor;
    }

    public void ChangeCursor()
    {
        if (GameManager.instance.IsPlayable)
        {
            CrossHairCursor();
        }
        else
        {
            NormalCursor();
        }
    }

    public void CrossHairCursor()
    {
        Cursor.SetCursor(crosshairTexture, new Vector2(crosshairTexture.width / 2, crosshairTexture.height / 2), CursorMode.Auto);
    }

    public void NormalCursor()
    {
        Cursor.SetCursor(normalTexture, Vector2.zero, CursorMode.ForceSoftware);
    }
}
