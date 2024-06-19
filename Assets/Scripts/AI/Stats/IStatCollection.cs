public interface IStatCollection
{
    T GetStat<T>() where T : ICharacterStat;
    void AddStat<T>(T stat) where T : ICharacterStat;
}