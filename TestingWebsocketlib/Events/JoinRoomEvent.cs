using WebSocketClient.Events;

namespace PCWebsocketExample.Events {
    public class JoinRoomEvent : WebSocketEvent {
        public override string eventId => "joinRoom";

        [EventDataIndex(0)]
        public string roomCode { get; set; } = string.Empty;

        [EventDataIndex(1)]
        public string? password { get; set; } = string.Empty;
    }

}
