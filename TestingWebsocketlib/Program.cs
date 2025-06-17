using SocketIOClient.Transport;
using PCWebsocketExample.Events;
using PCWebsocketExample.PokemonChess.PokemonChess;
using WebSocketClient;
using WebSocketClient.Events;

namespace PCWebsocketExample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            WebSocketMessageHandler messageHandler = new();
            PokemonChessClient client = await PokemonChessClient.CreateAsync(messageHandler);

            messageHandler.On<WebSocketEventConnected>(async evt => {
                if (client.webSocketClient.GetActiveTransport() == TransportProtocol.WebSocket) {
                    RoomDetails? firstRoom = client.roomManager.availableRooms.FirstOrDefault();
                    if (firstRoom != null)
                        await client!.roomManager.JoinRoomAsync(firstRoom.code, null);
                }
            });

            await client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
