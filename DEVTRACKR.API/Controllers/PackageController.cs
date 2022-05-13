using DEVTRACKR.API.Entities;
using DEVTRACKR.API.Models;
using DEVTRACKR.API.Persistence;
using DEVTRACKR.API.Persistence.Repository;
using  Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DEVTRACKR.API.Controllers
{
    [ApiController]
    [Route("api/packages")]

    public class PackageController : ControllerBase
    {
        private readonly IPackageRepository _repository;
        private readonly ISendGridClient _client;
        public PackageController(IPackageRepository repository, ISendGridClient client)
        {
            _repository = repository;
            _client = client;
        }

        // GET api/packages
        [HttpGet]
        public IActionResult GetAll() {
            var packages = _repository.GetAll();

            return Ok(packages);
        }

        // GET api/packages/1234-5678-9101
        [HttpGet("{code}")]
        public IActionResult GetByCode(string code){
            var package = _repository.GetByCode(code);

            if (package == null){
                return NotFound();
            }

            return Ok(package);
        }

       
        /// <summary>
        ///  Cadastro de um pacote. 
        /// </summary>
        /// <remarks>
        /// {
        ///  "title": "Pacote Cartas Colecionáveis",
        ///  "weigth": 1.0,
        ///  "senderName": "Luan",
        ///  "senderEmail": "bayiho6875@akapple.com"
        /// }
        /// </remarks>
        /// <param name="model">Dados do pacote</param>
        /// <returns>Objeto recém-criado.</returns>
        /// <response code ="201">Cadastro realizado com sucesso.</response>
        /// <response code ="400">Dados estão inválidos</response>
        /// 
        
        [HttpPost]
        public async Task<IActionResult> Post(AddPackageInputModel model){
           if (model.Title.Length < 10){
               return BadRequest("Title length must be at least characters long.");
           }
           
            var package = new Package(model.Title, model.Weigth);

           _repository.Add(package);

           var message = new SendGridMessage {
               From = new EmailAddress("bayiho6875@akapple.com", "BAYIHO"),
               Subject = "Your package was dispatched.",
               PlainTextContent = $"Your package with code {package.Code} was dispatched."
           };

           message.AddTo(model.SenderEmail,model.SenderName);

            await _client.SendEmailAsync(message);

            return CreatedAtAction(
                "GetByCode", 
                new { code = package.Code },
                package);
        }

        // POST api/packages/1234-5678-9101/updates
        [HttpPost("{code}/updates")]
        public IActionResult PostUpdate(string code,AddPackageUpdateInputModel model){
             var package = _repository.GetByCode(code);

            if (package == null){
                return NotFound();
            }

            package.AddUpdate(model.Status, model.Delivered);

            _repository.Update(package);

            return NoContent(); 
        }


    }
}