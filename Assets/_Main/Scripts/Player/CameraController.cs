using UnityEngine;

/// <summary>
/// Manages cursor states and camera controls
/// </summary>
public class CameraController : MonoBehaviour
{
    #region Singleton
    private static CameraController _instance;
    public static CameraController Instance { get { return _instance; } }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        } else {
            _instance = this;
        }
    }
    #endregion
    
    /// <summary>
    /// Unlocks cursor and makes it visible
    /// </summary>
    public void FreeMouseCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    /// <summary>
    /// Locks cursor and makes it invisible
    /// </summary>
    public void LockMouseCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
