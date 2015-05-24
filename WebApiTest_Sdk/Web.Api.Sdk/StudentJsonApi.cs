using System;
using Db.Model;
using Newtonsoft.Json.Linq;
using Web.Api.Sdk.Core;

namespace Web.Api.Sdk
{
    public class StudentJsonApi : JsonApi
    {
        public StudentJsonApi(string baseUrl) 
            : base(baseUrl)
        {
        }

        public JObject GetAll()
        {
            return MakeCall("student/all");
        }

        public JObject GetById(int id)
        {
            return MakeCall(String.Format("student/single/{0}", id));
        }

        public JObject Add(Student entity)
        {
            return Post("student/create", entity);
        }

        public JObject Replace(int id, Student entity)
        {
            return Put(String.Format("student/update/{0}", id), entity);
        }

        public JObject Remove(int id)
        {
            return Delete(String.Format("student/remove/{0}", id), null);
        }
    }
}