# external-services

Микросервис для взаимодействия с внешними API, который я разрабатывал для Navicon. 

На инфраструктурном уровне реализована логика работы с API сервиса DaData. Смысл заключается в обогащении сущности Entity (элемента справочника) на основе определенных полей. Возвращаемый объект Notification хранит список изменений сущности и список предупреждений валидации.

### Исходный элемент справочника для обогащения
```json
{
  "MessageId": "00000000-0000-0000-0000-000000000000",
  "EntityName": null,
  "SourceName": null,
  "DestName": null,
  "EntityData": {
    "PrimitiveEntityName": "Customer",
    "SourceCode": null,
    "MdmCode": "1",
    "DestCode": "1",
    "Attributes": [
      {
        "Name": "Surname",
        "AttributeType": 0,
        "Value": "иванов"
      },
      {
        "Name": "Name",
        "AttributeType": 0,
        "Value": "иван"
      },
      {
        "Name": "MiddleName",
        "AttributeType": 0,
        "Value": "иванович"
      },
      {
        "Name": "CustomerGender",
        "AttributeType": 4,
        "Value": null
      }
    ]
  }
}
```
###  Результат обогащения
```json
{
	"Alerts": [
		{
			"PrimitiveEntityName": "Customer",
			"MdmCode": "1",
			"AttributeName": "Surname",
			"SeverityType": "Info",
			"Message": "Сервис автоматической проверки успешно определил фамилию."
		},
		{
			"PrimitiveEntityName": "Customer",
			"MdmCode": "1",
			"AttributeName": "Name",
			"SeverityType": "Info",
			"Message": "Сервис автоматической проверки успешно определил имя."
		},
		{
			"PrimitiveEntityName": "Customer",
			"MdmCode": "1",
			"AttributeName": "MiddleName",
			"SeverityType": "Info",
			"Message": "Сервис автоматической проверки успешно определил отчество."
		},
		{
			"PrimitiveEntityName": "Customer",
			"MdmCode": "1",
			"AttributeName": "CustomerGender",
			"SeverityType": "Info",
			"Message": "Сервис автоматической проверки успешно определил пол."
		}
	],
	"Changes": [
		{
			"PrimitiveEntityName": "Customer",
			"MdmCode": "1",
			"AttributeName": "Surname",
			"Value": "Иванов"
		},
		{
			"PrimitiveEntityName": "Customer",
			"MdmCode": "1",
			"AttributeName": "Name",
			"Value": "Иван"
		},
		{
			"PrimitiveEntityName": "Customer",
			"MdmCode": "1",
			"AttributeName": "MiddleName",
			"Value": "Иванович"
		},
		{
			"PrimitiveEntityName": "Customer",
			"MdmCode": "1",
			"AttributeName": "CustomerGender",
			"Value": "М"
		}
	]
}
```
