# D2RIpOverlay
An overlay for Diablo 2 Resurrected, that shows the current game server IP (used for anni hunting).
The IP will turn to green if a game is found.

It works by querying all open TCP connections using GetExtendedTcpTable from iphlpapi.dll, filtering for the process "D2R" and putting the found IPs on a clickthroughable overlay which will be displayed as the topmost window.
Since there is now direct interaction with the game itself it should be safe to use (and hopefully not break any terms of use).

This project is still very much in the alpha phase. It works, but it is missing features, will have bugs and needs refractoring. But maybe it will be useful for someone.

# Archived
Since the beginning of ladder 1 IP hunting is no longer a thing.
