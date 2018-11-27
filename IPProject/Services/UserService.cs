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

        public UserService()
        {
            save = ConfigurationManager.AppSettings["Saving"];
            dir = ConfigurationManager.AppSettings["Dir"];
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
                list.Add(new User
                {
                    Login = user.Login,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password
                });
                Serialize(list);
            }
        }

        public List<User> GetList()
        {
            List<User> res = new List<User>();
            if (save.Equals("file"))
            {
                res = Deserialize();
            }
            return res;
        }

        public User GetElement(string login)
        {
            if (save.Equals("file"))
            {
                List<User> list = Deserialize();
                User element = list.FirstOrDefault(rec => rec.Login == login);
                if (element != null)
                {
                    return new User
                    {
                        Login = element.Login,
                        FirstName = element.FirstName,
                        LastName = element.LastName,
                        Password = element.Password
                    };
                }
            }
            throw new Exception("Элемент не найден");
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
            }
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
            }
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
            else
            {
                return null;
            }
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