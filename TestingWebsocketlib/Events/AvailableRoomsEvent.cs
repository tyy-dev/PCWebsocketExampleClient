using Newtonsoft.Json;
using WebSocketClient.Events;

namespace PCWebsocketExample.Events {
    public class AvailableRoomsEvent : WebSocketEvent {
        public override string eventId => "availableRooms";

        [EventDataIndex(0, deserializeJson = true)]
        public Dictionary<string, RoomDetails>? rooms { get; set; }

        [EventDataIndexAttribute(1)]
        public string? token { get; set; }
    }
    public class RoomDetails {
        public string socketid { get; set; }
        public string host { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public bool password { get; set; }
        public List<object> spectators { get; set; }
        public string winRate { get; set; }
        [JsonProperty("no_rng")]
        public bool noRng { get; set; }
        [JsonProperty("random_teams")]
        public bool randomTeams { get; set; }
        [JsonProperty("timer_setting")]
        public string timerSetting { get; set; }

        [JsonProperty("timer_millis")]
        public int? timerMillis { get; set; }

        [JsonProperty("timer_add")]
        public int timerAdd { get; set; }
    }

}
