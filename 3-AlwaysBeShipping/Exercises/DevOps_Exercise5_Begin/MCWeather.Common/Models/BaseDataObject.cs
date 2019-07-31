using System;
using SQLite;

namespace MCWeather.Common.Models
{
    public abstract class BaseDataObject
    {
        private string _id;

        protected BaseDataObject()
        {
        }

        [Newtonsoft.Json.JsonProperty("Id"), PrimaryKey]
        public string Id
        {
            get { return _id ?? (_id = Guid.NewGuid().ToString()); }
            set { _id = value; }
        }
    }
}
