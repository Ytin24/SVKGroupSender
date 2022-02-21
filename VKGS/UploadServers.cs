using Newtonsoft.Json;

namespace VKGS
{
    public class UploadServers
    {
        public class PhotoMessages : UploadServerBase
        {
            [JsonProperty("album_id")]
            public int AlbumId;

            [JsonProperty("user_id")]
            public int UserId;

            [JsonProperty("group_id")]
            public int GroupId;

            public class UploadResult
            {
                [JsonProperty("server")]
                public int Server;

                [JsonProperty("photo")]
                public string Photo;

                [JsonProperty("hash")]
                public string Hash;
            }
        }


        public class UploadServerBase
        {
            [JsonProperty("upload_url")]
            public string UploadUrl;
        }
    }
}