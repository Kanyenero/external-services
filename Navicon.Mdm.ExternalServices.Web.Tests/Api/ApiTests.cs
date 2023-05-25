using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Navicon.Mdm.MasterData.BusinessRules.Dto;
using Newtonsoft.Json;

namespace Navicon.Mdm.ExternalServices.Web.Tests.Api;

public class ApiTests
{
    private readonly EntityDto _entityDto;
    private readonly string _entityDtoJson;

    public ApiTests()
    {
        _entityDto = new EntityDto
        {
            EntityData = new PrimitiveEntityDto
            {
                PrimitiveEntityName = "Customer",
                MdmCode = "1",
                Attributes = new List<AttributeDto>
                {
                    new AttributeDto { Name = "Surname", AttributeType = AttributeTypeDto.String, Value = "иванов" },
                    new AttributeDto { Name = "Name", AttributeType = AttributeTypeDto.String, Value = "ивн" },
                    new AttributeDto { Name = "MiddleName", AttributeType = AttributeTypeDto.String, Value = "ивановч" },
                    new AttributeDto { Name = "CustomerGender", AttributeType = AttributeTypeDto.Enumeration, Value = null },
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
                            new AttributeDto { Name = "ContactInfo", AttributeType = AttributeTypeDto.String, Value = "ivanovivan@yandex/ru" },
                        },
                        LinkAttributes = new List<LinkAttributeDto>
                        {
                            new LinkAttributeDto { Name = "ContactInfoType", LinkedEntityInfoId = 28, LinkedEntityId = 10236 }
                        }
                    }
                },
                new EntityChildDto
                {
                    EntityData = new PrimitiveEntityDto
                    {
                        PrimitiveEntityName = "Contacts",
                        MdmCode = "3",
                        Attributes = new List<AttributeDto>
                        {
                            new AttributeDto { Name = "ContactInfo", AttributeType = AttributeTypeDto.String, Value = "8(920)-966-22-12" },
                        },
                        LinkAttributes = new List<LinkAttributeDto>
                        {
                            new LinkAttributeDto { Name = "ContactInfoType", LinkedEntityInfoId = 28, LinkedEntityId = 10233 }
                        }
                    }
                }
            }
        };

        _entityDtoJson =
            "{\"MessageId\":\"00000000-0000-0000-0000-000000000000\",\"EntityName\":null,\"SourceName\":null,\"DestName\":null,\"EntityData\":{\"PrimitiveEntityName\":\"Customer\",\"SourceCode\":null,\"MdmCode\":\"228\",\"DestCode\":\"228\",\"Attributes\":[{\"Name\":\"Surname\",\"AttributeType\":0,\"Value\":\"Иванов\"},{\"Name\":\"Name\",\"AttributeType\":0,\"Value\":\"Ивн\"},{\"Name\":\"MiddleName\",\"AttributeType\":0,\"Value\":\"Иванович\"},{\"Name\":\"Birthday\",\"AttributeType\":2,\"Value\":\"01.01.2000 0:00:00\"},{\"Name\":\"MaritalStatus\",\"AttributeType\":0,\"Value\":null},{\"Name\":\"MrMs\",\"AttributeType\":4,\"Value\":\"Уважаемый\"},{\"Name\":\"MarketingAgree\",\"AttributeType\":3,\"Value\":\"True\"},{\"Name\":\"PersonalDataAgree\",\"AttributeType\":3,\"Value\":\"True\"},{\"Name\":\"ResearchAgree\",\"AttributeType\":3,\"Value\":null},{\"Name\":\"SuppressAllCalls\",\"AttributeType\":3,\"Value\":null},{\"Name\":\"SuppressAllEmails\",\"AttributeType\":3,\"Value\":null},{\"Name\":\"SuppressAllMessages\",\"AttributeType\":3,\"Value\":\"False\"},{\"Name\":\"EmailBounceBlock\",\"AttributeType\":3,\"Value\":null},{\"Name\":\"AddressBounceBlock\",\"AttributeType\":3,\"Value\":null},{\"Name\":\"CustomerGender\",\"AttributeType\":4,\"Value\":\"Мужской\"},{\"Name\":\"phonemobile\",\"AttributeType\":0,\"Value\":\"+7 910 494-22-15\"},{\"Name\":\"phonework\",\"AttributeType\":0,\"Value\":\"+7 920 600-60-30\"},{\"Name\":\"phonehome\",\"AttributeType\":0,\"Value\":\"+7 4912 20-38-17\"},{\"Name\":\"mail\",\"AttributeType\":0,\"Value\":\"ivanovivan@yandex.ru\"}],\"LinkAttributes\":[{\"LinkedEntityInfoId\":4,\"LinkedEntityId\":2,\"Name\":\"CustomerType\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":12,\"LinkedEntityId\":315,\"Name\":\"PreferredDealer\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":12,\"LinkedEntityId\":243,\"Name\":\"SalesDealer\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":12,\"LinkedEntityId\":318,\"Name\":\"ServiceDealer\",\"IsMultiLink\":false}]},\"Children\":[{\"EntityData\":{\"PrimitiveEntityName\":\"Address\",\"SourceCode\":null,\"MdmCode\":\"12\",\"DestCode\":\"12\",\"Attributes\":[{\"Name\":\"FIASCode\",\"AttributeType\":0,\"Value\":\"9a0580e1-1ef1-4102-abb4-85d18fd2c5c4\"},{\"Name\":\"Index\",\"AttributeType\":0,\"Value\":\"119311\"},{\"Name\":\"AddressString\",\"AttributeType\":0,\"Value\":\"г Москва, ул Строителей, д 25\"},{\"Name\":\"Address1\",\"AttributeType\":0,\"Value\":\"ул Строителей\"},{\"Name\":\"Address2\",\"AttributeType\":0,\"Value\":\"д 25,  \"},{\"Name\":\"Address3\",\"AttributeType\":0,\"Value\":null}],\"LinkAttributes\":[{\"LinkedEntityInfoId\":3,\"LinkedEntityId\":71,\"Name\":\"City\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":8,\"LinkedEntityId\":null,\"Name\":\"AddressType\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":1,\"LinkedEntityId\":null,\"Name\":\"Country\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":2,\"LinkedEntityId\":null,\"Name\":\"Region\",\"IsMultiLink\":false}]},\"Children\":null},{\"EntityData\":{\"PrimitiveEntityName\":\"Address\",\"SourceCode\":null,\"MdmCode\":\"13\",\"DestCode\":\"13\",\"Attributes\":[{\"Name\":\"FIASCode\",\"AttributeType\":0,\"Value\":\"b2005455-9800-4652-b92c-bfb5aa9f140d\"},{\"Name\":\"Index\",\"AttributeType\":0,\"Value\":\"410052\"},{\"Name\":\"AddressString\",\"AttributeType\":0,\"Value\":\"г Саратов, ул Одесская, д 20А\"},{\"Name\":\"Address1\",\"AttributeType\":0,\"Value\":\"ул Одесская\"},{\"Name\":\"Address2\",\"AttributeType\":0,\"Value\":\"д 20А,  \"},{\"Name\":\"Address3\",\"AttributeType\":0,\"Value\":null}],\"LinkAttributes\":[{\"LinkedEntityInfoId\":3,\"LinkedEntityId\":678,\"Name\":\"City\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":8,\"LinkedEntityId\":null,\"Name\":\"AddressType\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":1,\"LinkedEntityId\":null,\"Name\":\"Country\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":2,\"LinkedEntityId\":null,\"Name\":\"Region\",\"IsMultiLink\":false}]},\"Children\":null},{\"EntityData\":{\"PrimitiveEntityName\":\"Contacts\",\"SourceCode\":null,\"MdmCode\":\"113\",\"DestCode\":\"113\",\"Attributes\":[{\"Name\":\"ContactInfo\",\"AttributeType\":0,\"Value\":\"89104748525\"}],\"LinkAttributes\":[{\"LinkedEntityInfoId\":6,\"LinkedEntityId\":8,\"Name\":\"ContactInfoType\",\"IsMultiLink\":false}]},\"Children\":null},{\"EntityData\":{\"PrimitiveEntityName\":\"CustomerCars\",\"SourceCode\":null,\"MdmCode\":\"21\",\"DestCode\":\"21\",\"Attributes\":[{\"Name\":\"Active\",\"AttributeType\":3,\"Value\":\"False\"}],\"LinkAttributes\":[{\"LinkedEntityInfoId\":10,\"LinkedEntityId\":829,\"Name\":\"VIN\",\"IsMultiLink\":false}]},\"Children\":null},{\"EntityData\":{\"PrimitiveEntityName\":\"CustomerCars\",\"SourceCode\":null,\"MdmCode\":\"22\",\"DestCode\":\"22\",\"Attributes\":[{\"Name\":\"Active\",\"AttributeType\":3,\"Value\":null}],\"LinkAttributes\":[{\"LinkedEntityInfoId\":10,\"LinkedEntityId\":826,\"Name\":\"VIN\",\"IsMultiLink\":false}]},\"Children\":null}]}"
            //"{\"MessageId\":\"00000000-0000-0000-0000-000000000000\",\"EntityName\":null,\"SourceName\":null,\"DestName\":null,\"EntityData\":{\"PrimitiveEntityName\":\"Customer\",\"SourceCode\":null,\"MdmCode\":\"228\",\"DestCode\":\"228\",\"Attributes\":[{\"Name\":\"Surname\",\"AttributeType\":0,\"Value\":\"Иванов\"},{\"Name\":\"Name\",\"AttributeType\":0,\"Value\":\"Ивн\"},{\"Name\":\"MiddleName\",\"AttributeType\":0,\"Value\":\"Иванович\"},{\"Name\":\"Birthday\",\"AttributeType\":2,\"Value\":\"03.01.2023 0:00:00\"},{\"Name\":\"MaritalStatus\",\"AttributeType\":0,\"Value\":null},{\"Name\":\"MrMs\",\"AttributeType\":4,\"Value\":\"Уважаемый\"},{\"Name\":\"MarketingAgree\",\"AttributeType\":3,\"Value\":\"True\"},{\"Name\":\"PersonalDataAgree\",\"AttributeType\":3,\"Value\":\"True\"},{\"Name\":\"ResearchAgree\",\"AttributeType\":3,\"Value\":null},{\"Name\":\"SuppressAllCalls\",\"AttributeType\":3,\"Value\":null},{\"Name\":\"SuppressAllEmails\",\"AttributeType\":3,\"Value\":null},{\"Name\":\"SuppressAllMessages\",\"AttributeType\":3,\"Value\":\"False\"},{\"Name\":\"EmailBounceBlock\",\"AttributeType\":3,\"Value\":null},{\"Name\":\"AddressBounceBlock\",\"AttributeType\":3,\"Value\":null},{\"Name\":\"CustomerGender\",\"AttributeType\":4,\"Value\":\"Мужской\"},{\"Name\":\"phonemobile\",\"AttributeType\":0,\"Value\":\"89104942215\"},{\"Name\":\"phonework\",\"AttributeType\":0,\"Value\":\"89206006030\"},{\"Name\":\"phonehome\",\"AttributeType\":0,\"Value\":\"84912203817\"},{\"Name\":\"mail\",\"AttributeType\":0,\"Value\":\"ivanovivan@yandex/ru\"}],\"LinkAttributes\":[{\"LinkedEntityInfoId\":4,\"LinkedEntityId\":2,\"Name\":\"CustomerType\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":12,\"LinkedEntityId\":315,\"Name\":\"PreferredDealer\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":12,\"LinkedEntityId\":243,\"Name\":\"SalesDealer\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":12,\"LinkedEntityId\":318,\"Name\":\"ServiceDealer\",\"IsMultiLink\":false}]},\"Children\":[{\"EntityData\":{\"PrimitiveEntityName\":\"Address\",\"SourceCode\":null,\"MdmCode\":\"10\",\"DestCode\":\"10\",\"Attributes\":[{\"Name\":\"FIASCode\",\"AttributeType\":0,\"Value\":null},{\"Name\":\"Index\",\"AttributeType\":0,\"Value\":null},{\"Name\":\"AddressString\",\"AttributeType\":0,\"Value\":null},{\"Name\":\"Address1\",\"AttributeType\":0,\"Value\":\"Строителей 25\"},{\"Name\":\"Address2\",\"AttributeType\":0,\"Value\":null},{\"Name\":\"Address3\",\"AttributeType\":0,\"Value\":null}],\"LinkAttributes\":[{\"LinkedEntityInfoId\":8,\"LinkedEntityId\":null,\"Name\":\"AddressType\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":1,\"LinkedEntityId\":null,\"Name\":\"Country\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":2,\"LinkedEntityId\":null,\"Name\":\"Region\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":3,\"LinkedEntityId\":71,\"Name\":\"City\",\"IsMultiLink\":false}]},\"Children\":null},{\"EntityData\":{\"PrimitiveEntityName\":\"Address\",\"SourceCode\":null,\"MdmCode\":\"11\",\"DestCode\":\"11\",\"Attributes\":[{\"Name\":\"FIASCode\",\"AttributeType\":0,\"Value\":null},{\"Name\":\"Index\",\"AttributeType\":0,\"Value\":null},{\"Name\":\"AddressString\",\"AttributeType\":0,\"Value\":null},{\"Name\":\"Address1\",\"AttributeType\":0,\"Value\":\"Одесская 20А\"},{\"Name\":\"Address2\",\"AttributeType\":0,\"Value\":null},{\"Name\":\"Address3\",\"AttributeType\":0,\"Value\":null}],\"LinkAttributes\":[{\"LinkedEntityInfoId\":8,\"LinkedEntityId\":null,\"Name\":\"AddressType\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":1,\"LinkedEntityId\":null,\"Name\":\"Country\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":2,\"LinkedEntityId\":null,\"Name\":\"Region\",\"IsMultiLink\":false},{\"LinkedEntityInfoId\":3,\"LinkedEntityId\":678,\"Name\":\"City\",\"IsMultiLink\":false}]},\"Children\":null}]}"
            ;
    }

    [Fact]
    public async Task Validate_EntityDto_And_Ensure_Result_Is_Not_Null()
    {
        var webFactory = new WebApplicationFactory<Program>();
        var client = webFactory.CreateClient();

        var requestJsonContent = JsonConvert.SerializeObject(_entityDto);
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
    public async Task Validate_EntityDtoJson_And_Ensure_Result_Is_Not_Null()
    {
        var webFactory = new WebApplicationFactory<Program>();
        var client = webFactory.CreateClient();

        var requestJsonContent = _entityDtoJson;
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
