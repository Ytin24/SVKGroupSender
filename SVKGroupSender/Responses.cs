using Newtonsoft.Json;

namespace SVKGroupSender
{
    public class BasicResponse<Type>
    {
        [JsonProperty("response")]
        public Type Response;

        [JsonProperty("error")]
        public Error Error;
    }

    public class ItemsResponse<Type>
    {
        [JsonProperty("count")]
        public int Count;

        [JsonProperty("items")]
        public List<Type> Items;
    }


    public class Error
    {
        [JsonProperty("error_code")]
        public int Code;

        [JsonProperty("error_msg")]
        public string Message;

        [JsonProperty("captcha_sid")]
        public string CaptchaSid;

        [JsonProperty("captcha_img")]
        public string CaptchaImg;

        [JsonProperty("request_params")]
        public List<Dictionary<string, string>> RequestParams;
    }
    public class ConversationResponse
    {
        [JsonProperty("conversation")]
        public ConversationInfo Conversation;

    }
    public class ConversationInfo
    {
        [JsonProperty("peer")]
        public PeerInfo Peer;

        [JsonProperty("can_write")]
        public PeerWriteSettings WriteSettings;
    }
    public class PeerInfo
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("type")]
        public string Type;
    }
    public class PeerWriteSettings
    {
        [JsonProperty("allowed")]
        public bool Allowed;
    }
}
