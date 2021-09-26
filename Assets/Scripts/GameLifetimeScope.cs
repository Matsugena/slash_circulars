using MessagePipe;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope {
    protected override void Configure (IContainerBuilder builder) {
        // RegisterMessagePipe returns options.
        // var options = builder.RegisterMessagePipe ( /* configure option */ );

        // RegisterMessageBroker: Register for IPublisher<T>/ISubscriber<T>, includes async and buffered.
        // builder.RegisterMessageBroker<int> (options);

        // also exists RegisterMessageBroker<TKey, TMessage>, RegisterRequestHandler, RegisterAsyncRequestHandler

        // RegisterMessageHandlerFilter: Register for filter, also exists RegisterAsyncMessageHandlerFilter, Register(Async)RequestHandlerFilter
        // builder.RegisterMessageHandlerFilter<MyFilter<int>> ();

        // builder.RegisterEntryPoint<MessagePipeDemo> (Lifetime.Singleton);
        var options = builder.RegisterMessagePipe ();
        builder.RegisterMessageBroker<DamageData> (options);
        builder.RegisterMessageBroker<Jump> (options);
        builder.RegisterMessageBroker<Move> (options);
        builder.RegisterMessageBroker<MouseEvent> (options);
        builder.RegisterMessageBroker<MouseUpEvent> (options);
        builder.RegisterMessageBroker<DragEvent> (options);

        var scope = Lifetime.Singleton;
        builder.Register<IPlayerInput, PlayerInput> (scope);
        builder.Register<IClickable, ClickableEnemy> (scope);
        builder.RegisterEntryPoint<ClickEntryPoint> (scope);
        builder.RegisterEntryPoint<InputEntryPoint> (scope);

    }
}