﻿using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TikTokLiveSharp.Client.Requests;

namespace TikTokLiveSharp.Client.Sockets
{
    public class TikTokWebSocket : ITikTokWebSocket
    {
        private ClientWebSocket clientWebSocket;

        private uint bufferSize;

        /// <summary>
        /// Creates a TikTok websocket instance.
        /// </summary>
        /// <param name="cookieContainer">The cookie container to use.</param>
        public TikTokWebSocket(TikTokCookieJar cookieContainer, uint buffSize = 10_000)
        {
            bufferSize = 200_000;
            clientWebSocket = new ClientWebSocket();
            clientWebSocket.Options.AddSubProtocol("echo-protocol");
            clientWebSocket.Options.KeepAliveInterval = TimeSpan.FromSeconds(15);
            StringBuilder cookieHeader = new StringBuilder();
            foreach (string cookie in cookieContainer)
                cookieHeader.Append(cookie);
            clientWebSocket.Options.SetRequestHeader("Cookie", cookieHeader.ToString());
        }

        /// <summary>
        /// Connect to the websocket.
        /// </summary>
        /// <param name="url">Websocket url.</param>
        /// <returns>Task to await.</returns>
        public async Task Connect(string url)
        {
            await clientWebSocket.ConnectAsync(new Uri(url), CancellationToken.None);
        }

        /// <summary>
        /// Disconnects from the websocket.
        /// </summary>
        /// <returns>Task to await.</returns>
        public async Task Disconnect()
        {
            await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        }

        /// <summary>
        /// Writes a message to the websocket.
        /// </summary>
        /// <param name="arr">The bytes to write.</param>
        /// <returns>Task to await.</returns>
        public async Task WriteMessage(ArraySegment<byte> arr)
        {
            await clientWebSocket.SendAsync(arr, WebSocketMessageType.Binary, true, CancellationToken.None);
        }

        /// <summary>
        /// Recieves a message from websocket.
        /// </summary>
        /// <returns></returns>
        public async Task<TikTokWebSocketResponse> ReceiveMessage()
        {
            var arr = new ArraySegment<byte>(new byte[bufferSize]);
            WebSocketReceiveResult response = await clientWebSocket.ReceiveAsync(arr, CancellationToken.None);
            if (response.MessageType == WebSocketMessageType.Binary)
                return new TikTokWebSocketResponse(arr.Array, response.Count);
            return null;
        }

        /// <summary>
        /// Is the websocket currently connected.
        /// </summary>
        public bool IsConnected => clientWebSocket.State == WebSocketState.Open;
    }
}
