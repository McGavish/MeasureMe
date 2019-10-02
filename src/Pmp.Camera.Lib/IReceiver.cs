using System.Threading.Tasks;

namespace Pmp.Camera.Lib
{
    public interface IReceiver<in T>
    {
        Task Continue(T t = default);
    }
}