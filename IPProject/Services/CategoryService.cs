using IPProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;

namespace IPProject.Services
{
    public class CategoryService
    {
        private readonly string save = "";

        private readonly string dir = "";

        private readonly DBContext context;

        public CategoryService(string path)
        {
            save = ConfigurationManager.AppSettings["Saving"];
            dir = path;
            context = new DBContext();
        }

        public void Add(Category category)
        {
            if (save.Equals("file"))
            {
                List<Category> list = Deserialize();
                Category element = list.FirstOrDefault(rec => rec.Title == category.Title);
                if (element != null)
                {
                    throw new Exception("Уже есть такая категория");
                }
                int maxId = list.Count > 0 ? list.Max(rec => rec.Id) : 0;
                list.Add(new Category
                {
                    Id = maxId + 1,
                    Description = category.Description,
                    ImageUrl = category.ImageUrl,
                    Title = category.Title
                });
                Serialize(list);
                return;
            }
            else if (save.Equals("db"))
            {
                Category element = context.Categories.FirstOrDefault(rec => rec.Title == category.Title);
                if (element != null)
                {
                    throw new Exception("Уже есть такая категория");
                }
                context.Categories.Add(new Category
                {
                    Description = category.Description,
                    ImageUrl = category.ImageUrl,
                    Title = category.Title
                });
                context.SaveChanges();
                return;
            }
            throw new Exception("Хранилище данных неопределено");
        }

        public List<Category> GetList()
        {
            List<Category> res = new List<Category>();
            if (save.Equals("file"))
            {
                res = Deserialize();
                return res;
            }
            else if (save.Equals("db"))
            {
                res = context.Categories.AsEnumerable().Select(rec => new Category
                {
                    Id = rec.Id,
                    Description = rec.Description,
                    ImageUrl = rec.ImageUrl,
                    Title = rec.Title
                }).ToList();
                return res;
            }
            throw new Exception("Хранилище данных неопределено");
        }

        public Category GetElement(int id)
        {
            if (save.Equals("file"))
            {
                List<Category> list = Deserialize();
                Category element = list.FirstOrDefault(rec => rec.Id == id);
                if (element != null)
                {
                    return new Category
                    {
                        Id = element.Id,
                        Title = element.Title,
                        Description = element.Description,
                        ImageUrl = element.ImageUrl
                    };
                }
                throw new Exception("Элемент не найден");
            }
            else if (save.Equals("db"))
            {
                Category element = context.Categories.FirstOrDefault(rec => rec.Id == id);
                if (element != null)
                {
                    return new Category
                    {
                        Id = element.Id,
                        Title = element.Title,
                        Description = element.Description,
                        ImageUrl = element.ImageUrl
                    };
                }
                throw new Exception("Элемент не найден");
            }
            throw new Exception("Хранилище данных неопределено");
        }

        public void UpdElement(Category model)
        {
            if (save.Equals("file"))
            {
                List<Category> list = Deserialize();
                Category element = list.FirstOrDefault(rec => rec.Id != model.Id && rec.Title == model.Title);
                if (element != null)
                {
                    throw new Exception("Уже есть такая категория");
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
                Serialize(list);
                return;
            }
            else if (save.Equals("db"))
            {
                Category element = context.Categories.FirstOrDefault(rec => rec.Id != model.Id && rec.Title == model.Title);
                if (element != null)
                {
                    throw new Exception("Уже есть такая категория");
                }
                element = context.Categories.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                element.Title = model.Title;
                element.ImageUrl = model.ImageUrl;
                element.Description = model.Description;
                context.SaveChanges();
                return;
            }
            throw new Exception("Хранилище данных неопределено");
        }

        public void DelElement(int id)
        {
            if (save.Equals("file"))
            {
                List<Category> list = Deserialize();
                List<News> listN = DeserializeNews();
                List<News> n = listN.AsEnumerable().Select(rec => new News
                {
                }).Where(rec => rec.CategoryId == id).ToList();
                if (n.Count > 0)
                {
                    throw new Exception("Невозможно удалить");
                }
                Category element = list.FirstOrDefault(rec => rec.Id == id);
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
                Category element = context.Categories.FirstOrDefault(rec => rec.Id == id);
                List<News> n = context.News.AsEnumerable().Select(rec => new News
                {
                }).Where(rec => rec.CategoryId == id).ToList();
                if (n.Count > 0)
                {
                    throw new Exception("Невозможно удалить");
                }
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                else
                {
                    context.Categories.Remove(element);
                    context.SaveChanges();
                }
                return;
            }
            throw new Exception("Хранилище данных неопределено");
        }

        private void Serialize(List<Category> list)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Category>));
            using (FileStream fs = new FileStream(dir + "category.json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, list);
            }
        }

        private List<Category> Deserialize()
        {
            List<Category> res = new List<Category>();
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Category>));
            using (FileStream fs = new FileStream(dir + "category.json", FileMode.OpenOrCreate))
            {
                res = (List<Category>)jsonFormatter.ReadObject(fs);
            }
            return res;
        }

        private List<News> DeserializeNews()
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