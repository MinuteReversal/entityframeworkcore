using System;
using entityframeworkcore.Models;
using entityframeworkcore.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace entityframeworkcore
{
    class Program
    {
        static void Main(string[] args)
        {
            var commands = new[]{
                "get all students",
                "read student by id",
                "create student",
                "update student",
                "delete sutdent by id",
                "exit"
            };

            while (true)
            {
                var i = 0;
                Console.WriteLine(commands.Aggregate("", (workingSentence, next) => $"{workingSentence}\r\n{i++}.{next}"));
                Console.WriteLine("please input a number.");
                var line = Console.ReadLine();


                if (getCommandIndex(commands, "get all students") == line)
                {
                    Console.WriteLine(getModelJson(Get()).Replace("[", "[\r\n").Replace("]", "\r\n]").Replace("},{", "},\r\n{"));
                }
                else if (getCommandIndex(commands, "read student by id") == line)
                {
                    Console.WriteLine($"input a ID");
                    var id = Console.ReadLine();
                    Console.WriteLine(getModelJson(Read(int.Parse(id))));
                }
                else if (getCommandIndex(commands, "create student") == line)
                {
                    Console.WriteLine($"input json like this:\r\n{getModelJson(new Student())}");
                    var sModel = Console.ReadLine();
                    var m = JsonToModel(sModel);
                    m.ID = 0;
                    Create(m);
                    Console.WriteLine("Created");
                }
                else if (getCommandIndex(commands, "update student") == line)
                {
                    Console.WriteLine($"input json like this:\r\n{getModelJson(new Student())}");
                    var sModel = Console.ReadLine();
                    Update(JsonToModel(sModel));
                    Console.WriteLine("Update");
                }
                else if (getCommandIndex(commands, "delete sutdent by id") == line)
                {
                    Console.WriteLine($"input a ID");
                    var id = Console.ReadLine();
                    Delete(int.Parse(id));
                    Console.WriteLine("Deleted");
                }
                else if (getCommandIndex(commands, "exit") == line)
                {
                    break;
                }
            }
        }

        static string getCommandIndex(string[] commands, string commandText)
        {
            return Array.FindIndex(commands, s => s == commandText).ToString();
        }
        static string getModelJson(object model)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }

        static Student JsonToModel(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Student>(json);
        }

        static DbContextOptions<SchoolContext> getOptions()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SchoolContext>();
            optionsBuilder.UseSqlServer(@"Server=(local);Database=EFCore;Trusted_Connection=True;MultipleActiveResultSets=true");
            return optionsBuilder.Options;
        }

        static IList<Student> Get()
        {
            using (var ctx = new SchoolContext())
            {
                return ctx.Students.ToList();
            }
        }

        static Student Read(int ID)
        {
            using (var ctx = new SchoolContext())
            {
                return ctx.Students.Find(ID);
            }
        }

        static Student Create(Student student)
        {
            using (var ctx = new SchoolContext())
            {
                ctx.Add(student);
                ctx.SaveChanges();
                return student;
            }
        }

        static Student Update(Student student)
        {
            //https://stackoverflow.com/questions/15336248/entity-framework-5-updating-a-record
            using (var ctx = new SchoolContext())
            {
                ctx.Students.Attach(student);
                ctx.Entry(student).State = EntityState.Modified;
                ctx.SaveChanges();
                return student;
            }
        }

        static Student Delete(int ID)
        {
            using (var ctx = new SchoolContext())
            {
                var s = ctx.Students.Find(ID);
                ctx.Students.Remove(s);
                ctx.SaveChanges();
                return s;
            }
        }
    }
}
