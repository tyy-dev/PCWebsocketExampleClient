using PCWebsocketExample.Events;
using PCWebsocketExample.PokemonChess.PokemonChess;

namespace PCWebsocketExample.PokemonChess {
    /// <summary>
    /// Manages available rooms and room-related operations
    /// </summary>
    public class PokemonChessRoomManager(PokemonChessClient client) {
        private readonly PokemonChessClient client = client;

        /// <summary>
        /// Gets or sets the list of currently available rooms.
        /// </summary>
        public List<RoomDetails> availableRooms = [];

        /// <summary>
        /// Joins a room asynchronously using the specified code and optional password.
        /// </summary>
        /// <param name="code">The room code to join.</param>
        /// <param name="password">The optional password for the room.</param>
        /// <returns>A task representing the asynchronous join operation.</returns>
        public async Task JoinRoomAsync(string code, string? password = null) {
            await client.webSocketClient.Emit(new JoinRoomEvent() {
                roomCode = code,
                password = password,
            });
        }
    }
}
