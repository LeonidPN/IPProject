using IPProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;

namespace IPProject.Services
{
    public class UserService
    {
        private readonly string save = "";

        private readonly string dir = "";

        private readonly DBContext context;

        public UserService()
        {
            save = ConfigurationManager.AppSettings["Saving"];
            dir = ConfigurationManager.AppSettings["Dir"];
            context = new DBContext();
        }

        public void Add(User user)
        {
            if (save.Equals("file"))
            {
                List<User> list = Deserialize();
                User element = list.FirstOrDefault(rec => rec.Login == user.Login);
                if (element != null)
                {
                    throw new Exception("Уже есть такой пользователь");
                }
                int maxId = list.Count > 0 ? list.Max(rec => rec.Id) : 0;
                list.Add(new User
                {
                    Id = maxId + 1,
                    Login = user.Login,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password
                });
                Serialize(list);
                return;
            }
            else if (save.Equals("db"))
            {
                User element = context.User.FirstOrDefault(rec => rec.Login == user.Login);
                if (element != null)
                {
                    throw new Exception("Уже есть такой пользователь");
                }
                context.User.Add(new User
                {
                    Login = user.Login,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password
                });
                context.SaveChanges();
                return;
            }
            throw new Exception("Хранилище данных неопределено");
        }

        public List<User> GetList()
        {
            List<User> res = new List<User>();
            if (save.Equals("file"))
            {
                res = Deserialize();
                return res;
            }
            else if (save.Equals("db"))
            {
                res = context.User.Select(rec => new User
                {
                    Id = rec.Id,
                    Login = rec.Login,
                    FirstName = rec.FirstName,
                    LastName = rec.LastName,
                    Password = rec.Password
                }).ToList();
                return res;
            }
            throw new Exception("Хранилище данных неопределено");
        }

        public User GetElement(int id)
        {
            if (save.Equals("file"))
            {
                List<User> list = Deserialize();
                User element = list.FirstOrDefault(rec => rec.Id == id);
                if (element != null)
                {
                    return new User
                    {
                        Id = element.Id,
                        Login = element.Login,
                        FirstName = element.FirstName,
                        LastName = element.LastName,
                        Password = element.Password
                    };
                }
                throw new Exception("Элемент не найден");
            }
            else if (save.Equals("db"))
            {
                User element = context.User.FirstOrDefault(rec => rec.Id == id);
                if (element != null)
                {
                    return new User
                    {
                        Id = element.Id,
                        Login = element.Login,
                        FirstName = element.FirstName,
                        LastName = element.LastName,
                        Password = element.Password
                    };
                }
                throw new Exception("Элемент не найден");
            }
            throw new Exception("Хранилище данных неопределено");
        }

        public void UpdElement(User model)
        {
            if (save.Equals("file"))
            {
                List<User> list = Deserialize();
                User element = list.FirstOrDefault(rec => rec.Login == model.Login);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                element.FirstName = model.FirstName;
                element.LastName = model.LastName;
                element.Password = model.Password;
                Serialize(list);
                return;
            }
            else if (save.Equals("db"))
            {
                User element = context.User.FirstOrDefault(rec => rec.Login == model.Login);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                element.FirstName = model.FirstName;
                element.LastName = model.LastName;
                element.Password = model.Password;
                context.SaveChanges();
                return;
            }
            throw new Exception("Хранилище данных неопределено");
        }

        public void DelElement(string login)
        {
            if (save.Equals("file"))
            {
                List<User> list = Deserialize();
                User element = list.FirstOrDefault(rec => rec.Login == login);
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
                User element = context.User.FirstOrDefault(rec => rec.Login == login);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                else
                {
                    context.User.Remove(element);
                    context.SaveChanges();
                }
                return;
            }
            throw new Exception("Хранилище данных неопределено");
        }

        public User Authorization(string login, string password)
        {
            if (save.Equals("file"))
            {
                List<User> list = Deserialize();
                User element = list.FirstOrDefault(rec => rec.Login == login && rec.Password == password);
                if (element == null)
                {
                    throw new Exception("Логин или пароль неверный");
                }
                return new User
                {
                    Login = element.Login,
                    FirstName = element.FirstName,
                    LastName = element.LastName,
                    Password = element.Password
                };
            }
            else if (save.Equals("db"))
            {
                User element = context.User.FirstOrDefault(rec => rec.Login == login && rec.Password == password);
                if (element == null)
                {
                    throw new Exception("Логин или пароль неверный");
                }
                return new User
                {
                    Login = element.Login,
                    FirstName = element.FirstName,
                    LastName = element.LastName,
                    Password = element.Password
                };
            }
            throw new Exception("Хранилище данных неопределено");
        }

        private void Serialize(List<User> list)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<User>));
            using (FileStream fs = new FileStream(dir + "user.json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, list);
            }
        }

        private List<User> Deserialize()
        {
            List<User> res = new List<User>();
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<User>));
            using (FileStream fs = new FileStream(dir + "user.json", FileMode.OpenOrCreate))
            {
                res = (List<User>)jsonFormatter.ReadObject(fs);
            }
            return res;
        }
    }
}