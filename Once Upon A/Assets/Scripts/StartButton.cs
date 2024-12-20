public class StartButton : ButtonController
{
    public GameManager Manager;

    public override void OnClick()
    {
        GameManager.Manager.LoadLevel();
    }

    public void Update()
    {
        if (Manager.Input.Actions.Jump.WasPressedThisFrame())
        {
            OnClick();
        }
    }
}
