using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace Service.IA.Util
{
    public class HistoricoBanco : ChatHistoryProvider
    {
        public async Task AddMessageAsync(string sessionId, ChatMessage message)
        {
            //using var conn = new NpgsqlConnection(_connectionString);
            //await conn.OpenAsync();
            //
            //var cmd = new NpgsqlCommand(
            //    "INSERT INTO chat_history (session_id, role, content) VALUES (@sessionId, @role, @content)",
            //    conn
            //);
            //cmd.Parameters.AddWithValue("sessionId", sessionId);
            //cmd.Parameters.AddWithValue("role", message.Role);
            //cmd.Parameters.AddWithValue("content", message.Content);
            //
            //await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IReadOnlyList<ChatMessage>> GetMessagesAsync(string sessionId)
        {
            var messages = new List<ChatMessage>();

            //using var conn = new NpgsqlConnection(_connectionString);
            //await conn.OpenAsync();
            //
            //var cmd = new NpgsqlCommand(
            //    "SELECT role, content FROM chat_history WHERE session_id = @sessionId ORDER BY id ASC",
            //    conn
            //);
            //cmd.Parameters.AddWithValue("sessionId", sessionId);
            //
            //using var reader = await cmd.ExecuteReaderAsync();
            //while (await reader.ReadAsync())
            //{
            //    messages.Add(new ChatMessage(
            //        reader.GetString(0), // role
            //        reader.GetString(1)  // content
            //    ));
            //}

            return messages;
        }


    }
}
