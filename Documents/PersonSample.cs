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
        public Address Address { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "id")]
        public Contact[] Contacts { get; set; }
    }

    public class Address
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

    public class Contact
    {
        [JsonProperty(PropertyName = "id")]
        public string Email { get; set; }
    }

}
