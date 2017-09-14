# Introduction 

The purpose of this project is to allow that [Autofixture](https://github.com/AutoFixture/AutoFixture) to create your [Akka.Net](https://github.com/akkadotnet/akka.net) actors in your tests with [Xunit2](https://github.com/BraspagDevelopers/autofixture-xunit2-akka).

# Getting Started
You first need to install the **Braspag.Autofixture.Xunit2.Akka** package from Nuget.org:
```
PM> Braspag.Autofixture.Xunit2.Akka
```

After that, you need to add the **ActorSpecimenBuilder** to your AutoDataAttribute:

``` csharp
public class AutoApiDataAttribute : AutoDataAttribute
    {
        public AutoApiDataAttribute() : this(new Fixture())
        {
        }

        public AutoActorDataAttribute(IFixture fixture)
        {
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
```

And finally, in your tests, you should add your actor as parameter using the generic type **TestActorRefWrapper<T>** where T must inhereit from ActorBase:

``` csharp
[Theory, AutoNSubstituteData]
public void ProcessBatches_WhenBatchesExistsInDb_ShouldSendMessageToProcessTransactionCoordinator(TestActorRefWrapper<MyActorClass> sut)
{
	// Your test
}
```

# Build and Test
To build this project you will need Visual Studio 2017. If you already have it, clone this repo and have fun!