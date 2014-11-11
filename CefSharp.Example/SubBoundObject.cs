namespace CefSharp.Example
{
	public class SubBoundObject
	{
		public string SimpleProperty { get; set; }

		public SubBoundObject()
		{
			SimpleProperty = "This is a very simple property.";
		}

		public string GetMyType()
		{
			return "My Type is " + GetType();
		}

		public string EchoSimpleProperty()
		{
			return SimpleProperty;
		}
	}
}
