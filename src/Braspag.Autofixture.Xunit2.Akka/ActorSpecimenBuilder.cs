using Akka.Actor;
using Akka.TestKit.Xunit2;
using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace Autofixture.Xunit2.Akka
{
    public class ActorSpecimenBuilder : ISpecimenBuilder
    {
        protected Type TypeToLookFor()
        {
            return typeof(TestActorRefWrapper<>);
        }

        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as ParameterInfo;

            if (pi == null) return new NoSpecimen();

            if (!IsGenericTypeOf(TypeToLookFor(), pi.ParameterType))
            {
                return new NoSpecimen();
            }

            var actorType = pi.ParameterType.GenericTypeArguments[0];

            var actorConstructor = actorType.GetConstructors()[0];
            var actorConstructorArguments = actorConstructor.GetParameters();

            var actorConstructorArgumentsSubstitutes = new object[actorConstructorArguments.Length];

            for (int i = 0; i < actorConstructorArguments.Length; i++)
            {
                actorConstructorArgumentsSubstitutes[i] = context.Resolve(actorConstructorArguments[i].ParameterType);
            }

            var testKit = new TestKit(@"akka.loggers = [""Akka.TestKit.TestEventListener, Akka.TestKit""]");
            return BuildActor(testKit, pi, actorType, actorConstructorArgumentsSubstitutes);
        }

        protected object BuildActor(TestKit testKit, ParameterInfo pi, Type actorType, object[] actorConstructorArgumentsSubstitutes)
        {
            var ctor = pi.ParameterType.GetConstructor(new[] { typeof(TestKit), typeof(Props), typeof(ActorSystem) });
            return ctor.Invoke(new object[] { testKit, Props.Create(actorType, actorConstructorArgumentsSubstitutes), testKit.Sys });
        }

        protected bool IsGenericTypeOf(Type genericType, Type someType)
        {
            if (someType.IsGenericType && genericType == someType.GetGenericTypeDefinition()) return true;

            return someType.BaseType != null && IsGenericTypeOf(genericType, someType.BaseType);
        }
    }
}