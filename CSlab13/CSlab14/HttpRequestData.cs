using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace CSlab13
{
    public class AssemblyView
    {
        public string name { get; set; }
        public int id { get; set; }
        public List<PartView> PartViews { get; set; } = new List<PartView>();
    }

    public class PartView
    {
        public int Id { get; set; }

        public int AssemblyId { get; set; }

        public int DetailId { get; set; }

        public string DetailName { get; set; } = null!;
        public Detail Detail { get; set; } = null!;
        public int Quantity { get; set; }
    }

    public class HttpRequestData
    {
        public static HttpClient client = new() { BaseAddress = new Uri("http://localhost:5204") };

        public static Detail GetDetailFromId(int id)
        {
            string url = $"http://localhost:5204/api/Detail/{id}";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data = sr1.ReadToEnd();
            var deserializeDetail = JsonConvert.DeserializeObject<Detail>(data);
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);

            return deserializeDetail;
        }

        public static Detail GetDetailFromName(string name)
        {
            string url = $"http://localhost:5204/api/Detail/{name}";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data = sr1.ReadToEnd();
            var deserializeDetail = JsonConvert.DeserializeObject<Detail>(data);
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);

            return deserializeDetail;
        }

        public static List<Detail> GetAllDetails()
        {
            string url = "http://localhost:5204/api/Details";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            var data = sr1.ReadToEnd();
            var deserializeListOfDetails = JsonConvert.DeserializeObject<List<Detail>>(data);
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(ans);

            return deserializeListOfDetails;
        }

        public static void AddDetail(Detail temp)
        {
            // client.BaseAddress = new Uri("http://localhost:5204");
            Task<HttpResponseMessage> request = client.PostAsJsonAsync($"api/Detail", temp);
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.WriteLine(data1);
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
        }

        public static void ChangeDetail(Detail temp)
        {
            Task<HttpResponseMessage> request = client.PutAsJsonAsync($"api/Detail", temp);
            string ans = request.Result.StatusCode.ToString();
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            Console.Write(ans);
        }

        public static void DeleteDetail(Detail temp)
        {
            string url = $"http://localhost:5204/api/Detail/{temp.Id}";
            Task<HttpResponseMessage> request =
                new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Delete, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
        }

        public static Assembly GetAssemblyFromId(int id)
        {
            string url = $"http://localhost:5204/api/Assembly/{id}";
            Task<HttpResponseMessage> request = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data = sr1.ReadToEnd();
            var deserializeAssemblyView = JsonConvert.DeserializeObject<AssemblyView>(data);
            Assembly temp = new Assembly() { Id = deserializeAssemblyView.id, Name = deserializeAssemblyView.name };
            List<Part> tempParts = new List<Part>();
            foreach (var VARIABLE in deserializeAssemblyView.PartViews)
            {
                Part part = new Part()
                {
                    Id = VARIABLE.Id,
                    AssemblyId = temp.Id,
                    Assembly = temp,
                    Quantity = VARIABLE.Quantity,
                    // DetailName = VARIABLE.DetailName, /////////////////////////////////////////////////////////////
                    DetailId = VARIABLE.DetailId,
                    Detail = VARIABLE.Detail
                };
                tempParts.Add(part);
            }

            temp.Parts = tempParts;

            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
            
            
            return temp;
        }

        public static Assembly GetAssemblyFromName(string name)
        {
            string url = $"http://localhost:5204/api/Assembly/{name}";
            Task<HttpResponseMessage> request = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data = sr1.ReadToEnd();
            var deserializeAssemblyView = JsonConvert.DeserializeObject<AssemblyView>(data);
            Assembly temp = new Assembly() { Id = deserializeAssemblyView.id, Name = deserializeAssemblyView.name };
            List<Part> tempParts = new List<Part>();
            foreach (var VARIABLE in deserializeAssemblyView.PartViews)
            {
                Part part = new Part()
                {
                    Id = VARIABLE.Id,
                    Assembly = temp,
                    AssemblyId = temp.Id,
                    Quantity = VARIABLE.Quantity,
                    // DetailName = VARIABLE.DetailName, /////////////////////////////////////////////////////////////
                    DetailId = VARIABLE.DetailId,
                    Detail = VARIABLE.Detail
                };
                tempParts.Add(part);
            }

            temp.Parts = tempParts;

            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
            return temp;
        }

        public static List<Assembly> GetAllAssemblies()
        {
            string url = "http://localhost:5204/api/Assemblies";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data = sr1.ReadToEnd();
            Console.WriteLine(data);
            var deserializeListAssembliesView = JsonConvert.DeserializeObject<List<AssemblyView>>(data);
            if (deserializeListAssembliesView == null)
                return new List<Assembly>();
            List<Assembly> list = new List<Assembly>();
            foreach (var assemblyView in deserializeListAssembliesView)
            {
                Assembly temp = new Assembly() { Id = assemblyView.id, Name = assemblyView.name };
                List<Part> tempParts = new List<Part>();
                foreach (var VARIABLE in assemblyView.PartViews)
                {
                    Part part = new Part()
                    {
                        Id = VARIABLE.Id,
                        Assembly = temp,
                        AssemblyId = temp.Id,
                        Quantity = VARIABLE.Quantity,
                        // DetailName = VARIABLE.DetailName, /////////////////////////////////////////////////////////////
                        DetailId = VARIABLE.DetailId,
                        Detail = VARIABLE.Detail
                    };
                    tempParts.Add(part);
                }

                temp.Parts = tempParts;
                list.Add(temp);
            }

            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
            return list;
        }

        public static void AddAssembly(Assembly assembly)
        {
            List<Part> parts = assembly.Parts;
            List<PartView> partViews = new List<PartView>();
            for (int i = 0; i < parts.Count; i++)
            {
                PartView partView = new PartView()
                {
                    AssemblyId = parts[i].AssemblyId,
                    Detail = parts[i].Detail,
                    Id = parts[i].Id,
                    Quantity = parts[i].Quantity,
                    DetailId = parts[i].DetailId,
                    DetailName = parts[i].Detail.Name
                };
                partViews.Add(partView);
            }

            AssemblyView temp = new AssemblyView()
            {
                id = assembly.Id,
                name = assembly.Name,
                PartViews = partViews
            };

            Task<HttpResponseMessage> request = client.PostAsJsonAsync(
                $"api/Assembly", temp);
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(data1);
            Console.Write(ans);
        }

        public static void ChangeAssembly(Assembly assembly)
        {
            List<Part> parts = assembly.Parts;
            List<PartView> partViews = new List<PartView>();
            for (int i = 0; i < parts.Count; i++)
            {
                PartView partView = new PartView()
                {
                    AssemblyId = parts[i].AssemblyId,
                    Detail = parts[i].Detail,
                    Id = parts[i].Id,
                    Quantity = parts[i].Quantity,
                    DetailId = parts[i].DetailId,
                    DetailName = parts[i].Detail.Name
                };
                partViews.Add(partView);
            }

            AssemblyView temp = new AssemblyView()
            {
                id = assembly.Id,
                name = assembly.Name,
                PartViews = partViews
            };

            Task<HttpResponseMessage> request = client.PutAsJsonAsync(
                $"api/Assembly", temp);
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(data1);
            Console.Write(ans);
        }

        public static void DeleteAssembly(Assembly assembly)
        {
            string url = $"http://localhost:5204/api/Assembly/{assembly.Id}";
            Task<HttpResponseMessage> request =
                new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Delete, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(ans);
        }
    }
}