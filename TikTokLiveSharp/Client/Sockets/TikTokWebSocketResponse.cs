﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TikTokLiveSharp.Client.Sockets
{
    public class TikTokWebSocketResponse
    {
        /// <summary>
        /// Creates a TikTok websocket response instance.
        /// </summary>
        /// <param name="array">Response array.</param>
        /// <param name="count">Response count.</param>
        public TikTokWebSocketResponse(byte[] array, int count)
        {
            Array = array;
            Count = count;
        }

        /// <summary>
        /// Array recieved from web socket.
        /// </summary>
        public readonly byte[] Array;

        /// <summary>
        /// Max count for values recieved from response.
        /// </summary>
        public readonly int Count;
    }
}
