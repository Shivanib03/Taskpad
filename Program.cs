using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaskPad
{
    class Program
    {
        static void Main(string[] args)
        {

            string fileName = "C:\\Users\\Shivani.Barahate\\source\\repos\\TaskPad\\tsk.txt";
            var numberOfTasks = 0;

            List<Task> taskList = new List<Task>();

            try
            {
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                StreamReader read = new StreamReader(stream);
                stream.Seek(0, SeekOrigin.Begin);
                string taskobj;
                while ((taskobj = read.ReadLine()) != null)
                {

                    if (taskobj == System.Environment.NewLine) continue;
                    Task t = JsonConvert.DeserializeObject<Task>(taskobj);
                    taskList.Add(t);
                    t.id = numberOfTasks;
                    numberOfTasks++;
                }
                stream.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }


            static int compare(Task a, Task b)
            {
                return b.priority - a.priority;
            }

            string option;
            Console.WriteLine("\n!----------------------------Taskpad-------------------------------!\n");
            do
            {
              Excptn: Console.WriteLine("\na.Create a new task\nb.view by id\nc.view all tasks\nd.edit\ne.delete\nf.Exit\nEnter your option: \n");

                option = Console.ReadLine();

                switch (option)
                {
                    case "a":
                        try
                        {
                            ++numberOfTasks;
                            var t = new Task(numberOfTasks);

                            Console.WriteLine("\nEnter the task title: ");
                            t.title = Console.ReadLine();

                            Console.WriteLine("\nEnter the message: ");
                            t.msg = Console.ReadLine();

                        Priority: Console.WriteLine("\nEnter priority 1/2/3 for the task");
                            int priority = Convert.ToInt32(Console.ReadLine());
                            if (priority < 1 || priority > 3)
                            {
                                Console.WriteLine("\n Invalid Priority!!! Please enter again!\n");
                                goto Priority;
                            }
                            t.priority = priority;

                        d: Console.WriteLine("\nEnter the date (format dd/mm/yyyy) for task to be completed:");
                            var date = Console.ReadLine();
                            var arr = date.Split("/");

                            int day = int.Parse(arr[0]);
                            int month = int.Parse(arr[1]);
                            int year = int.Parse(arr[2]);

                            var Date = new DateTime(year, month, day);
                            if (Date < DateTime.Now)
                            { Console.WriteLine("\nInvalid date!!!"); goto d; }
                            else if (day < 1 || day > 31 || month < 1 || month > 12)
                            { Console.WriteLine("\nInvalid date!!!"); goto d; }
                            else
                                t.completeBy = Date;

                            t.state = "pending";

                            taskList.Add(t);
                            //saveTask(t,fileName);
                            Console.WriteLine("\nAdded Successfully!!!");
                            break;
                        }
                        catch(Exception e)
                        { Console.WriteLine(e.Message);goto Excptn; }
                    case "b":
                        try
                        {
                        ID: Console.WriteLine("\nEnter id: ");
                            var id = int.Parse(Console.ReadLine());
                            if (taskList.Any(t => t.id == id))
                            {
                                foreach (var task in taskList)
                                {
                                    if (task.id == id)
                                    {
                                        Console.WriteLine("\n{0,-5}{1,-15}{2,-30}{3,-10}{4,-10}{5,-8}", "ID", "Title", "Message", "priority", "state", "Target date");
                                        Console.WriteLine("\n{0,-5}{1,-15}{2,-30}{3,-10}{4,-10}{5,-8}", task.id, task.title, task.msg, task.priority, task.state, task.completeBy.Date);
                                    }
                                }
                            }

                            else
                            {
                                Console.WriteLine("\nInvalid id");
                                goto ID;
                            }
                            break;
                        }
                        catch (Exception e)
                        { Console.WriteLine(e.Message); goto Excptn; }
                    case "c":
                        try
                        {
                            taskList.Sort(compare);
                            Console.WriteLine("\n---------------------------------------------------------------------------\n");
                            Console.WriteLine("\n{0,-5}{1,-15}{2,-30}{3,-10}{4,-10}{5,-8}", "ID", "Title", "Message", "Priority", "state", "Target date");

                            foreach (var task in taskList)
                            {
                                Console.WriteLine("\n{0,-5}{1,-15}{2,-30}{3,-10}{4,-10}{5,-8}", task.id, task.title, task.msg, task.priority, task.state, task.completeBy.Date);

                            }
                            Console.WriteLine("\n---------------------------------------------------------------------------\n");

                            break;
                        }
                        catch (Exception e)
                        { Console.WriteLine(e.Message); goto Excptn; }

                    case "d":
                        try
                        {
                        Valid: Console.WriteLine("\nPlease Enter the ID of Task : ");
                            int tid = int.Parse(Console.ReadLine());
                            int index;
                            if (taskList.Any(t => t.id == tid))
                                index = taskList.FindIndex((x => x.id == tid));
                            else
                            {
                                Console.WriteLine("\nPlease enter a valid ID !");
                                goto Valid;
                            }


                            string Title = taskList[index].title;
                            string Message = taskList[index].msg;
                            string state = taskList[index].state;
                            int Priority = taskList[index].priority;
                            DateTime Stamp = taskList[index].completeBy;
                            string opt;
                            var loop = true;
                            while (loop)
                            {
                                Console.WriteLine("\nEnter the property you want to edit.\n a. Title\n b. Message" +
                                    "\n c. State\n d. Priority \n e. Exit\n");
                                opt = Console.ReadLine();
                                switch (opt)
                                {
                                    case "a":
                                        Console.WriteLine("\nPlease enter the Title: ");
                                        Title = Console.ReadLine();
                                        break;
                                    case "b":
                                        Console.WriteLine("\nPlease enter the Message: ");
                                        Message = Console.ReadLine();
                                        break;
                                    case "c":
                                    st:
                                        Console.WriteLine("\nPlease enter the state - complete/pending/incomplete: ");
                                        var st = Console.ReadLine();
                                        if (st == "complete" || st == "pending" || st == "incomplete")
                                            state = st;
                                        else
                                        {
                                            Console.WriteLine("\nEnter valid state");
                                            goto st;
                                        }
                                        break;
                                    case "d":
                                        Console.WriteLine("\nPlease enter the Priority: ");
                                        Priority = Convert.ToInt32(Console.ReadLine());
                                        break;
                                    case "e":
                                        loop = false;
                                        break;
                                }
                            }
                            taskList.RemoveAt(index);
                            taskList.Insert(index, new Task(index, Title, Message, state, Priority, Stamp));
                            Console.WriteLine("\nUpdated Successfully!");
                            break;
                        }
                        catch (Exception e)
                        { Console.WriteLine(e.Message); goto Excptn; }

                    case "e":
                        try
                        {
                        Validate: Console.WriteLine("\nPlease Enter the ID of Task : ");
                            int taskid = int.Parse(Console.ReadLine());
                            int i;
                            if (taskList.Any(t => t.id == taskid))
                                i = taskList.FindIndex((x => x.id == taskid));
                            else
                            {
                                Console.WriteLine("\n Please enter a valid ID !");
                                goto Validate;
                            }
                            taskList.RemoveAt(i);
                            Console.WriteLine("\nDeleted successfully!!!");
                            break;
                        }
                        catch (Exception e)
                        { Console.WriteLine(e.Message); goto Excptn; }
                }
            } while (option != "f");

            Console.WriteLine("--------------------------Thank You!-------------------------------");
            saveTask(taskList,fileName);
        }

        public static void saveTask(List<Task> t,string fileName)
        {
            try
            {
                FileStream stream = new FileStream(fileName, FileMode.Open,FileAccess.Write);
                StreamWriter wrt = new StreamWriter(stream);
                stream.Seek(0, SeekOrigin.Begin);
                foreach (var task in t)
                {
                    var newTask = JsonConvert.SerializeObject(task);
                    byte[] textdata = Encoding.UTF8.GetBytes(newTask);
                    stream.Write(textdata, 0, textdata.Length);
                    byte[] newline = Encoding.UTF8.GetBytes(System.Environment.NewLine);
                    stream.Write(newline, 0, newline.Length);
                    
                }
                stream.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
