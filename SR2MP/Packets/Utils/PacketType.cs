namespace SR2MP.Packets.Utils;

public enum PacketType : byte
{   // Type                 // Hierachy                                     // Exception                                // Use Case
    Connect = 0,            // Client -> Server                                                                         Try to connect to Server
    ConnectAck = 1,         // Server -> Client                                                                         Initiate Player Join
    Close = 2,              // Server -> All Clients                                                                    Broadcast Server Close
    PlayerJoin = 3,         // Server -> All Clients                        (except client that joins)                  Add Player
    PlayerLeave = 4,        // Server -> All Clients                        (except client that left)                   Remove Player
    PlayerUpdate = 5,       // Client -> Server -> All Clients              (except updater)                            Update Player
    Heartbeat = 8,          // Client -> Server                                                                         Check if Clients are alive
    HeartbeatAck = 9,       // Server -> Client                                                                         Automatically time the Clients out if the Server crashes
}