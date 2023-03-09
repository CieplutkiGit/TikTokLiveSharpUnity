﻿using System;
using System.Collections.Generic;
using TikTokLiveSharp.Client;

namespace TikTokLiveSharp.Utils
{
    internal static class Constants
    {
        public const string TIKTOK_URL_WEB = "https://www.tiktok.com/";
        public const string TIKTOK_URL_WEBCAST = "https://webcast.tiktok.com/webcast/";
        public const string TIKTOK_SIGN_API = "https://tiktok.isaackogan.com/webcast/sign_url";

        public const double DEFAULT_TIMEOUT = 20f;
        public const double DEFAULT_POLLTIME = .5f;
        public static readonly ClientSettings DEFAULT_SETTINGS = new ClientSettings()
        {
            Timeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT),
            PollingInterval = TimeSpan.FromSeconds(DEFAULT_POLLTIME),
            ClientLanguage = "en-US",
            FetchRoomInfoOnConnect = true,
            HandleExistingMessagesOnConnect = true,
            DownloadGiftInfo = true,
            Proxy = null,
            SocketBufferSize = 10_000
        };

        public static readonly IReadOnlyDictionary<string, object> DEFAULT_CLIENT_PARAMS = new Dictionary<string, object>()
        {
            { "aid",  1988},
            { "app_language",  "en-US"},
            { "app_name",  "tiktok_web"},
            { "browser_language",  "en"},
            { "browser_name",  "Mozilla"},
            { "browser_online",  true},
            { "browser_platform",  "Win32"},
            { "browser_version",  "5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.63 Safari/537.36"},
            { "cookie_enabled",  true},
            { "cursor",  ""},
            { "internal_ext",  ""},
            { "device_platform",  "web"},
            { "focus_state",  true},
            { "from_page",  "user"},
            { "history_len",  4},
            { "is_fullscreen",  false},
            { "is_page_visible",  true},
            { "did_rule",  3},
            { "fetch_rule",  1},
            { "identity",  "audience"},
            { "last_rtt",  0},
            { "live_id",  12},
            { "resp_content_type",  "protobuf"},
            { "screen_height",  1152},
            { "screen_width",  2048},
            { "tz_name",  "Europe/Berlin"},
            { "referer",  "https, //www.tiktok.com/"},
            { "root_referer",  "https, //www.tiktok.com/"},
            { "msToken",  ""},
            { "version_code",  180800},
            { "webcast_sdk_version",  "1.3.0"},
            { "update_version_code",  "1.3.0" }
        };

        public static readonly IReadOnlyDictionary<string, string> DEFAULT_REQUEST_HEADERS = new Dictionary<string, string>()
        {
            { "Connection", "keep-alive" },
            { "Cache-Control", "max-age=0" },
            { "Accept", "text/html,application/json,application/protobuf" },
            { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.63 Safari/537.36" },
            { "Referer", "https://www.tiktok.com/" },
            { "Origin", "https://www.tiktok.com" },
            { "Accept-Language", "en-US,en; q=0.9" },
            { "Accept-Encoding", "gzip, deflate" }
        };
    }
}