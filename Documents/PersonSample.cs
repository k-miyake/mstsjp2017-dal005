using Newtonsoft.Json;

namespace Documents
{

    public class PersonSample
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "id")]
        public Address2 Address { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "id")]
        public Contact2[] Contacts { get; set; }
    }

    public class Address2
    {
        [JsonProperty(PropertyName = "id")]
        public string Line1 { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Line2 { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int Zip { get; set; }
    }

    public class Contact2
    {
        [JsonProperty(PropertyName = "id")]
        public string Email { get; set; }
    }

}
