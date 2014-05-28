using System.Threading.Tasks;

namespace CefSharp
{
	public class TaskStringVisitor : IStringVisitor
	{
		private readonly TaskCompletionSource<string> taskCompletionSource;

		public TaskStringVisitor()
		{
			taskCompletionSource = new TaskCompletionSource<string>();
		}

		public void Visit(string str)
		{
			taskCompletionSource.SetResult(str);
		}

		public Task<string> Task
		{
			get { return taskCompletionSource.Task; }
		}
	}
}
