namespace VirtualEyeClinic.Interaction
{
    public interface IInteractable
    {
        string GetInteractionPrompt();
        void Interact();
    }
}
