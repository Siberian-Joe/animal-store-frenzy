public class AIContext
{
    public IStatCollection StatCollection { get; }

    public AIContext(IStatCollection statCollection)
    {
        StatCollection = statCollection;
    }
}