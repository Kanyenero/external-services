using Microsoft.AspNetCore.Mvc;
using Navicon.Mdm.ExternalServices.ServiceContracts;
using Navicon.Mdm.MasterData.BusinessRules.Dto;

namespace Navicon.Mdm.ExternalServices.Web.Controllers
{
    [ApiController]
    public class EntityValidationController
    {
        private readonly IEntityValidationService _entityValidationService;

        public EntityValidationController(IEntityValidationService entityValidationService)
        {
            _entityValidationService = entityValidationService;
        }

        [HttpPost]
        [Route("entityvalidation")]
        public async Task<NotificationDto> ValidateEntity(EntityDto entity)
        {
            return await _entityValidationService.ValidateEntityAsync(entity);
        }

        //[HttpGet]
        //[Route("")]
        //public async Task Test()
        //{
        //    var entityDtoJson =
        //        // LegalEntity
        //        //"{\"MessageId\":\"00000000-0000-0000-0000-000000000000\",\"EntityName\":null,\"SourceName\":null,\"DestName\":null,\"EntityData\":{\"PrimitiveEntityName\":\"LegalEntity\",\"SourceCode\":null,\"MdmCode\":\"122\",\"DestCode\":\"122\",\"Attributes\":[{\"Name\":\"LegalEntityName\",\"AttributeType\":0,\"Value\":null},{\"Name\":\"INN\",\"AttributeType\":0,\"Value\":\"7707083893\"},{\"Name\":\"KPP\",\"AttributeType\":0,\"Value\":null},{\"Name\":\"OGRN\",\"AttributeType\":0,\"Value\":null},{\"Name\":\"OKVED\",\"AttributeType\":0,\"Value\":null},{\"Name\":\"CreationDate\",\"AttributeType\":2,\"Value\":null}],\"LinkAttributes\":[{\"LinkedEntityInfoId\":25,\"LinkedEntityId\":null,\"Name\":\"OPF\",\"IsMultiLink\":false}]},\"Children\":null}"

        //        // PhysicalPerson
        //        "{\"MessageId\":\"00000000-0000-0000-0000-000000000000\",\"EntityName\":null,\"SourceName\":null,\"DestName\":null,\"EntityData\":{\"PrimitiveEntityName\":\"PhysicalPerson\",\"SourceCode\":null,\"MdmCode\":\"29\",\"DestCode\":\"29\",\"Attributes\":[{\"Name\":\"FullName\",\"AttributeType\":0,\"Value\":null},{\"Name\":\"Surname\",\"AttributeType\":0,\"Value\":\"иванов\"},{\"Name\":\"Name\",\"AttributeType\":0,\"Value\":\"иван\"},{\"Name\":\"MiddleName\",\"AttributeType\":0,\"Value\":\"ивановч\"},{\"Name\":\"Gender\",\"AttributeType\":4,\"Value\":null},{\"Name\":\"Birthday\",\"AttributeType\":0,\"Value\":null}],\"LinkAttributes\":[]},\"Children\":[{\"EntityData\":{\"PrimitiveEntityName\":\"Address\",\"SourceCode\":null,\"MdmCode\":\"61\",\"DestCode\":\"61\",\"Attributes\":[{\"Name\":\"FIASCode\",\"AttributeType\":0,\"Value\":null},{\"Name\":\"Address\",\"AttributeType\":0,\"Value\":\"московская 8 к1\"}],\"LinkAttributes\":[{\"LinkedEntityInfoId\":26,\"LinkedEntityId\":null,\"Name\":\"Country\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":27,\"LinkedEntityId\":134,\"Name\":\"City\",\"IsMultiLink\":false}]},\"Children\":null}]}"
        //        ;
        //    var entityDto = Newtonsoft.Json.JsonConvert.DeserializeObject<EntityDto>(entityDtoJson);

        //    var result = await _entityValidationService.ValidateEntityAsync(entityDto);
        //}
    }
}
