using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TaskPad
{
    [Serializable]
    class Task
    {
        [JsonProperty(PropertyName = "Id")]
        public int id { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string title { get; set; }

        [JsonProperty(PropertyName = "Message")]
        public string msg { get; set; }
        
        [JsonProperty(PropertyName ="Priority")]
        public int priority { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string state { get; set; }

        [JsonProperty(PropertyName = "Complete by")]
        public DateTime completeBy { get; set; }

        public Task() { }
        
        public Task(int n)
        {
            this.id = n;
        }
      public Task(int ID,string Title,string Message,string state, int priority, DateTime complete )
        {
            this.id = ID;
            this.title = Title;
            this.msg = Message;
            this.priority = priority;
            this.state = state;
            this.completeBy = complete;
        }
    }
}
