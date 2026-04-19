using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SignalForge.Models.Dtos;

namespace SignalForge.Managers.Chat;

public interface IChatManager
{
    Task<MessageDto> SendMessageAsync(Guid senderId, SendMessageDto input, CancellationToken cancellationToken = default);
    Task<MessageDto> EditMessageAsync(Guid editorId, Guid messageId, string newContent, CancellationToken cancellationToken = default);
    Task<bool> DeleteMessageAsync(Guid deleterId, Guid messageId, CancellationToken cancellationToken = default);
    Task<bool> MarkAsReadAsync(Guid readerId, Guid messageId, CancellationToken cancellationToken = default);
    Task<List<MessageDto>> GetGroupMessagesAsync(Guid groupId, int skip = 0, int take = 50, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(Guid userId, Guid groupId, CancellationToken cancellationToken = default);
}
