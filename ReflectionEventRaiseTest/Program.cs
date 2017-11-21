using System;
using System.Reflection;

namespace ReflectionEventRaiseTest
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var fooInstance = new FooClass();

			fooInstance.BarEvent += FooInstanceOnBarEvent1;
			fooInstance.BarEvent += FooInstanceOnBarEvent2;

			RaiseClassEvent(fooInstance, nameof(FooClass.BarEvent), 1, 2);

			Console.ReadKey();
		}

		private static void RaiseClassEvent(object instance, string eventName, params object[] arguments)
		{
			var eventDelegate = (MulticastDelegate)instance.GetType().GetField(eventName, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(instance);
			if (eventDelegate == null) return;

			foreach (var handler in eventDelegate.GetInvocationList())
				handler.Method.Invoke(handler.Target, arguments);
		}

		private static void FooInstanceOnBarEvent1(int foo, int bar)
		{
			Console.WriteLine($"Bar event listener 1: {foo}, {bar}");
		}

		private static void FooInstanceOnBarEvent2(int foo, int bar)
		{
			Console.WriteLine($"Bar event listener 2: {foo}, {bar}");
		}
	}

	public class FooClass
	{
		public delegate void BarDelegate(int foo, int bar);

		public event BarDelegate BarEvent;
	}
}
