public interface IFreezable
{
    bool IsFrozen { get; }

    void Freeze(float duration);
}

