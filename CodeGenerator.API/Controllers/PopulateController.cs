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
    public partial class PopulateController : ApiControllerBase
    {

        [HttpPost]
        [Route("/populate")]
        [SwaggerOperation(Description = "Trae los registros de AgenteDano paginados.")]
        public async Task<ActionResult> Populate([FromBody] PopulateCommand command)
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
