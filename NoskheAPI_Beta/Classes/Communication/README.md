# Noskhe SignalR functions for mobile client
**(data are ONLY sent from server to client, client does not need to call any SignalR-related function on the server)**

## Configuration (joining and utilization):
  1. Connect to hub via this link: http://198.143.180.164:5000/NotificationHub , and
  2. Passing your valid token to the connection method in advance
  3. You will be disconnected from the hub under these circumstances:
	3.1. Automatically after closing the application
	3.2. Calling logout REST function on server
## Functions (which are needed to be implemeted on client side):
### Pharmacy Inquiry:
**Description**: Ersale ettela'ate darookhanei ke baraye anjame noskhe pichi eghdam karde
**Function header**: `PharmacyInquiry(string pharmacyName, string courierName, string phone);`
| Property | Description |
| --- | --- |
| pharmacyName | naame darookhane |
| courierName | naame peyk motori |
| phone | shomare e peyk motori |
### Invoice Details:
**Description**: Ersale ettela'ate factore hazinehaye sefaresh va ersal
**Function header**: `InvoiceDetails(int nofiticationId, decimal priceWithoutShippingCost, decimal shippingCost, string paymentUrl);`
| Property | Description |
| --- | --- |
| nofiticationId | id ye notification bara response tavasote client |
| priceWithoutShippingCost | hazineye factore sefaaresh |
| shippingCost | hazineye ersaal |
| paymentUrl | linke pardaakht hazineye kol (hazineye kol = priceWithoutShippingCost + shippingCost) |
### Cancellation Report:
**Description**: Vaghti hich daroo khanei mojud nabashe YA hame cancel karde bashan (naader hast)
**Function header**: `CancellationReport();`
