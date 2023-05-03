namespace InputReader
{
    public interface IEntityInputSource
    {
        float HorizontalDirection { get; }
        bool Jump { get; }
        bool Kick { get; }
        bool Bite { get; }

        void ResetOneTimeActions();
    }
}