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
        private readonly string save;

        private readonly string dir;

        private readonly DBContext context;

        public NewsService(string path)
        {
            save = ConfigurationManager.AppSettings["Saving"];
            dir = path;
            context = new DBContext();
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
                    UserId = news.UserId
                });
                Serialize(list);
                return;
            }
            else if (save.Equals("db"))
            {
                News element = context.News.FirstOrDefault(rec => rec.Title == news.Title);
                if (element != null)
                {
                    throw new Exception("Уже есть такая новость");
                }
                context.News.Add(new News
                {
                    Description = news.Description,
                    ImageUrl = news.ImageUrl,
                    Title = news.Title,
                    Body = news.Body,
                    CategoryId = news.CategoryId,
                    DateCreate = DateTime.Now,
                    NumberOfViews = 0,
                    UserId = news.UserId
                });
                context.SaveChanges();
                return;
            }
            throw new Exception("Хранилище данных неопределено");
        }

        public List<News> GetList()
        {
            List<News> res = new List<News>();
            if (save.Equals("file"))
            {
                res = Deserialize();
                return res;
            }
            else if (save.Equals("db"))
            {
                res = context.News.AsEnumerable().Select(rec => new News
                {
                    Id = rec.Id,
                    Description = rec.Description,
                    ImageUrl = rec.ImageUrl,
                    Title = rec.Title,
                    Body = rec.Body,
                    CategoryId = rec.CategoryId,
                    DateCreate = rec.DateCreate,
                    NumberOfViews = rec.NumberOfViews,
                    UserId = rec.UserId
                }).ToList();
                return res;
            }
            throw new Exception("Хранилище данных неопределено");
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
                        UserId = element.UserId,
                        NumberOfViews = element.NumberOfViews,
                        DateCreate = element.DateCreate,
                        CategoryId = element.CategoryId,
                        Body = element.Body,
                        Description = element.Description,
                        ImageUrl = element.ImageUrl,
                        Title = element.Title
                    };
                }
                throw new Exception("Элемент не найден");
            }
            else if (save.Equals("db"))
            {
                News element = context.News.FirstOrDefault(rec => rec.Id == id);
                if (element != null)
                {
                    return new News
                    {
                        Id = element.Id,
                        UserId = element.UserId,
                        NumberOfViews = element.NumberOfViews,
                        DateCreate = element.DateCreate,
                        CategoryId = element.CategoryId,
                        Body = element.Body,
                        Description = element.Description,
                        ImageUrl = element.ImageUrl,
                        Title = element.Title
                    };
                }
                throw new Exception("Элемент не найден");
            }
            throw new Exception("Хранилище данных неопределено");
        }

        public List<News> GetListByCategory(int categoryId)
        {
            List<News> res = new List<News>();
            if (save.Equals("file"))
            {
                List<News> list = Deserialize();
                res = list.AsEnumerable().Select(rec => new News
                {
                    Id = rec.Id,
                    Title = rec.Title,
                    ImageUrl = rec.ImageUrl,
                    Description = rec.Description,
                    Body = rec.Body,
                    CategoryId = rec.CategoryId,
                    DateCreate = rec.DateCreate,
                    NumberOfViews = rec.NumberOfViews,
                    UserId = rec.UserId
                }).Where(rec => rec.CategoryId == categoryId).ToList();
                return res;
            }
            else if (save.Equals("db"))
            {
                res = context.News.AsEnumerable().Select(rec => new News
                {
                    Id = rec.Id,
                    Description = rec.Description,
                    ImageUrl = rec.ImageUrl,
                    Title = rec.Title,
                    Body = rec.Body,
                    CategoryId = rec.CategoryId,
                    DateCreate = rec.DateCreate,
                    NumberOfViews = rec.NumberOfViews,
                    UserId = rec.UserId
                }).Where(rec => rec.CategoryId == categoryId).ToList();
                return res;
            }
            throw new Exception("Хранилище данных неопределено");
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
                if (model.ImageUrl != null)
                {
                    element.ImageUrl = model.ImageUrl;
                }
                element.Description = model.Description;
                element.Body = model.Body;
                element.CategoryId = model.CategoryId;
                element.DateCreate = model.DateCreate;
                element.NumberOfViews = model.NumberOfViews;
                element.UserId = model.UserId;
                Serialize(list);
                return;
            }
            else if (save.Equals("db"))
            {
                News element = context.News.FirstOrDefault(rec => rec.Id != model.Id && rec.Title == model.Title);
                if (element != null)
                {
                    throw new Exception("Уже есть такая новость");
                }
                element = context.News.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                element.Title = model.Title;
                if (model.ImageUrl != null)
                {
                    element.ImageUrl = model.ImageUrl;
                }
                element.Description = model.Description;
                element.Body = model.Body;
                element.CategoryId = model.CategoryId;
                element.DateCreate = model.DateCreate;
                element.NumberOfViews = model.NumberOfViews;
                element.UserId = model.UserId;
                context.SaveChanges();
                return;
            }
            throw new Exception("Хранилище данных неопределено");
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
                return;
            }
            else if (save.Equals("db"))
            {
                News element = context.News.FirstOrDefault(rec => rec.Id == id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                else
                {
                    context.News.Remove(element);
                    context.SaveChanges();
                }
                return;
            }
            throw new Exception("Хранилище данных неопределено");
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
                return res;
            }
            else if (save.Equals("db"))
            {
                List<News> list = context.News.AsEnumerable().Select(rec => new News
                {
                    Id = rec.Id,
                    Description = rec.Description,
                    ImageUrl = rec.ImageUrl,
                    Title = rec.Title,
                    Body = rec.Body,
                    CategoryId = rec.CategoryId,
                    DateCreate = rec.DateCreate,
                    NumberOfViews = rec.NumberOfViews,
                    UserId = rec.UserId
                }).ToList();
                Random r = new Random();
                for (int i = 0; i < list.Count; i++)
                {
                    if (r.Next(0, 99) > 50)
                    {
                        res.Add(list[i]);
                    }
                }
                return res;
            }
            throw new Exception("Хранилище данных неопределено");
        }

        public void IncreaseViews(int id)
        {
            if (save.Equals("file"))
            {
                List<News> list = Deserialize();
                News element = list.FirstOrDefault(rec => rec.Id == id);
                if (element != null)
                {
                    element.NumberOfViews++;
                    Serialize(list);
                    return;
                }
                throw new Exception("Элемент не найден");
            }
            else if (save.Equals("db"))
            {
                News element = context.News.FirstOrDefault(rec => rec.Id == id);
                if (element != null)
                {
                    element.NumberOfViews++;
                    return;
                }
                throw new Exception("Элемент не найден");
            }
            throw new Exception("Хранилище данных неопределено");
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