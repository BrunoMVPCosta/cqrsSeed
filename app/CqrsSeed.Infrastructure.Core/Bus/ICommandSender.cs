using CqrsSeed.Infrastructure.Core.Commands;

namespace CqrsSeed.Infrastructure.Core.Bus
{
    public interface ICommandSender
    {
        void Send(CommandBase command);

    }
}