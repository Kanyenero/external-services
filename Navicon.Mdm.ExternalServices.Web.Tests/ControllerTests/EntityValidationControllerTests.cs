using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Navicon.Mdm.MasterData.BusinessRules.Dto;
using Newtonsoft.Json;

namespace Navicon.Mdm.ExternalServices.Web.Tests.ControllerTests
{
    public class EntityValidationControllerTests
    {
        private readonly EntityDto _physicalPersonEntity;
        private readonly EntityDto _legalEntityEntity;
        //private readonly string _entityDtoJson;

        public EntityValidationControllerTests()
        {
            _physicalPersonEntity = new EntityDto
            {
                EntityName = "PhysicalPerson",
                EntityData = new PrimitiveEntityDto
                {
                    PrimitiveEntityName = "PhysicalPerson",
                    MdmCode = "1",
                    Attributes = new List<AttributeDto>
                    {
                        new AttributeDto() { Name = "Surname", AttributeType = AttributeTypeDto.String, Value = "иванов" },
                        new AttributeDto() { Name = "Name", AttributeType = AttributeTypeDto.String, Value = "ивн" },
                        new AttributeDto() { Name = "MiddleName", AttributeType = AttributeTypeDto.String, Value = "иванович" },
                        new AttributeDto() { Name = "Gender", AttributeType = AttributeTypeDto.Enumeration, Value = null },
                    }
                },
                Children = new List<EntityChildDto>
                {
                    new EntityChildDto
                    {
                        EntityData = new PrimitiveEntityDto
                        {
                            PrimitiveEntityName = "Contacts",
                            MdmCode = "2",
                            Attributes = new List<AttributeDto>
                            {
                                //new AttributeDto { Name = "Address", AttributeType = AttributeTypeDto.String, Value = "мск сухонская 11 89" },
                                //new AttributeDto { Name = "FIASCode", AttributeType = AttributeTypeDto.String, Value = null },
                                //new AttributeDto { Name = "Country_DaData", AttributeType = AttributeTypeDto.String, Value = null },
                                //new AttributeDto { Name = "City_DaData", AttributeType = AttributeTypeDto.String, Value = null },
                                new AttributeDto { Name = "ContactInfo", AttributeType = AttributeTypeDto.String, Value = "ivanovivan@yandex/ru" },
                            },
                            LinkAttributes = new List<LinkAttributeDto>
                            {
                                new LinkAttributeDto { Name = "ContactInfoType", LinkedEntityInfoId = 28, LinkedEntityId = 10236 }
                            }
                        }
                    }
                }
            };

            _legalEntityEntity = new EntityDto
            {
                EntityName = "LegalEntity",
                EntityData = new PrimitiveEntityDto
                {
                    PrimitiveEntityName = "LegalEntity",
                    MdmCode = "1",
                    Attributes = new List<AttributeDto>
                    {
                        new AttributeDto { Name = "LegalEntityName", AttributeType = AttributeTypeDto.String, Value = null },
                        new AttributeDto { Name = "CreationDate", AttributeType = AttributeTypeDto.String, Value = null },
                        new AttributeDto { Name = "INN", AttributeType = AttributeTypeDto.String, Value = "7707083893" },
                        new AttributeDto { Name = "KPP", AttributeType = AttributeTypeDto.String, Value = "540602001" },
                        new AttributeDto { Name = "OGRN", AttributeType = AttributeTypeDto.String, Value = null },
                        new AttributeDto { Name = "OKVED", AttributeType = AttributeTypeDto.String, Value = null },
                        new AttributeDto { Name = "OPF_DaData", AttributeType = AttributeTypeDto.String, Value = null }
                    }
                },
                Children = new List<EntityChildDto>
                {
                    new EntityChildDto
                    {
                        EntityData = new PrimitiveEntityDto
                        {
                            PrimitiveEntityName = "Address",
                            MdmCode = "2",
                            Attributes = new List<AttributeDto>
                            {
                                new AttributeDto { Name = "Address", AttributeType = AttributeTypeDto.String, Value = null },
                                new AttributeDto { Name = "FIASCode", AttributeType = AttributeTypeDto.String, Value = null },
                                new AttributeDto { Name = "Country_DaData", AttributeType = AttributeTypeDto.String, Value = null },
                                new AttributeDto { Name = "City_DaData", AttributeType = AttributeTypeDto.String, Value = null },
                            }
                        }
                    }
                }
            };
        }

        [Fact]
        public async void Validate_PhysicalPerson_And_Ensure_Result_Is_Not_Null()
        {
            var webFactory = new WebApplicationFactory<Program>();
            var client = webFactory.CreateClient();

            var requestJsonContent = JsonConvert.SerializeObject(_physicalPersonEntity);
            var requestHttpContent = new StringContent(requestJsonContent, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("http://localhost:5237/entityvalidation", requestHttpContent);

            NotificationDto notification = null;

            if (response.IsSuccessStatusCode)
            {
                var responseHttpContent = response.Content;
                var responseJsonContent = await responseHttpContent.ReadAsStringAsync();

                notification = JsonConvert.DeserializeObject<NotificationDto>(responseJsonContent);
            }

            Assert.NotNull(notification);
        }

        [Fact]
        public async void Validate_LegalEntity_And_Ensure_Result_Is_Not_Null()
        {
            var webFactory = new WebApplicationFactory<Program>();
            var client = webFactory.CreateClient();

            var requestJsonContent = JsonConvert.SerializeObject(_legalEntityEntity);
            var requestHttpContent = new StringContent(requestJsonContent, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("http://localhost:5237/entityvalidation", requestHttpContent);

            NotificationDto notification = null;

            if (response.IsSuccessStatusCode)
            {
                var responseHttpContent = response.Content;
                var responseJsonContent = await responseHttpContent.ReadAsStringAsync();

                notification = JsonConvert.DeserializeObject<NotificationDto>(responseJsonContent);
            }

            Assert.NotNull(notification);
        }
    }
}
