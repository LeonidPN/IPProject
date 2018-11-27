using IPProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;

namespace IPProject.Services
{
    public class NewsService
    {
        private readonly string save = "";

        private readonly string dir = "";

        public NewsService()
        {
            save = ConfigurationManager.AppSettings["Saving"];
            dir = ConfigurationManager.AppSettings["Dir"];
        }

        public void Add(News news)
        {
            if (save.Equals("file"))
            {
                List<News> list = Deserialize();
                News element = list.FirstOrDefault(rec => rec.Title == news.Title);
                if (element != null)
                {
                    throw new Exception("Уже есть такая новость");
                }
                int maxId = list.Count > 0 ? list.Max(rec => rec.Id) : 0;
                list.Add(new News
                {
                    Id = maxId + 1,
                    Description = news.Description,
                    ImageUrl = news.ImageUrl,
                    Title = news.Title,
                    Body = news.Body,
                    CategoryId = news.CategoryId,
                    DateCreate = DateTime.Now,
                    NumberOfViews = 0,
                    UserLogin = news.UserLogin
                });
                Serialize(list);
            }
        }

        public List<News> GetList()
        {
            List<News> res = new List<News>();
            if (save.Equals("file"))
            {
                res = Deserialize();
            }
            return res;
        }

        public News GetElement(int id)
        {
            if (save.Equals("file"))
            {
                List<News> list = Deserialize();
                News element = list.FirstOrDefault(rec => rec.Id == id);
                if (element != null)
                {
                    return new News
                    {
                        Id = element.Id,
                        UserLogin = element.UserLogin,
                        NumberOfViews = element.NumberOfViews,
                        DateCreate = element.DateCreate,
                        CategoryId = element.CategoryId,
                        Body = element.Body,
                        Description = element.Description,
                        ImageUrl = element.ImageUrl,
                        Title = element.Title
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public List<News> GetListByCategory(int categoryId)
        {
            List<News> res = new List<News>();
            if (save.Equals("file"))
            {
                List<News> list = Deserialize();
                res = list.Select(rec => new News
                {
                    Id = rec.Id,
                    Title = rec.Title,
                    ImageUrl = rec.ImageUrl,
                    Description = rec.Description,
                    Body = rec.Body,
                    CategoryId = rec.CategoryId,
                    DateCreate = rec.DateCreate,
                    NumberOfViews = rec.NumberOfViews,
                    UserLogin = rec.UserLogin
                }).Where(rec => rec.CategoryId == categoryId).ToList();
            }
            return res;
        }

        public void UpdElement(News model)
        {
            if (save.Equals("file"))
            {
                List<News> list = Deserialize();
                News element = list.FirstOrDefault(rec => rec.Id != model.Id && rec.Title == model.Title);
                if (element != null)
                {
                    throw new Exception("Уже есть такая новость");
                }
                element = list.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                element.Title = model.Title;
                element.ImageUrl = model.ImageUrl;
                element.Description = model.Description;
                element.Body = model.Body;
                element.CategoryId = model.CategoryId;
                element.DateCreate = model.DateCreate;
                element.NumberOfViews = model.NumberOfViews;
                element.UserLogin = model.UserLogin;
                Serialize(list);
            }
        }

        public void DelElement(int id)
        {
            if (save.Equals("file"))
            {
                List<News> list = Deserialize();
                News element = list.FirstOrDefault(rec => rec.Id == id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                else
                {
                    list.Remove(element);
                    Serialize(list);
                }
            }
        }

        public List<News> GetInteresting()
        {
            List<News> res = new List<News>();
            if (save.Equals("file"))
            {
                List<News> list = Deserialize();
                Random r = new Random();
                for (int i = 0; i < list.Count; i++)
                {
                    if (r.Next(0, 99) > 50)
                    {
                        res.Add(list[i]);
                    }
                }
            }
            return res;
        }

        private void Serialize(List<News> list)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<News>));
            using (FileStream fs = new FileStream(dir + "news.json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, list);
            }
        }

        private List<News> Deserialize()
        {
            List<News> res = new List<News>();
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<News>));
            using (FileStream fs = new FileStream(dir + "news.json", FileMode.OpenOrCreate))
            {
                res = (List<News>)jsonFormatter.ReadObject(fs);
            }
            return res;
        }
    }
}