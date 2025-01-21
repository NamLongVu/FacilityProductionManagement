1. Development of a service for hosting process equipment for production facilities.
2. API Security Key:
- Key: X-Api-Key
- Value:1A2B3C4D5E6F7G8H9I0J
3. Methods:
<br />a. Create a new equipment placement contract:
- Example input:
{
    "facilityCode": "FA03",
    "equipmentTypeCode": "EQ03",
    "quantity": 5
}
- Route: https://facilityapp-bye9ephug2fvbegv.polandcentral-01.azurewebsites.net/api/CreateContract
<br />b. Get the current list of contracts:
- Route: https://facilityapp-bye9ephug2fvbegv.polandcentral-01.azurewebsites.net/api/GetContracts
