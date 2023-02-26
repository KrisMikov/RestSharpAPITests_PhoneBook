using RestSharp;
using System.Net;
using System.Numerics;
using System.Text.Json;

namespace RestSharpAPITests
{
    public class RestSharpAPI_Tests
    {
        private RestClient client;
        private const string baseUrl = "https://contactbook.krismikov.repl.co/api";

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient(baseUrl);
        }

        [Test]

        public void Test_List_Contacts()
        {
            var request = new RestRequest("contacts", Method.Get);
            var response = this.client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

           var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);
           Assert.That(contacts[0].firstName, Is.EqualTo("Steve"));
           Assert.That(contacts[0].lastName, Is.EqualTo("Jobs"));
        }

        [Test]
        public void Test_Find_Contact_By_Keyword()
        {
            var request = new RestRequest("contacts/search/albert", Method.Get);
            var response = this.client.Execute(request);

            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);
            Assert.That(contacts[0].firstName, Is.EqualTo("Albert"));
            Assert.That(contacts[0].lastName, Is.EqualTo("Einstein"));
        }

        [Test]
        public void Test_Find_Contast_Missing_Word()
        {
            var request = new RestRequest("contacts/search/missing" + DateTime.Now.Ticks, Method.Get);
            var response = this.client.Execute(request);

            Assert.That(response.Content, Is.EqualTo("[]"));
        }

    

        [Test]
        public void Test_Create_New_Contact_Invalid_Data()
        {
            var request = new RestRequest("contacts", Method.Post);
            var reqBody = new
            {
                email = "othermail@abv.bg",
                phone = "088997765544",
                dateCreated = DateTime.Now.Ticks
            };
            request.AddBody(reqBody);
            var response = this.client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        }

        [Test]
        public void Test_Create_New_Contact_Valid_Data()
        {
            var request = new RestRequest("contacts", Method.Post);
            var reqBody = new
            {
                firstName = "Some first name",
                lastName = "Some last name",
                email = "newemail@abv.bg",
                phone = "0889977665544",
                dateCreated = DateTime.Now.Ticks
            };
            request.AddBody(reqBody);
            var response = this.client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
          
            
        }


    }
}