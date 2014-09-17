using System.Runtime.Serialization;

namespace CefSharp
{
	[DataContract]
	[KnownType(typeof(object[]))]
	public class JavascriptResponse
	{
		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public bool Success { get; set; }

		[DataMember]
		public object Result { get; set; }
	}
}
