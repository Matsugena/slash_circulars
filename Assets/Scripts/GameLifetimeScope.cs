using MessagePipe;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope {
    protected override void Configure(IContainerBuilder builder) {

        var options = builder.RegisterMessagePipe();
        builder.RegisterMessageBroker<MouseEvent>(options);
        builder.RegisterMessageBroker<MouseUpEvent>(options);
        builder.RegisterMessageBroker<DragEvent>(options);
        builder.RegisterMessageBroker<FlickEvent>(options);
        builder.RegisterMessageBroker<GameEvent>(options);

        var scope = Lifetime.Singleton;
        // TODO 開発時と以外で分ける
        builder.Register<IGameController, DevelopGameController>(scope);
        builder.RegisterEntryPoint<InputEntryPoint>(scope);

    }
}