﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Http;
using SerializationFileStructure.Models;
using SerializationFileStructure.Services;

namespace SerializationFileStructure.Controllers
{
    public class SerializeController : ApiController
    {
        // POST api/values
        public HttpResponseMessage Post([FromBody]SerializeDataModel SerializeData)
        {
            if (!Directory.Exists(SerializeData.serializePath))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Incorect serialize path");
            } else if (!Directory.Exists(SerializeData.filePath))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Incorect file path");
            }
            //string str = @"C:\Users\Andriy\Desktop\TEST";
            Filelist dirs = FileHelper.GetFilesRecursive(@SerializeData.serializePath);

            Filelist data = dirs; // replaces List<string>
                                  // fill

            //@"C:\Users\Andriy\Desktop\XXX\data.dat"

            var folderPath = SerializeData.serializePath;

            int pos = folderPath.LastIndexOf("\\") + 1;

            var fileName = folderPath.Substring(pos, folderPath.Length - pos);

            using (var stream = File.Create(@SerializeData.filePath + "\\" + fileName + ".dat"))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
            }

            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}