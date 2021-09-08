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

        builder.Register<IClickable, ClickableEnemy> (Lifetime.Singleton);
        builder.RegisterEntryPoint<ClickEntryPoint> (Lifetime.Singleton);

    }
}