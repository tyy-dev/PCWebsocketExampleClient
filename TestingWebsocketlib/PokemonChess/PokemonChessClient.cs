using SocketIOClient;
using SocketIOClient.Transport;
using PCWebsocketExample.Events;
using WebSocketClient;
using WebSocketClient.Events;

namespace PCWebsocketExample.PokemonChess {
    namespace PokemonChess {
        public class PokemonChessClient {

            /// <summary>
            /// The underlying WebSocket client used for communication.
            /// </summary>
            public readonly WebSocketClient.WebSocketClient webSocketClient;

            /// <summary>
            /// The message handler that processes incoming WebSocket messages and events.
            /// </summary>
            public WebSocketMessageHandler messageHandler {
                get;
            }

            /// <summary>
            /// Manages the available rooms and room operations.
            /// </summary>
            public PokemonChessRoomManager roomManager {
                get; private set;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="PokemonChessClient"/> class.
            /// Private constructor to enforce use of the asynchronous factory method.
            /// </summary>
            /// <param name="messageHandler">Optional message handler. If null, a default is created.</param>
            private PokemonChessClient(WebSocketMessageHandler? messageHandler = null) {
                this.messageHandler = messageHandler ?? new();

                this.webSocketClient = new WebSocketClient.WebSocketClient(
                    url: "wss://pokemonchess.com:2999",
                    messageHandler: this.messageHandler,
                    isSocketIO: true,
                    socketIOOptions: new SocketIOOptions {
                        EIO = SocketIO.Core.EngineIO.V4,
                        Transport = TransportProtocol.Polling,
                        AutoUpgrade = false,
                        Query = [
                            new KeyValuePair<string, string>("beta", "true")
                        ]
                    }
                );

                this.RegisterEventHandlers();
                this.RegisterManagers();
            }

            /// <summary>
            /// Asynchronously creates and connects a new instance of the <see cref="PokemonChessClient"/>.
            /// </summary>
            /// <param name="messageHandler">Optional message handler to customize message processing.</param>
            /// <returns>The connected <see cref="PokemonChessClient"/> instance.</returns>
            public static async Task<PokemonChessClient> CreateAsync(WebSocketMessageHandler? messageHandler = null) {
                PokemonChessClient client = new(messageHandler);
                await client.webSocketClient.ConnectAsync();
                return client;
            }

            private void RegisterManagers() => this.roomManager = new(this);

            private void RegisterEventHandlers() {
                this.messageHandler.On<WebSocketEventConnected>(_ => Console.WriteLine("[Connected]"));
                this.messageHandler.On<WebSocketEventCloseConnection>(evt => Console.WriteLine($"[Closed] Reason={evt.status}, Desc={evt.description}"));

                this.messageHandler.On<AvailableRoomsEvent>(evt => {
                    roomManager.availableRooms = evt.rooms?.Values.ToList() ?? [];
                    Console.WriteLine($"[Available Rooms] Rooms={roomManager.availableRooms.Count}");
                });

                this.messageHandler.On(evt => {
                    if (evt.innerEventEventId != evt.eventId)
                        Console.WriteLine($"Event={evt.innerEventEventId}, Data={string.Join(", ", evt.rawData)}");
                });
            }

            /// <summary>
            /// Connects to the server, requests the list of rooms, and switches to the WebSocket transport protocol.
            /// </summary>
            /// <returns>A task that represents the asynchronous connect operation.</returns>
            public async Task ConnectAsync() {
                await this.webSocketClient.Emit("listRooms");
                await this.webSocketClient.SwitchTransportAsync(transport: TransportProtocol.WebSocket);
            }
        }
    }
}
