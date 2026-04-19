using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalForge.Entities;
using SignalForge.EntityFrameworkCore;
using SignalForge.Models.Dtos;
using SignalForge.Models.Enums;

namespace SignalForge.Managers.Chat;

public class ChatManager : IChatManager
{
    private readonly ISignalForgeDbContext _dbContext;

    public ChatManager(ISignalForgeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<MessageDto> SendMessageAsync(Guid senderId, SendMessageDto input, CancellationToken cancellationToken = default)
    {
        var message = new ChatMessage
        {
            Id = Guid.NewGuid(),
            GroupId = input.GroupId,
            SenderUserId = senderId,
            Content = input.Content,
            Status = MessageStatus.Sent,
            SentAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _dbContext.ChatMessages.AddAsync(message, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var sender = await _dbContext.ChatUsers.FindAsync(new object[] { senderId }, cancellationToken);

        return new MessageDto
        {
            Id = message.Id,
            GroupId = message.GroupId,
            SenderUserId = message.SenderUserId,
            SenderDisplayName = sender?.DisplayName ?? "Unknown",
            Content = message.Content,
            Status = message.Status,
            SentAt = message.SentAt,
            IsDeleted = message.IsDeleted
        };
    }

    public virtual async Task<MessageDto> EditMessageAsync(Guid editorId, Guid messageId, string newContent, CancellationToken cancellationToken = default)
    {
        var msg = await _dbContext.ChatMessages
            .Include(m => m.Sender)
            .FirstOrDefaultAsync(m => m.Id == messageId, cancellationToken);

        if (msg == null || msg.SenderUserId != editorId || msg.IsDeleted)
            throw new Exception("Message not found or unauthorized.");

        msg.Content = newContent;
        msg.EditedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new MessageDto
        {
            Id = msg.Id,
            GroupId = msg.GroupId,
            SenderUserId = msg.SenderUserId,
            SenderDisplayName = msg.Sender.DisplayName,
            Content = msg.Content,
            Status = msg.Status,
            SentAt = msg.SentAt,
            EditedAt = msg.EditedAt,
            IsDeleted = msg.IsDeleted
        };
    }

    public virtual async Task<bool> DeleteMessageAsync(Guid deleterId, Guid messageId, CancellationToken cancellationToken = default)
    {
        var msg = await _dbContext.ChatMessages
            .FirstOrDefaultAsync(m => m.Id == messageId, cancellationToken);

        if (msg == null || msg.SenderUserId != deleterId)
            return false;

        // Soft delete
        msg.IsDeleted = true;
        msg.Content = "[Deleted]";
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public virtual async Task<bool> MarkAsReadAsync(Guid readerId, Guid messageId, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.MessageReadReceipts
            .AnyAsync(r => r.MessageId == messageId && r.UserId == readerId, cancellationToken);

        if (exists) return false;

        var msg = await _dbContext.ChatMessages.FindAsync(new object[] { messageId }, cancellationToken);
        if (msg == null) return false;

        var receipt = new MessageReadReceipt
        {
            Id = Guid.NewGuid(),
            MessageId = messageId,
            UserId = readerId,
            ReadAt = DateTime.UtcNow
        };

        if (msg.Status != MessageStatus.Read)
        {
            msg.Status = MessageStatus.Read;
        }

        await _dbContext.MessageReadReceipts.AddAsync(receipt, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public virtual async Task<List<MessageDto>> GetGroupMessagesAsync(Guid groupId, int skip = 0, int take = 50, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ChatMessages
            .Include(m => m.Sender)
            .Where(m => m.GroupId == groupId)
            .OrderByDescending(m => m.SentAt)
            .Skip(skip)
            .Take(take)
            .Select(m => new MessageDto
            {
                Id = m.Id,
                GroupId = m.GroupId,
                SenderUserId = m.SenderUserId,
                SenderDisplayName = m.Sender.DisplayName,
                Content = m.IsDeleted ? "[Deleted]" : m.Content,
                Status = m.Status,
                SentAt = m.SentAt,
                EditedAt = m.EditedAt,
                IsDeleted = m.IsDeleted
            })
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<int> GetUnreadCountAsync(Guid userId, Guid groupId, CancellationToken cancellationToken = default)
    {
        // Get all messages in group not read by this user
        var query = from m in _dbContext.ChatMessages
                    where m.GroupId == groupId && m.SenderUserId != userId && !m.IsDeleted
                    where !_dbContext.MessageReadReceipts.Any(r => r.MessageId == m.Id && r.UserId == userId)
                    select m.Id;

        return await query.CountAsync(cancellationToken);
    }
}
