using CodeGenerator.API.Controllers.Base;
using CodeGenerator.Application.Commands.General;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace CodeGenerator.API.Controllers
{
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public partial class GenerateController : ApiControllerBase
    {

        [HttpPost]
        [Route("/generate")]
        [SwaggerOperation(Description = "Genera el código de un proyecto específico.")]
        public async Task<ActionResult> Generate([FromBody] PopulateCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
