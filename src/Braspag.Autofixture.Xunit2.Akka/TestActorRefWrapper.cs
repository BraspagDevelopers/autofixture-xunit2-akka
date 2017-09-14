using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;

namespace Autofixture.Xunit2.Akka
{
    public class TestActorRefWrapper<T> : TestActorRef<T> where T : ActorBase
    {
        public TestKit TestKit { get; }

        public TestActorRefWrapper(TestKit testKit, Props props, ActorSystem system) : base(system, props)
        {
            TestKit = testKit;
        }
    }
}
