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

        public CategoryService()
        {
            save = ConfigurationManager.AppSettings["Saving"];
            dir = ConfigurationManager.AppSettings["Dir"];
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
            }
        }

        public List<Category> GetList()
        {
            List<Category> res = new List<Category>();
            if (save.Equals("file"))
            {
                res = Deserialize();
            }
            return res;
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
            }
            throw new Exception("Элемент не найден");
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
                element.ImageUrl = model.ImageUrl;
                element.Description = model.Description;
                Serialize(list);
            }
        }

        public void DelElement(int id)
        {
            if (save.Equals("file"))
            {
                List<Category> list = Deserialize();
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
            }
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
    }
}