using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using data.service;
using data.entity;
using wad.Models;
using HtmlAgilityPack;

namespace wad.Controllers.API
{
    public class SchemaController : ApiController
    {
        IHtmlItemService _htmlItemService;
        IHtmlSnippetService _htmlSnippetService;
        public SchemaController(IHtmlItemService htmlItemService, IHtmlSnippetService htmlSnippetService)
        {
            _htmlItemService = htmlItemService;
            _htmlSnippetService = htmlSnippetService;        

        }

        // /api/schema/GetAllTypes
        public HttpResponseMessage GetAllTypes()
        {
            try
            {
                return Request.CreateResponse(statusCode: HttpStatusCode.OK,
                                            value: SchemaHelper.GetAllTypes().Select(p => new { word = p, score = 300 }));
            }
            catch
            {
                return new HttpResponseMessage(statusCode: HttpStatusCode.BadRequest);
            }
        }

        public HttpResponseMessage GetAllTypes(string contains)
        {
            try
            {
                /*return Request.CreateResponse(statusCode: HttpStatusCode.OK,
                                            value: SchemaHelper.GetAllTypes().Where(p => p.ToLower()
                                                .Contains(contains.ToLower())).Select(p => new { word = p, score = 300 }));*/
                return Request.CreateResponse(statusCode: HttpStatusCode.OK,
                                            value: SchemaHelper.GetAllTypesAndProperties().Where(p => p.ToLower()
                                                .Contains(contains.ToLower())).Select(p => new { word = p, score = 300 }));
            }
            catch
            {
                return new HttpResponseMessage(statusCode: HttpStatusCode.BadRequest);
            }
        }
        // /api/schema/GetPropertiesByType?type=dd
        public HttpResponseMessage GetPropertiesByType(string type)
        {
            try
            {
                return Request.CreateResponse(statusCode: HttpStatusCode.OK,
                                            value: SchemaHelper.GetAllPropsForType(type)
                                                            .Select(p => new { word = p, score = 300 }));
            }
            catch
            {
                return new HttpResponseMessage(statusCode: HttpStatusCode.BadRequest);
            }
        }

        public HttpResponseMessage GetPropertiesByType(string type, string contains)
        {
            try
            {
                var obj = SchemaHelper.GetAllPropsForType(type).Where(p => p.ToLower()
                    .Contains(contains.ToLower())).Select(p => new { word = p, score=300});
                return Request.CreateResponse(statusCode: HttpStatusCode.OK,
                                            value: obj);
            }
            catch
            {
                return new HttpResponseMessage(statusCode: HttpStatusCode.BadRequest);
            }
        }
        // /api/schema/GetTypesByPropery?prop=dd
        public HttpResponseMessage GetTypesByPropery(string prop)
        {
            try
            {
                return Request.CreateResponse(statusCode: HttpStatusCode.OK,
                                            value: SchemaHelper.Properties.Where(p => p.id == prop).Select(p => p.ranges).Select(p => new { word = p, score = 300 }));
            }
            catch
            {
                return new HttpResponseMessage(statusCode: HttpStatusCode.BadRequest);
            }
        }

        // POST api/schema
        public void Post([FromBody]HtmlReceived value)
        {
           
            //FindNode(htmldoc.DocumentNode, "data-snippetid",hhh.listDeleted, hhh.listUpdated);
        }

        // PUT api/schema/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/schema/5
        public void Delete(int id)
        {
        }
        //void FindNode(HtmlNode node, string attr, List<string> deletedList, List<string> updatedList){
        //    if (node.ChildNodes.Where(n=>deletedList.Contains(n.Attributes["data-snippetid"].Value)).Count()!=0)
        //    {
        //        foreach (var n in node.ChildNodes.Where(n => deletedList.Contains(n.Attributes["data-snippetid"].Value)))
        //        {
        //            node.c
        //        }
        //    }
        //    else
        //    {
        //        foreach (object child in node.ChildNodes)
        //        {
        //            if (child.GetType() == typeof(HtmlNode))
        //            {
        //                FindNode(node, attr, deletedList, updatedList);
        //            }
        //        }
        //    }
        //}
    }
}
