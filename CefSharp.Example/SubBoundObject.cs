namespace CefSharp.Example
{
	public class SubBoundObject
	{
        // setting Parent will cause stack overflow if not for late binding
        public BoundObject Parent { get; set; }

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
