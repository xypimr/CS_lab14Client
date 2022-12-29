using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace CSlab13
{
    public class BuildingView
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<AuditoriumGroupView> AuditoriumGroupViews { get; set; } = new List<AuditoriumGroupView>();
    }

    public class AuditoriumGroupView
    {
        public int Id { get; set; }

        public int BuildingId { get; set; }

        public int AuditoriumId { get; set; }

        // public string DetailName { get; set; } = null!;
        public Auditorium Auditorium { get; set; } = null!;
        public int Quantity { get; set; }
    }

    public class HttpRequestData
    {
        public static HttpClient client = new() { BaseAddress = new Uri("http://localhost:5204") };

        public static Auditorium GetAuditoriumFromId(int id)
        {
            string url = $"http://localhost:5204/api/Auditorium/{id}";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data = sr1.ReadToEnd();
            var deserializeDetail = JsonConvert.DeserializeObject<Auditorium>(data);
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);

            return deserializeDetail;
        }

        public static List<Auditorium> GetAllAuditoriums()
        {
            string url = "http://localhost:5204/api/Auditoriums";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            var data = sr1.ReadToEnd();
            var deserializeListOfDetails = JsonConvert.DeserializeObject<List<Auditorium>>(data);
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(ans);

            return deserializeListOfDetails;
        }

        public static void AddAuditorium(Auditorium temp)
        {
            Task<HttpResponseMessage> request = client.PostAsJsonAsync($"api/Auditorium", temp);
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.WriteLine(data1);
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
        }

        public static void ChangeAuditorium(Auditorium temp)
        {
            Task<HttpResponseMessage> request = client.PutAsJsonAsync($"api/Auditorium", temp);
            string ans = request.Result.StatusCode.ToString();
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            Console.Write(ans);
        }

        public static void DeleteAuditorium(Auditorium temp)
        {
            string url = $"http://localhost:5204/api/Auditorium/{temp.Id}";
            Task<HttpResponseMessage> request =
                new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Delete, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
        }

        public static Building GetBuildingFromId(int id)
        {
            string url = $"http://localhost:5204/api/Building/{id}";
            Task<HttpResponseMessage> request = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data = sr1.ReadToEnd();
            var deserializeAssemblyView = JsonConvert.DeserializeObject<BuildingView>(data);
            Building temp = new Building() { Id = deserializeAssemblyView.Id, Name = deserializeAssemblyView.Name };
            List<AuditoriumGroup> tempAuditoriumGroups = new List<AuditoriumGroup>();
            foreach (var VARIABLE in deserializeAssemblyView.AuditoriumGroupViews)
            {
                AuditoriumGroup auditoriumGroup = new AuditoriumGroup()
                {
                    Id = VARIABLE.Id,
                    BuildingId = temp.Id,
                    Building = temp,
                    Quantity = VARIABLE.Quantity,
                    // DetailName = VARIABLE.DetailName, /////////////////////////////////////////////////////////////
                    AuditoriumId = VARIABLE.AuditoriumId,
                    Auditorium = VARIABLE.Auditorium
                };
                tempAuditoriumGroups.Add(auditoriumGroup);
            }

            temp.AuditoriumGroups = tempAuditoriumGroups;

            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
            
            
            return temp;
        }
        
        public static List<Building> GetAllBuilding()
        {
            string url = "http://localhost:5204/api/Buildings";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data = sr1.ReadToEnd();
            Console.WriteLine(data);
            var deserializeObject = JsonConvert.DeserializeObject<List<BuildingView>>(data);
            if (deserializeObject == null)
                return new List<Building>();
            List<Building> list = new List<Building>();
            foreach (var assemblyView in deserializeObject)
            {
                Building temp = new Building() { Id = assemblyView.Id, Name = assemblyView.Name };
                List<AuditoriumGroup> tempParts = new List<AuditoriumGroup>();
                foreach (var VARIABLE in assemblyView.AuditoriumGroupViews)
                {
                    AuditoriumGroup auditoriumGroup = new AuditoriumGroup()
                    {
                        Id = VARIABLE.Id,
                        Building = temp,
                        BuildingId = temp.Id,
                        Quantity = VARIABLE.Quantity,
                        // DetailName = VARIABLE.DetailName, /////////////////////////////////////////////////////////////
                        AuditoriumId = VARIABLE.AuditoriumId,
                        Auditorium = VARIABLE.Auditorium
                    };
                    tempParts.Add(auditoriumGroup);
                }

                temp.AuditoriumGroups = tempParts;
                list.Add(temp);
            }

            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
            return list;
        }

        public static void AddBuilding(Building building)
        {
            List<AuditoriumGroup> auditoriumGroups = building.AuditoriumGroups;
            List<AuditoriumGroupView> auditoriumGroupViews = new List<AuditoriumGroupView>();
            for (int i = 0; i < auditoriumGroups.Count; i++)
            {
                AuditoriumGroupView auditoriumGroupView = new AuditoriumGroupView()
                {
                    BuildingId = auditoriumGroups[i].BuildingId,
                    Auditorium = auditoriumGroups[i].Auditorium,
                    Id = auditoriumGroups[i].Id,
                    Quantity = auditoriumGroups[i].Quantity,
                    AuditoriumId = auditoriumGroups[i].AuditoriumId,
                };
                auditoriumGroupViews.Add(auditoriumGroupView);
            }

            BuildingView temp = new BuildingView()
            {
                Id = building.Id,
                Name = building.Name,
                AuditoriumGroupViews = auditoriumGroupViews
            };

            Task<HttpResponseMessage> request = client.PostAsJsonAsync(
                $"api/Building", temp);
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(data1);
            Console.Write(ans);
        }

        public static void ChangeBuilding(Building building)
        {
            List<AuditoriumGroup> auditoriumGroups = building.AuditoriumGroups;
            List<AuditoriumGroupView> auditoriumGroupViews = new List<AuditoriumGroupView>();
            for (int i = 0; i < auditoriumGroups.Count; i++)
            {
                AuditoriumGroupView auditoriumGroupView = new AuditoriumGroupView()
                {
                    BuildingId = auditoriumGroups[i].BuildingId,
                    Auditorium = auditoriumGroups[i].Auditorium,
                    Id = auditoriumGroups[i].Id,
                    Quantity = auditoriumGroups[i].Quantity,
                    AuditoriumId = auditoriumGroups[i].AuditoriumId,
                };
                auditoriumGroupViews.Add(auditoriumGroupView);
            }

            BuildingView temp = new BuildingView()
            {
                Id = building.Id,
                Name = building.Name,
                AuditoriumGroupViews = auditoriumGroupViews
            };

            Task<HttpResponseMessage> request = client.PutAsJsonAsync(
                $"api/Building", temp);
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(data1);
            Console.Write(ans);
        }

        public static void DeleteBuilding(Building building)
        {
            string url = $"http://localhost:5204/api/Building/{building.Id}";
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