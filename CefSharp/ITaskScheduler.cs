using System.Threading.Tasks;

namespace CefSharp
{
	public interface ITaskScheduler
	{
		void ExecuteTask(Task task);
	}
}
