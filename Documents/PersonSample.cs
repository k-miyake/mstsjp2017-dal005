using Newtonsoft.Json;

namespace Documents
{

    public class PersonSample
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "address")]
        public Address2 Address { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "contacts")]
        public Contact2[] Contacts { get; set; }
    }

    public class Address2
    {
        [JsonProperty(PropertyName = "line1")]
        public string Line1 { get; set; }
        [JsonProperty(PropertyName = "line2")]
        public string Line2 { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "zip")]
        public int Zip { get; set; }
    }

    public class Contact2
    {
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }

}
