namespace SimpleCLI.Runtime.Core.Modules.Interfaces
{
    public interface ISimpleCliModule
    {
        string Name { get; }
        string Description { get; }
        void Initialize(SimpleCliParser parser);
        void Shutdown();
    }
}