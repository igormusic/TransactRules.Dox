using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TransactRules.Dox.Data;
using dto = TransactRules.Dox.DataContracts.Configuration;
using domain = TransactRules.Dox.Configuration;
using TransactRules.Dox.Mapping;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using TransactRules.Dox.DataContracts;

namespace TransactRules.Dox.Api.Controllers
{
    public class DocumentTemplatesController : ApiController
    {

        private IDocumentTemplateRepository _repository;

        public IDocumentTemplateRepository Repository { 
            get { 
                if (_repository == null)
                    _repository = new DocumentTemplateRepository();

                return _repository;
            } 
            set {
                _repository = value;
            } 
        }

        // GET: api/DocumentTypes
        public IEnumerable<dto.DocumentTemplate> Get()
        {
            return Repository.Items().ToDTO();
        }

        // GET: api/DocumentTypes/5
        public dto.DocumentTemplate Get(int id)
        {
            return Repository.GetById(id).ToDTO();
        }

        // POST: api/DocumentTypes

        public async Task<HttpResponseMessage> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateResponse<Error>(
                    HttpStatusCode.UnsupportedMediaType, 
                    new Error { 
                        code = (int) HttpStatusCode.UnsupportedMediaType, 
                        message ="Expected Mime multipart content"}  );
            }

            try{
                var root = HttpContext.Current.Server.MapPath("~/App_Data/Temp/FileUploads");
                Directory.CreateDirectory(root);
                var provider = new MultipartFormDataStreamProvider(root);
                var result = await Request.Content.ReadAsMultipartAsync(provider);
            
                if (result.FormData["templateName"] == null)
                {
                     return Request.CreateResponse<Error>(
                        HttpStatusCode.BadRequest, 
                        new Error { 
                            code = (int) HttpStatusCode.BadRequest, 
                            message ="Required parameter: templateName"}  );
                }


                var model = new domain.DocumentTemplate { TemplateName = result.FormData["templateName"] };

                //Repository.Create(model);

                //get the files
                foreach (var file in result.FileData)
                {                
                    //TODO: Do something with each uploaded file

                    var fileName = file.LocalFileName;
                }

                return Request.CreateResponse<dto.DocumentTemplate>(HttpStatusCode.OK, model.ToDTO());

            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        // PUT: api/DocumentTypes/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE: api/DocumentTypes/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var item = Repository.GetById(id);

                if (item == null)
                {
                    return NotFound();
                }

                Repository.Delete(item);

                return Ok();
            }
            catch { 
            
            }
            ;

            return InternalServerError();
        }
    }
}
