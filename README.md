# PlayerRankingModule

I spent approximately 10 hours on this part for development and testing.
I spend some time on message and logging system, in order to avoid security issues and bugs that would crash the application.
Testing (connection with server with correct request and database parameters) took a significant amount of time.

Technical choices :
I used UnityWebRequest (UnityEngine.Networking) to send request and communicate with NodeJS server.
I had the choice to use WebSocket, Socket.io but I set up UnityWebRequest for this use case.
I used Unity Custom Editor to managed some parameters.
I used JSON.net for some Json conversions.
I made 2 scenes to separate the sending of requests and the database display saved in memory.

Which seemed difficult to me:
Relearn the C# and Unity syntax when have juggled several programming languages (C++, java, ...).
Pool developments on the client and server and make them communicate with each other with the right type of data, headers and syntax.
Use Unity Custom Editor in the right way to managed parameters (with few time to master them).
Debuging some specific aspects of requests for example (header content-type managament)

If I had to push the project a step further :
- Integrate "rank fetch" for the N first scores (which is a server functionality).
- add an admin parameter to access deleteUserID.
- improve the regex system, error messages and logs
- add a search bar (by userID) in the Rankings page.
- change the way for custom editor management: (current way with singleton and static) to targets or scripting parameters to save on non-runtime.
- improve the custom editor to be more modular and reusable.

If I has to change something, it would be the custom editors because I am not completely satisfied with my way of doing them.